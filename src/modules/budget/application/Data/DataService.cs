using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Class DataService.
/// </summary>
public class DataService
{
	private readonly IQueryBus queryBus;
	private readonly BlobServiceClient blobServiceClient;

	/////// <summary>
	/////// Initializes a new instance of the <see cref="DataService"/> class.
	/////// DataService.
	/////// </summary>
	////public DataService()
	////{
	////}

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="queryBus">QueryBus.</param>
	/// <param name="configuration">IConfiguration.</param>
	public DataService(IQueryBus queryBus, IConfiguration configuration)
	{
		this.queryBus = queryBus;
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
	}

	/// <summary>
	/// Method to export budgets to CSV file.
	/// </summary>
	/// <returns>CSV file.</returns>
	public async Task Export()
	{
		// Create a unique name for the container
		string containerName = "csv";

		var query = new GetBudgetsToExport() { };
		var budgets = await this.queryBus.Query<GetBudgetsToExport, List<GetBudgetsToExportInfo>?>(query);

		BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();

		// Create a local file in the ./data/ directory for uploading and downloading
		string localPath = "data";
		Directory.CreateDirectory(localPath);
		string fileName = "quickstart" + Guid.NewGuid().ToString() + ".csv";
		string localFilePath = Path.Combine(localPath, fileName);

		// Write text to the file
		using (var writer = new StreamWriter(localFilePath))
		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteHeader<GetBudgetsToExportInfo>();
			csv.NextRecord();
			foreach (var budget in budgets!)
			{
				csv.WriteRecord(budget);
				csv.NextRecord();
			}
		}

		// Get a reference to a blob
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		// Upload data from the local file
		await blobClient.UploadAsync(localFilePath, true);

		// Delete the local file after uploading to Azure Blob Storage
		File.Delete(localFilePath);
	}
}