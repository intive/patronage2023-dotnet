using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Class BlobStorageService.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
	private readonly BlobServiceClient blobServiceClient;
	private readonly ICsvService csvService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BlobStorageService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="configuration">The application's configuration, used for retrieving the connection string for the Blob Storage.</param>
	/// <param name="csvService">GenerateLocalCsvFilePath.</param>
	public BlobStorageService(IConfiguration configuration, ICsvService csvService)
	{
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
		this.csvService = csvService;
	}

	/// <summary>
	/// Checks if a blob container exists, and if not, creates one.
	/// </summary>
	/// <param name="containerName">The name of the container to be checked/created.</param>
	/// <returns>A client reference to the newly created or existing blob container.</returns>
	public async Task<BlobContainerClient> CreateBlobContainerIfNotExists(string containerName)
	{
		var containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();
		return containerClient;
	}

	/// <summary>
	/// Uploads a CSV file containing a list of budgets to Azure Blob Storage.
	/// </summary>
	/// <param name="budgetInfos">A list of budgets to be written to the CSV file and uploaded.</param>
	/// <param name="containerClient">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <returns>The absolute URI of the uploaded blob in Azure Blob Storage.</returns>
	public async Task<string> UploadToBlobStorage(GetBudgetTransferList budgetInfos, BlobContainerClient containerClient)
	{
		string localFilePath = this.csvService.GeneratePathToCsvFile();
		string filePath = this.csvService.WriteBudgetsToCSV(budgetInfos, localFilePath);

		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

		await blobClient.UploadAsync(filePath, true);
		File.Delete(filePath);

		// Utworzenie polityki dostępu na podstawie których wygenerujemy SAS
		var sasBuilder = new BlobSasBuilder
		{
			BlobContainerName = containerClient.Name,
			BlobName = blobClient.Name,
			Resource = "b",
			StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
			ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
		};

		// Ustalenie uprawnień - tutaj ustawiamy na czytanie
		sasBuilder.SetPermissions(BlobSasPermissions.Read);

		// Generowanie SAS
		var storageSharedKeyCredential = new StorageSharedKeyCredential(containerClient.AccountName, "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==");
		string sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

		// Dodanie SAS do URL Blobu
		var sasUri = new UriBuilder(blobClient.Uri)
		{
			Query = sasToken,
		};

		return sasUri.ToString();

		////return blobClient.Uri.AbsoluteUri;
	}

	/// <summary>
	/// Downloads a specified file from Azure Blob Storage.
	/// </summary>
	/// <param name="filename">The name of the file to be downloaded.</param>
	/// <param name="containerClient">A client object for interacting with the Azure Blob Storage container.</param>
	/// <returns>A task representing the asynchronous operation, yielding the downloaded file's information.</returns>
	public async Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename, BlobContainerClient containerClient)
	{
		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filename));
		BlobDownloadInfo download = await blobClient.DownloadAsync();
		return download;
	}
}