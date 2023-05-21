using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Class DataService.
/// </summary>
public class BudgetDataService : IBudgetDataService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IBlobStorageService blobStorageService;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetDataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="queryBus">IQueryBus.</param>
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
		var download = await this.blobStorageService.DownloadFromBlobStorage(Path.GetFileName(filename), containerClient);
		using (var reader = new StreamReader(download.Content))
		{
			using (var csv = new CsvReader(reader, csvConfig))
			{
				csv.Read();
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
			}
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
	// TODO: zrob z tego extension method
	public GetBudgetTransferInfo? Create(GetBudgetTransferInfo budget, GetBudgetsNameInfo? budgetsNames)
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
		var errors = new List<string>();

		if (string.IsNullOrEmpty(budget.Name))
		{
			errors.Add("Budget name is missing");
		}

		if (string.IsNullOrEmpty(budget.IconName))
		{
			errors.Add("Budget icon name is missing");
		}

		if (string.IsNullOrEmpty(budget.Currency))
		{
			errors.Add("Budget currency is missing");
		}

		if (string.IsNullOrEmpty(budget.Value))
		{
			errors.Add("Budget value is missing");
		}
		else if (!decimal.TryParse(budget.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
		{
			errors.Add("Budget value is not a valid decimal number");
		}

		if (string.IsNullOrEmpty(budget.StartDate))
		{
			errors.Add("Budget start date is missing");
		}
		else if (!DateTime.TryParse(budget.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget start date is not a valid date");
		}

		if (string.IsNullOrEmpty(budget.EndDate))
		{
			errors.Add("Budget end date is missing");
		}
		else if (!DateTime.TryParse(budget.EndDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget end date is not a valid date");
		}

		if (!string.IsNullOrEmpty(budget.StartDate) && !string.IsNullOrEmpty(budget.EndDate) &&
			DateTime.Parse(budget.StartDate) >= DateTime.Parse(budget.EndDate))
		{
			errors.Add("Budget start date cannot be later than or equal to end date");
		}

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
		using var stream = file.OpenReadStream();
		using (var csv = new CsvReader(new StreamReader(stream), csvConfig))
		{
			csv.Read();
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
				budgetInfos.Add(updateBudget!);
			}
		}

		return new GetBudgetTransferList { BudgetsList = budgetInfos };
	}
}