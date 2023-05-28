using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetExportService interface defines a contract for services that handle the exportation of budget data.
/// </summary>
public interface IBlobStorageService
{
	/// <summary>
	/// Checks if a blob container exists, and if not, creates one.
	/// </summary>
	/// <returns>A client reference to the newly created or existing blob container.</returns>
	Task<BlobContainerClient> CreateBlobContainerIfNotExists();

	/// <summary>
	/// Uploads a CSV file containing a list of budgets to Azure Blob Storage.
	/// </summary>
	/// <param name="stream">A list of budgets to be written to the CSV file and uploaded.</param>
	/// <param name="filename">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <returns>The absolute URI of the uploaded blob in Azure Blob Storage.</returns>
	Task UploadToBlobStorage(Stream stream, string filename);

	/// <summary>
	/// Downloads a specified file from Azure Blob Storage.
	/// </summary>
	/// <param name="filename">The name of the file to be downloaded.</param>
	/// <returns>A task representing the asynchronous operation, yielding the downloaded file's information.</returns>
	Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename);

	/// <summary>
	/// 1.
	/// </summary>
	/// <param name="filename">2.</param>
	/// <returns>3.</returns>
	Task<string> GenerateLinkToDownload(string filename);
}