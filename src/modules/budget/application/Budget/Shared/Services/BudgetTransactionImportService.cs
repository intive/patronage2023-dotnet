using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Import;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetTransactionImportService class provides functionalities to import budget transactions from a .csv file.
/// </summary>
public class BudgetTransactionImportService : IBudgetTransactionImportService
{
	private readonly IBlobStorageService blobStorageService;
	private readonly IBudgetTransactionDataService budgetTransactionDataService;
	private readonly ICsvService<GetBudgetTransactionTransferInfo> csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionImportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="budgetTransactionDataService">IDataHelper.</param>
	/// <param name="csvService">GetBudgetTransferList.</param>
	public BudgetTransactionImportService(IBlobStorageService blobStorageService, IBudgetTransactionDataService budgetTransactionDataService, ICsvService<GetBudgetTransactionTransferInfo> csvService)
	{
		this.blobStorageService = blobStorageService;
		this.budgetTransactionDataService = budgetTransactionDataService;
		this.csvService = csvService;
	}

	/// <summary>
	/// Imports budget transactions from a provided .csv file, validates them, and stores them in the system.
	/// </summary>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <param name="file">The .csv file containing the transactions to be imported.</param>
	/// <returns>A tuple containing a list of any errors encountered during the import process and
	/// the URI of the saved .csv file in the Azure Blob Storage if any budget transactions were successfully imported.
	/// If no budget transactions were imported, the URI is replaced with a message stating "No budget transactions were saved.".</returns>
	public async Task<GetImportResult<BudgetTransactionAggregateList>> Import(BudgetId budgetId, IFormFile file)
	{
		var errors = new List<string>();

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		var budgetTransactionInfos = await this.budgetTransactionDataService.CreateValidBudgetTransactionsList(budgetId, file, csvConfig, errors);

		if (budgetTransactionInfos.CorrectList.Count == 0)
		{
			return new GetImportResult<BudgetTransactionAggregateList>(
				new BudgetTransactionAggregateList(new List<BudgetTransactionAggregate>()),
				new ImportResult { ErrorsList = errors, Uri = "No budget transactions were saved." });
		}

		string fileName = this.csvService.GenerateFileNameWithCsvExtension();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			var errorBudgetTransactions = budgetTransactionInfos.ErrorsList.Select(x => (GetBudgetTransactionTransferInfo)x);
			this.csvService.WriteRecordsToMemoryStream(errorBudgetTransactions, csvWriter);
			memoryStream.Position = 0;

			await this.blobStorageService.UploadToBlobStorage(memoryStream, fileName);
		}

		string uri = await this.blobStorageService.GenerateLinkToDownload(fileName);

		var budgetTransactionsAggregateList = await this.budgetTransactionDataService.MapFrom(budgetTransactionInfos.CorrectList, csvConfig);

		if (budgetTransactionInfos.ErrorsList.Count == 0)
		{
			return new GetImportResult<BudgetTransactionAggregateList>(budgetTransactionsAggregateList, new ImportResult
			{
				ErrorsList = errors,
				Uri = "All transactions were saved.",
			});
		}

		return new GetImportResult<BudgetTransactionAggregateList>(budgetTransactionsAggregateList, new ImportResult { ErrorsList = errors, Uri = uri });
	}
}