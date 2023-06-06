using System.Globalization;

using CsvHelper;

using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

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
	/// <param name="blobStorageService">An instance of a class implementing IBlobStorageService interface.
	/// This service is used for interacting with Azure Blob Storage, such as uploading the CSV file and generating a download link.</param>
	/// <param name="csvService">An instance of a class implementing ICsvService interface of type GetBudgetTransferInfo.
	/// This service is used for CSV-related operations like generating a CSV filename and writing records to a CSV file.</param>
	public BudgetExportService(IBlobStorageService blobStorageService, ICsvService<GetBudgetTransferInfo> csvService)
	{
		this.blobStorageService = blobStorageService;
		this.csvService = csvService;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">A GetTransferList object which encapsulates a list of budgets to be exported.
	/// Each budget contains details like name, value, start date, end date, and other attributes.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<ExportResult> Export(GetTransferList<GetBudgetTransferInfo>? budgets)
	{
		string filename = this.csvService.GenerateFileNameWithCsvExtension();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(budgets!.CorrectList, csv);
			memoryStream.Position = 0;

			await this.blobStorageService.UploadToBlobStorage(memoryStream, filename);
		}

		string uri = await this.blobStorageService.GenerateLinkToDownload(filename);

		return new ExportResult { Uri = uri };
	}
}