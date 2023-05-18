using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Class BlobStorageService.
/// </summary>
public class BlobStorageService
{
	private readonly BlobServiceClient blobServiceClient;
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly GenerateLocalCsvFilePath generateLocalCsvFilePath;
	private readonly WriteBudgetsToCsvFile writeBudgetsToCsvFile;

	/// <summary>
	/// Initializes a new instance of the <see cref="BlobStorageService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="configuration">The application's configuration, used for retrieving the connection string for the Blob Storage.</param>
	/// <param name="generateLocalCsvFilePath">GenerateLocalCsvFilePath.</param>
	/// <param name="writeBudgetsToCsvFile">WriteBudgetsToCsvFile.</param>
	public BlobStorageService(IConfiguration configuration, IExecutionContextAccessor contextAccessor, GenerateLocalCsvFilePath generateLocalCsvFilePath, WriteBudgetsToCsvFile writeBudgetsToCsvFile)
	{
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
		this.contextAccessor = contextAccessor;
		this.generateLocalCsvFilePath = generateLocalCsvFilePath;
		this.writeBudgetsToCsvFile = writeBudgetsToCsvFile;
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
	public async Task<string> UploadToBlobStorage(List<GetBudgetsToExportInfo> budgetInfos, BlobContainerClient containerClient)
	{
		string localFilePath = this.generateLocalCsvFilePath.Generate();
		string filePath = this.writeBudgetsToCsvFile.WriteBudgets(budgetInfos, localFilePath);

		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

		await blobClient.UploadAsync(filePath, true);
		File.Delete(filePath);

		return blobClient.Uri.AbsoluteUri;
	}

	/// <summary>
	/// x.
	/// </summary>
	/// <param name="filename">xxx.</param>
	/// <param name="containerClient">xx.</param>
	/// <returns>xxxx.</returns>
	public async Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename, BlobContainerClient containerClient)
	{
		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filename));
		BlobDownloadInfo download = await blobClient.DownloadAsync();
		return download;
	}
}