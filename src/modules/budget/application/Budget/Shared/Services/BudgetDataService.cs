using System.Globalization;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetDataService class implements the IBudgetDataService interface and provides methods
/// for data operations related to budgets.
/// </summary>
public class BudgetDataService : IBudgetDataService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IQueryBus queryBus;
	private readonly ICsvService<GetBudgetTransferInfo> csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetDataService"/> class.
	/// </summary>
	/// <param name="contextAccessor">Provides access to the execution context, allowing the service to use user-specific information.</param>
	/// <param name="queryBus">Enables the service to dispatch queries to the appropriate handlers.</param>
	/// <param name="csvService">The service responsible for CSV file operations.</param>
	public BudgetDataService(IExecutionContextAccessor contextAccessor, IQueryBus queryBus, ICsvService<GetBudgetTransferInfo> csvService)
	{
		this.contextAccessor = contextAccessor;
		this.queryBus = queryBus;
		this.csvService = csvService;
	}

	/// <summary>
	/// Converts a collection of budget information from CSV format into a list of BudgetAggregate objects.
	/// </summary>
	/// <param name="budgetsToImport">Collection of budget information to be converted, represented as GetBudgetTransferInfo objects.</param>
	/// <returns>A Task containing a BudgetAggregateList, representing the converted budget information.</returns>
	public Task<BudgetAggregateList> MapFrom(IEnumerable<GetBudgetTransferInfo> budgetsToImport)
	{
		var newBudgets = new List<BudgetAggregate>();
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

		return Task.FromResult(new BudgetAggregateList(newBudgets));
	}

	/// <summary>
	/// Creates a new budget based on the provided budget information.
	/// If a budget with the same name already exists in the database, a random number is appended to the name.
	/// </summary>
	/// <param name="budget">The budget information used to create the new budget.</param>
	/// <param name="budgetsNames">The existing budget's names used for checking whether the new budget's name already exists in the database.</param>
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
	public async Task<GetTransferList<GetBudgetTransferInfo>> CreateValidBudgetsList(IFormFile file, CsvConfiguration csvConfig, List<string> errors)
	{
		var validBudgets = new List<GetBudgetTransferInfo>();
		var invalidBudgets = new List<GetBudgetTransferInfo>();
		var budgetsNames = await this.queryBus.Query<GetBudgetsName, GetBudgetsNameInfo?>(new GetBudgetsName());

		var budgets = await this.csvService.GetRecordsFromCsv<GetBudgetTransferInfo>(file, csvConfig);
		int rowNumber = 1;

		foreach (var budget in budgets)
		{
			var results = this.BudgetValidate(budget);

			if (results.Any())
			{
				rowNumber++;
				foreach (string result in results)
				{
					errors.Add($"row: {rowNumber} | error: {result}");
				}

				invalidBudgets.Add(budget);
				continue;
			}

			var updateBudget = this.Create(budget, budgetsNames);
			validBudgets.Add(updateBudget);
		}

		return new GetTransferList<GetBudgetTransferInfo> { CorrectList = validBudgets, ErrorsList = invalidBudgets };
	}
}