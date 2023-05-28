using System.Globalization;

using CsvHelper;

using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetExportService class provides functionalities to export budgets to a .csv file and upload it to Azure Blob Storage.
/// </summary>
public class BudgetExportService : IBudgetExportService
{
	private readonly IBlobStorageService blobStorageService;
	private readonly ICsvService<GetBudgetTransferInfo> csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetExportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="csvService">1.</param>
	public BudgetExportService(IBlobStorageService blobStorageService, ICsvService<GetBudgetTransferInfo> csvService)
	{
		this.blobStorageService = blobStorageService;
		this.csvService = csvService;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">GetBudgetsListToExport.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<string?> Export(GetBudgetTransferList? budgets)
	{
		string filename = this.csvService.GenerateFileNameWithCsvExtension();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(budgets!.BudgetsList, csv);
			memoryStream.Position = 0;

			await this.blobStorageService.UploadToBlobStorage(memoryStream, filename);
		}

		return await this.blobStorageService.GenerateLinkToDownload(filename);
	}
}