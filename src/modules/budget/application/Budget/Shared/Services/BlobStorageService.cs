using System.Globalization;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BlobStorageService class implements the IBlobStorageService interface and provides methods
/// for managing blob storage operations such as uploading, retrieving, and deleting blobs.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
	private readonly BlobServiceClient blobServiceClient;
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IConfiguration configuration;

	/// <summary>
	/// Initializes a new instance of the <see cref="BlobStorageService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="configuration">The application's configuration, used for retrieving the connection string for the Blob Storage.</param>
	/// <param name="contextAccessor">1.</param>
	public BlobStorageService(IConfiguration configuration, IExecutionContextAccessor contextAccessor)
	{
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
		this.contextAccessor = contextAccessor;
		this.configuration = configuration;
	}

	/// <summary>
	/// 1.
	/// </summary>
	/// <param name="filename">12.</param>
	/// <returns>11.</returns>
	public async Task<string> GenerateLinkToDownload(string filename)
	{
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists();
		BlobClient blobClient = containerClient.GetBlobClient(filename);
		string? connectionString = this.configuration.GetConnectionString("BlobStorage");
		string accountKey = connectionString!.Split(new[] { "AccountKey=" }, StringSplitOptions.None)[1].Split(';')[0];
		string blobSasUri = this.GenerateSasForBlob(blobClient, accountKey);

		return blobSasUri;
	}

	/// <summary>
	/// Uploads a CSV file containing a list of budgets to Azure Blob Storage.
	/// </summary>
	/// <param name="stream">.</param>
	/// <param name="filename">1.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	public async Task UploadToBlobStorage(Stream stream, string filename)
	{
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists();
		BlobClient blobClient = containerClient.GetBlobClient(filename);
		await blobClient.UploadAsync(stream, true);
	}

	/// <summary>
	/// Downloads a specified file from Azure Blob Storage.
	/// </summary>
	/// <param name="filename">The name of the file to be downloaded.</param>
	/// <returns>A task representing the asynchronous operation, yielding the downloaded file's information.</returns>
	public async Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename)
	{
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists();
		BlobClient blobClient = containerClient.GetBlobClient(filename);
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

	/// <summary>
	/// Checks if a blob container exists, and if not, creates one.
	/// </summary>
	/// <returns>A client reference to the newly created or existing blob container.</returns>
	private async Task<BlobContainerClient> CreateBlobContainerIfNotExists()
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		var containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();
		return containerClient;
	}
}