using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Import;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetImportService class provides functionalities to import budgets from a .csv file.
/// </summary>
public class BudgetImportService : IBudgetImportService
{
	private readonly IBlobStorageService blobStorageService;
	private readonly IBudgetDataService budgetDataService;
	private readonly ICsvService<GetBudgetTransferInfo> csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetImportService"/> class.
	/// </summary>
	/// <param name="blobStorageService">The service responsible for interacting with the blob storage.</param>
	/// <param name="budgetDataService">The service responsible for accessing budget-related data.</param>
	/// <param name="csvService">The service responsible for CSV file operations.</param>
	public BudgetImportService(IBlobStorageService blobStorageService, IBudgetDataService budgetDataService, ICsvService<GetBudgetTransferInfo> csvService)
	{
		this.blobStorageService = blobStorageService;
		this.budgetDataService = budgetDataService;
		this.csvService = csvService;
	}

	/// <summary>
	/// Imports budgets from a provided .csv file, validates them, and stores them in the system.
	/// </summary>
	/// <param name="file">The .csv file containing the budgets to be imported.</param>
	/// <returns>A tuple containing a list of any errors encountered during the import process and
	/// the URI of the saved .csv file in the Azure Blob Storage if any budgets were successfully imported.
	/// If no budgets were imported, the URI is replaced with a message stating "No budgets were saved.".</returns>
	public async Task<GetImportResult> Import(IFormFile file)
	{
		var errors = new List<string>();

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		var budgetInfos = this.budgetDataService.CreateValidBudgetsList(file, csvConfig, errors);

		if (budgetInfos.Result.BudgetsList.Count == 0)
		{
			return new GetImportResult(
				new BudgetAggregateList(new List<BudgetAggregate>()),
				new ImportResult { ErrorsList = errors, Uri = "No budgets were saved." });
		}

		string fileName = this.csvService.GenerateFileNameWithCsvExtension();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(budgetInfos.Result.BudgetsErrorsList, csvWriter);
			memoryStream.Position = 0;

			await this.blobStorageService.UploadToBlobStorage(memoryStream, fileName);
		}

		string uri = await this.blobStorageService.GenerateLinkToDownload(fileName);

		var budgetsAggregateList = await this.budgetDataService.MapFrom(budgetInfos.Result.BudgetsList);

		if (budgetInfos.Result.BudgetsErrorsList.Count == 0)
		{
			return new GetImportResult(budgetsAggregateList, new ImportResult { ErrorsList = errors, Uri = "All budgets were saved." });
		}

		return new GetImportResult(budgetsAggregateList, new ImportResult { ErrorsList = errors, Uri = uri });
	}
}