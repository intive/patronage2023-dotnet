using System.Globalization;
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Class DataService.
/// </summary>
public class DataService
{
	private readonly IQueryBus queryBus;
	private readonly ICommandBus commandBus;
	private readonly BlobServiceClient blobServiceClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="queryBus">QueryBus.</param>
	/// <param name="commandBus">ICommand.</param>
	/// <param name="configuration">IConfiguration.</param>
	public DataService(IQueryBus queryBus, ICommandBus commandBus, IConfiguration configuration)
	{
		this.queryBus = queryBus;
		this.commandBus = commandBus;
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
	}

	/// <summary>
	/// Method to export budgets to CSV file.
	/// </summary>
	/// <returns>CSV file.</returns>
	public async Task<int> Export()
	{
		// Create a unique name for the container
		string containerName = "csv";
		BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();

		var query = new GetBudgetsToExport() { };
		var budgets = await this.queryBus.Query<GetBudgetsToExport, List<GetBudgetsToExportInfo>?>(query);

		if (budgets!.Count == 0)
		{
			return (int)HttpStatusCode.NotFound;
		}

		// Create a local file in the ./data/ directory for uploading and downloading
		string localPath = "data";
		Directory.CreateDirectory(localPath);
		string fileName = "quickstart" + Guid.NewGuid().ToString() + ".csv";
		string localFilePath = Path.Combine(localPath, fileName);

		// Write text to the file
		using (var writer = new StreamWriter(localFilePath))
		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteHeader<GetBudgetsToExportInfo>();
			csv.NextRecord();
			foreach (var budget in budgets!)
			{
				csv.WriteRecord(budget);
				csv.NextRecord();
			}
		}

		// Get a reference to a blob
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		// Upload data from the local file
		await blobClient.UploadAsync(localFilePath, true);

		// Delete the local file after uploading to Azure Blob Storage
		File.Delete(localFilePath);

		return (int)HttpStatusCode.OK;
	}

	/// <summary>
	/// Method to import budgets to CSV file.
	/// </summary>
	/// <param name="fileName">fileName.</param>
	/// <returns>CSV file.</returns>
	public async Task Import(string fileName)
	{
		string containerName = "csv";
		BlobContainerClient containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);

		////Get a reference to the blob that needs to be read
		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		// Pobieranie strumienia danych z pliku CSV
		BlobDownloadInfo download = await blobClient.DownloadAsync();
		var reader = new StreamReader(download.Content);

		// Ustawienia dla CSVHelper
		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false, // nagłówek pliku CSV jest obecny
			Delimiter = ",", // separator pól w pliku CSV
		};

		// Parsowanie pliku CSV przy użyciu CSVHelper
		using (var csv = new CsvReader(reader, csvConfig))
		{
			csv.Read();

			// Wyodrębnianie danych z pliku CSV
			var budgets = csv.GetRecords<CreateBudgetsToImport>();

			foreach (var budget in budgets)
			{
				decimal limit = decimal.Parse(budget.Limit, CultureInfo.InvariantCulture);

				var newbudget = new CreateBudget(Guid.NewGuid(), budget.Name, Guid.NewGuid(), new Money(limit, (Currency)Enum.Parse(typeof(Currency), budget.Currency)), new Period(DateTime.Parse(budget.StarTime), DateTime.Parse(budget.EndTime)), budget.Description, budget.IconName);

				await this.commandBus.Send(newbudget);
			}
		}

		reader.Close();
	}
}