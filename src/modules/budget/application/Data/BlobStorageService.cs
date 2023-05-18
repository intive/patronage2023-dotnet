using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Azure Blob Storage Service class.
/// </summary>
public class BlobStorageService : IBlobStorageService
{
	private readonly BlobServiceClient blobServiceClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="BlobStorageService"/> class.
	/// Blob storage service.
	/// </summary>
	/// <param name="configuration">IConfiguration.</param>
	public BlobStorageService(IConfiguration configuration)
	{
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
	}

	/// <summary>
	/// Asynchronously uploading file to Azure Blob Storage.
	/// </summary>
	/// <param name="containerName">Name of container.</param>
	/// <param name="fileName">File name.</param>
	/// <param name="fileData">File content.</param>
	/// <returns>Url to uploaded file.</returns>
	public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileData)
	{
		BlobContainerClient blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

		await blobClient.UploadAsync(fileData);

		return blobClient.Uri.ToString();
	}
}