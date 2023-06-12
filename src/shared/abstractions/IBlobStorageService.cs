using Azure.Storage.Blobs.Models;

namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// IBlobStorageService interface defines a contract for services that handle the exportation of data.
/// </summary>
public interface IBlobStorageService
{
	/// <summary>
	/// Uploads a CSV file containing a list of records to Azure Blob Storage.
	/// </summary>
	/// <param name="stream">A list of records to be written to the CSV file and uploaded.</param>
	/// <param name="filename">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <returns>The bloblClient name of the uploaded blob in Azure Blob Storage.</returns>
	Task<string> UploadToBlobStorage(Stream stream, string filename);

	/// <summary>
	/// Downloads a specified file from Azure Blob Storage.
	/// </summary>
	/// <param name="filename">The name of the file to be downloaded.</param>
	/// <returns>A task representing the asynchronous operation, yielding the downloaded file's information.</returns>
	Task<BlobDownloadInfo> DownloadFromBlobStorage(string filename);

	/// <summary>
	/// Generates a Shared Access Signature (SAS) URI for a specified file in the Blob Storage.
	/// This URI includes a SAS token, which allows for secure, direct access to the file.
	/// </summary>
	/// <param name="filename">The name of the file for which the SAS URI is to be generated.</param>
	/// <returns>A task representing the asynchronous operation, with a string result containing the SAS URI for the specified file.</returns>
	Task<string> GenerateLinkToDownload(string filename);
}