using System.Globalization;
using CsvHelper.Configuration;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetDataService class implements the IBudgetDataService interface and provides methods
/// for data operations related to budgets.
/// </summary>
public class BudgetTransactionDataService : IBudgetTransactionDataService
{
	private readonly IValidator<GetBudgetTransactionImportInfo> validator;
	private readonly ICsvService<GetBudgetTransactionTransferInfo> csvService;
	private readonly IKeycloakService keycloak;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionDataService"/> class.
	/// </summary>
	/// <param name="validator">Import info validator.</param>
	/// <param name="csvService">Service to handle csv files.</param>
	/// <param name="keycloak">Keycloak.</param>
	/// <param name="contextAccessor">Context accessor.</param>
	public BudgetTransactionDataService(IValidator<GetBudgetTransactionImportInfo> validator, ICsvService<GetBudgetTransactionTransferInfo> csvService, IKeycloakService keycloak, IExecutionContextAccessor contextAccessor)
	{
		this.validator = validator;
		this.csvService = csvService;
		this.keycloak = keycloak;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// Converts a collection of budget transaction information from CSV format into a list of BudgetTransactionAggregate objects.
	/// </summary>
	/// <param name="budgetTransactionsToImport">Collection of budget transactions information to be converted, represented as GetBudgetTransactionTransferInfo objects.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <returns>A Task containing a BudgetAggregateList, representing the converted budget information.</returns>
	public async Task<BudgetTransactionAggregateList> MapFrom(IEnumerable<GetBudgetTransactionImportInfo> budgetTransactionsToImport, CsvConfiguration csvConfig)
	{
		var newBudgetTransactions = new List<BudgetTransactionAggregate>();
		foreach (var transaction in budgetTransactionsToImport)
		{
			var transactionId = new TransactionId(Guid.NewGuid());
			decimal value = decimal.Parse(transaction.Value, CultureInfo.InvariantCulture);
			var userid = this.contextAccessor.GetUserId();
			string email = await this.keycloak.GetEmailById(userid.ToString()!, CancellationToken.None);
			var transactionType = (TransactionType)Enum.Parse(typeof(TransactionType), transaction.TransactionType);
			var categoryType = new CategoryType(transaction.CategoryType);
			var budgetTransactionDate = DateTime.Parse(transaction.Date);
			var status = (Status)Enum.Parse(typeof(Status), transaction.Status);
			var newBudgetTransaction = BudgetTransactionAggregate.Create(transactionId, transaction.BudgetId, transactionType, transaction.Name, email, value, categoryType, budgetTransactionDate, status);
			newBudgetTransactions.Add(newBudgetTransaction);
		}

		return await Task.FromResult(new BudgetTransactionAggregateList(newBudgetTransactions));
	}

	/// <summary>
	/// Validates the properties of a budget transaction object.
	/// </summary>
	/// <param name="budgetTransaction">The budget transaction object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget object is valid.</returns>
	public async Task<List<string>> BudgetTransactionValidate(GetBudgetTransactionImportInfo budgetTransaction)
	{
		var validationResult = await this.validator.ValidateAsync(budgetTransaction);

		var errors = new List<string>();
		errors.AddErrors(validationResult);

		return errors;
	}

	/// <summary>
	/// Reads budgets from a CSV file, validates them, and returns a list of valid budgets.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <param name="file">The CSV file containing the budgets to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budgets read from the CSV file.</returns>
	public async Task<GetTransferList<GetBudgetTransactionImportInfo>> CreateValidBudgetTransactionsList(BudgetId budgetId, IFormFile file, CsvConfiguration csvConfig, List<string> errors)
	{
		var validBudgetTransactions = new List<GetBudgetTransactionImportInfo>();
		var invalidBudgetTransactions = new List<GetBudgetTransactionImportInfo>();

		var budgetTransactionsTransfer = await this.csvService.GetRecordsFromCsv<GetBudgetTransactionTransferInfo>(file, csvConfig);
		var budgetTransactions = budgetTransactionsTransfer.MapToBudgetTransactionImportInfo(budgetId);
		int rowNumber = 1;

		foreach (var budget in budgetTransactions)
		{
			var results = await this.BudgetTransactionValidate(budget);

			if (results.Any())
			{
				rowNumber++;
				foreach (string result in results)
				{
					errors.Add($"row: {rowNumber} | error: {result}");
				}

				invalidBudgetTransactions.Add(budget);
				continue;
			}

			validBudgetTransactions.Add(budget);
		}

		return new GetTransferList<GetBudgetTransactionImportInfo> { CorrectList = validBudgetTransactions, ErrorsList = invalidBudgetTransactions };
	}
}