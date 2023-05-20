using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper.Configuration;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Modules.Budget.Application.Data.Service;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class ImportBudgetsFromBlobStorage.
/// </summary>
public class ImportBudgetsFromBlobStorage
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BlobStorageService blobStorageService;

	/// <summary>
	/// Initializes a new instance of the <see cref="ImportBudgetsFromBlobStorage"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="blobStorageService">BlobStorageService.</param>
	public ImportBudgetsFromBlobStorage(IExecutionContextAccessor contextAccessor, BlobStorageService blobStorageService)
	{
		this.contextAccessor = contextAccessor;
		this.blobStorageService = blobStorageService;
	}

	/// <summary>
	/// Downloads a CSV file containing a list of budgets from Azure Blob Storage and imports the budgets into the application.
	/// </summary>
	/// <param name="filename">The name of the blob to be downloaded from Azure Blob Storage.</param>
	/// <param name="containerClient">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	public async Task<BudgetAggregateList> Import(string filename, BlobContainerClient containerClient, CsvConfiguration csvConfig)
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
					string description = string.IsNullOrEmpty(budget.Description) ? string.Empty : budget.Description;
					var newBudget = BudgetAggregate.Create(budgetId, budget.Name, userId, money, period, description, budget.IconName);
					newBudgets.Add(newBudget);
				}
			}
		}

		return new BudgetAggregateList(newBudgets);
	}
}