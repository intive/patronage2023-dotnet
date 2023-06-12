using System.Globalization;

using CsvHelper;

using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

using FileDescriptor = Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export.FileDescriptor;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetTransactionExportService class provides functionalities to export budget transaction to a .csv file and upload it to Azure Blob Storage.
/// </summary>
public class BudgetTransactionExportService : IBudgetTransactionExportService
{
	private readonly IBlobStorageService blobStorageService;
	private readonly ICsvService<GetBudgetTransactionTransferInfo> csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionExportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="csvService">1.</param>
	public BudgetTransactionExportService(IBlobStorageService blobStorageService, ICsvService<GetBudgetTransactionTransferInfo> csvService)
	{
		this.blobStorageService = blobStorageService;
		this.csvService = csvService;
	}

	/// <summary>
	/// Exports the budget transactions to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="transactions">GetBudgetTransactionList To Export.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<ExportResult> ExportToStorage(GetTransferList<GetBudgetTransactionTransferInfo>? transactions)
	{
		string filename = this.csvService.GenerateFileNameWithCsvExtension();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(transactions!.CorrectList, csv);
			memoryStream.Position = 0;

			await this.blobStorageService.UploadToBlobStorage(memoryStream, filename);
		}

		string uri = await this.blobStorageService.GenerateLinkToDownload(filename);

		return new ExportResult { Uri = uri };
	}

	/// <inheritdoc/>
	public async Task<FileDescriptor> Export(GetTransferList<GetBudgetTransactionTransferInfo>? transactions)
	{
		byte[] content = Array.Empty<byte>();
		using (var memoryStream = new MemoryStream())
		await using (var streamWriter = new StreamWriter(memoryStream))
		await using (var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(transactions!.CorrectList, csv);
			memoryStream.Position = 0;
			content = new byte[memoryStream.Length];
			memoryStream.Read(content, 0, content.Length);
		}

		return new FileDescriptor("Budget transactions.csv", content);
	}
}