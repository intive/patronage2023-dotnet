using System.Globalization;

using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

using CsvHelper;

using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// BlobStorageService class implements the IBlobStorageService interface and provides methods
/// for managing blob storage operations such as uploading, retrieving, and deleting blobs.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
	private readonly BlobServiceClient blobServiceClient;
	private readonly ICsvService csvService;
	private readonly IConfiguration configuration;

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
		this.configuration = configuration;
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
		string filename = this.csvService.GenerateFileName();
		BlobClient blobClient = containerClient.GetBlobClient(filename);

		var memoryStream = new MemoryStream();
		var streamWriter = new StreamWriter(memoryStream);
		var csv = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

		try
		{
			this.csvService.WriteBudgetsToMemoryStream(budgetInfos, csv);
			memoryStream.Position = 0;

			await blobClient.UploadAsync(memoryStream, true);

			string? connectionString = this.configuration.GetConnectionString("BlobStorage");
			string accountKey = connectionString!.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1].Split(';')[0];
			string blobSasUri = this.GenerateSasForBlob(blobClient, accountKey);

			return blobSasUri;
		}
		finally
		{
			await csv.DisposeAsync();
			await streamWriter.DisposeAsync();
			await memoryStream.DisposeAsync();
		}
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

	/// <summary>
	/// Generates a Shared Access Signature (SAS) for a specific blob within Azure Storage.
	/// </summary>
	/// <param name="blobClient">An instance of BlobClient which provides access to a specific blob in Azure Storage.</param>
	/// <param name="accountKey">The account key for your Azure Storage account. It is used in conjunction with the BlobClient to generate the SAS token.</param>
	/// <returns>
	/// A string representing the URI of the blob, with the generated SAS token appended as a query string.</returns>
	private string GenerateSasForBlob(BlobClient blobClient, string accountKey)
	{
		var sasBuilder = new BlobSasBuilder
		{
			BlobContainerName = blobClient.BlobContainerName,
			BlobName = blobClient.Name,
			Resource = "b",
			StartsOn = DateTimeOffset.UtcNow.AddMinutes(-5),
			ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
		};

		sasBuilder.SetPermissions(BlobSasPermissions.Read);

		var storageSharedKeyCredential = new StorageSharedKeyCredential(blobClient.AccountName, accountKey);
		string sasToken = sasBuilder.ToSasQueryParameters(storageSharedKeyCredential).ToString();

		var sasUri = new UriBuilder(blobClient.Uri)
		{
			Query = sasToken,
		};

		return sasUri.ToString();
	}
}