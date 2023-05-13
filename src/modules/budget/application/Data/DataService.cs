using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Class DataService.
/// </summary>
public class DataService
{
	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	public DataService()
	{
	}

	/// <summary>
	/// Method to export budgets to CSV file.
	/// </summary>
	/// <returns>CSV file.</returns>
	public async Task Export()
	{
		// TODO: Replace <storage-account-name> with your actual storage account name
		var blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://azurite:10000/devstoreaccount1;QueueEndpoint=http://azurite:10001/devstoreaccount1;");

		// Create a unique name for the container
		string containerName = "quickstartblobs" + Guid.NewGuid().ToString();

		// Create the container and return a container client object
		BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

		// Create a local file in the ./data/ directory for uploading and downloading
		string localPath = "data";
		Directory.CreateDirectory(localPath);
		string fileName = "quickstart" + Guid.NewGuid().ToString() + ".txt";
		string localFilePath = Path.Combine(localPath, fileName);

		// Write text to the file
		await File.WriteAllTextAsync(localFilePath, "Hello, World!");

		// Get a reference to a blob
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		Console.WriteLine("Uploading to Blob storage as blob:\n\t {0}\n", blobClient.Uri);

		// Upload data from the local file
		await blobClient.UploadAsync(localFilePath, true);

		Console.WriteLine("Listing blobs...");

		// List all blobs in the container
		await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
		{
			Console.WriteLine("\t" + blobItem.Name);
		}

		// Download the blob to a local file
		// Append the string "DOWNLOADED" before the .txt extension.
		// so you can compare the files in the data directory
		string downloadFilePath = localFilePath.Replace(".txt", "DOWNLOADED.txt");

		Console.WriteLine("\nDownloading blob to\n\t{0}\n", downloadFilePath);

		// Download the blob's contents and save it to a file
		await blobClient.DownloadToAsync(downloadFilePath);

		//// Clean up
		// Console.Write("Press any key to begin clean up");
		// Console.ReadLine();

		// Console.WriteLine("Deleting blob container...");
		// await containerClient.DeleteAsync();

		// Console.WriteLine("Deleting the local source and downloaded files...");
		// File.Delete(localFilePath);
		// File.Delete(downloadFilePath);

		// Console.WriteLine("Done");
	}
}