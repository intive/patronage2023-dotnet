namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Interface for Azure Blob storage service.
/// </summary>
public interface IBlobStorageService
{
	/// <summary>
	/// Method asynchronously uploads file to storage.
	/// </summary>
	/// <param name="containerName">Name of the container.</param>
	/// <param name="fileName">File name.</param>
	/// <param name="fileData">File content data stream.</param>
	/// <returns>String with url to Azure blob storage.</returns>
	Task<string> UploadFileAsync(string containerName, string fileName, Stream fileData);
}