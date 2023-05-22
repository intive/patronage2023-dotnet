using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// IBudgetExportService interface defines a contract for services that handle the exportation of budget data.
/// </summary>
public interface IBlobStorageService
{
	/// <summary>
	/// Checks if a blob container exists, and if not, creates one.
	/// </summary>
	/// <param name="containerName">The name of the container to be checked/created.</param>
	/// <returns>A client reference to the newly created or existing blob container.</returns>
	Task<BlobContainerClient> CreateBlobContainerIfNotExists(string containerName);

	/// <summary>
	/// Uploads a CSV file containing a list of budgets to Azure Blob Storage.
	/// </summary>
	/// <param name="budgetInfos">A list of budgets to be written to the CSV file and uploaded.</param>
	/// <param name="containerClient">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <returns>The absolute URI of the uploaded blob in Azure Blob Storage.</returns>
	Task<string> UploadToBlobStorage(GetBudgetTransferList budgetInfos, BlobContainerClient containerClient);

	/// <summary>
	/// Downloads a specified file from Azure Blob Storage.
	/// </summary>
	/// <param name="filename">The name of the file to be downloaded.</param>
	/// <param name="containerClient">A client object for interacting with the Azure Blob Storage container.</param>
	/// <returns>A task representing the asynchronous operation, yielding the downloaded file's information.</returns>
	Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename, BlobContainerClient containerClient);
}