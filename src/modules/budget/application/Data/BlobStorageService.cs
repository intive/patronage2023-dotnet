using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Intive.Patronage2023.Shared.Domain;
using Microsoft.AspNetCore.Http;
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
	public async Task<Uri> UploadFileAsync(string containerName, string fileName, Stream fileData)
	{
		BlobContainerClient blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		BlobClient blobClient = blobContainerClient.GetBlobClient(fileName);

		await blobContainerClient.CreateIfNotExistsAsync();

		await blobClient.UploadAsync(fileData);

		return blobClient.Uri;
	}

	/// <summary>
	/// Asynchronously retrieves file from blob storage.
	/// </summary>
	/// <param name="url">File url.</param>
	/// <returns>Variable of type IFormFile containing retrieved file.</returns>
	public async Task<IFormFile> GetFileFromUrlAsync(Uri url)
	{
		string containerName = url.Segments[2].TrimEnd('/');
		string blobName = url.Segments[3].TrimEnd('/');

		var blobContainerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		var blobClient = blobContainerClient.GetBlobClient(blobName);

		Response<BlobDownloadInfo> response = await blobClient.DownloadAsync();

		var fileModel = new FileModel
		{
			FileName = blobName,
			Content = new MemoryStream(),
		};

		await response.Value.Content.CopyToAsync(fileModel.Content);

		IFormFile file = new UrlFileForm(fileModel);

		return file;
	}
}