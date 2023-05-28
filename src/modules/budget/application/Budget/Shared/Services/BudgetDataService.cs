using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetDataService class implements the IBudgetDataService interface and provides methods
/// for data operations related to budgets.
/// </summary>
public class BudgetDataService : IBudgetDataService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IBlobStorageService blobStorageService;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetDataService"/> class.
	/// </summary>
	/// <param name="contextAccessor">Provides access to the execution context, allowing the service to use user-specific information.</param>
	/// <param name="blobStorageService">Provides functionality for interacting with Azure Blob Storage.</param>
	/// <param name="queryBus">Enables the service to dispatch queries to the appropriate handlers.</param>
	public BudgetDataService(IExecutionContextAccessor contextAccessor, IBlobStorageService blobStorageService, IQueryBus queryBus)
	{
		this.contextAccessor = contextAccessor;
		this.blobStorageService = blobStorageService;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Downloads a CSV file containing a list of budgets from Azure Blob Storage and imports the budgets into the application.
	/// </summary>
	/// <param name="filename">The name of the blob to be downloaded from Azure Blob Storage.</param>
	/// <param name="containerClient">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task<BudgetAggregateList> ConvertBudgetsFromCsvToBudgetAggregate(string filename, BlobContainerClient containerClient, CsvConfiguration csvConfig)
	{
		var newBudgets = new List<BudgetAggregate>();
		var download = await this.blobStorageService.DownloadFromBlobStorage(filename);
		using var reader = new StreamReader(download.Content);
		using var csv = new CsvReader(reader, csvConfig);
		await csv.ReadAsync();
		var budgetsToImport = csv.GetRecords<GetBudgetTransferInfo>();

		foreach (var budget in budgetsToImport)
		{
			var budgetId = new BudgetId(Guid.NewGuid());
			var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
			decimal limit = decimal.Parse(budget.Value, CultureInfo.InvariantCulture);
			var money = new Money(limit, (Currency)Enum.Parse(typeof(Currency), budget.Currency));
			var period = new Period(DateTime.Parse(budget.StartDate), DateTime.Parse(budget.EndDate));
			string description = budget.Description ?? string.Empty;
			var newBudget = BudgetAggregate.Create(budgetId, budget.Name, userId, money, period, description, budget.IconName);
			newBudgets.Add(newBudget);
		}

		return new BudgetAggregateList(newBudgets);
	}

	/// <summary>
	/// Creates a new budget based on the provided budget information.
	/// If a budget with the same name already exists in the database, a random number is appended to the name.
	/// </summary>
	/// <param name="budget">The budget information used to create the new budget.</param>
	/// <param name="budgetsNames">The budget information used to create the new budget2.</param>
	/// <returns>Creates a new budget.</returns>
	public GetBudgetTransferInfo Create(GetBudgetTransferInfo budget, GetBudgetsNameInfo? budgetsNames)
	{
		bool isExistingBudget = budgetsNames!.BudgetName!.Contains(budget.Name);
		string budgetName = isExistingBudget ? budget.Name + Guid.NewGuid() : budget.Name;
		budgetsNames.BudgetName.Add(budgetName);

		var budgetInfo = new GetBudgetTransferInfo
		{
			Name = budgetName,
			IconName = budget.IconName,
			Description = budget.Description,
			Currency = budget.Currency,
			Value = budget.Value,
			StartDate = budget.StartDate,
			EndDate = budget.EndDate,
		};

		return budgetInfo;
	}

	/// <summary>
	/// Validates the properties of a budget object.
	/// </summary>
	/// <param name="budget">The budget object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget object is valid.</returns>
	public List<string> BudgetValidate(GetBudgetTransferInfo budget)
	{
		var validator = new GetBudgetTransferInfoValidator();
		var validationResult = validator.Validate(budget);

		var errors = new List<string>();
		errors.AddErrors(validationResult);

		return errors;
	}

	/// <summary>
	/// Reads budgets from a CSV file, validates them, and returns a list of valid budgets.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="file">The CSV file containing the budgets to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budgets read from the CSV file.</returns>
	public async Task<GetBudgetTransferList> CreateValidBudgetsList(IFormFile file, CsvConfiguration csvConfig, List<string> errors)
	{
		var budgetInfos = new List<GetBudgetTransferInfo>();
		var budgetsNames = await this.queryBus.Query<GetBudgetsName, GetBudgetsNameInfo?>(new GetBudgetsName());
		await using var stream = file.OpenReadStream();
		using var streamReader = new StreamReader(stream);
		using var csv = new CsvReader(streamReader, csvConfig);
		await csv.ReadAsync();
		var budgets = csv.GetRecords<GetBudgetTransferInfo>().ToList();
		int rowNumber = 0;

		foreach (var budget in budgets)
		{
			var results = this.BudgetValidate(budget);
			rowNumber++;

			if (results.Any())
			{
				foreach (string result in results)
				{
					errors.Add($"row: {rowNumber}| error: {result}");
				}

				continue;
			}

			var updateBudget = this.Create(budget, budgetsNames);
			budgetInfos.Add(updateBudget);
		}

		return new GetBudgetTransferList { BudgetsList = budgetInfos };
	}
}