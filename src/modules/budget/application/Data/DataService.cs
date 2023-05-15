using System.Globalization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Shared.Abstractions;
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
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BlobServiceClient blobServiceClient;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="configuration">IConfiguration.</param>
	public DataService(IQueryBus queryBus, ICommandBus commandBus, IExecutionContextAccessor contextAccessor, IConfiguration configuration)
	{
		this.queryBus = queryBus;
		this.commandBus = commandBus;
		this.contextAccessor = contextAccessor;
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
	}

	/// <summary>
	/// Method to save butgets to CSV file and export to Azure Blob Storage.
	/// </summary>
	/// <returns>Name of the uploaded file.</returns>
	public async Task<string?> Export()
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists(containerName);

		var budgets = await this.GetBudgetsToExport();

		string localFilePath = this.GenerateLocalCsvFilePath();

		string filePath = this.WriteBudgetsToCsvFile(budgets!, localFilePath);

		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

		await blobClient.UploadAsync(filePath, true);

		File.Delete(filePath);

		return blobClient.Name;
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

		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		BlobDownloadInfo download = await blobClient.DownloadAsync();
		var reader = new StreamReader(download.Content);

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		using (var csv = new CsvReader(reader, csvConfig))
		{
			csv.Read();

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

	private async Task<BlobContainerClient> CreateBlobContainerIfNotExists(string containerName)
	{
		var containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();
		return containerClient;
	}

	private async Task<List<GetBudgetsToExportInfo>?> GetBudgetsToExport()
	{
		var query = new GetBudgetsToExport() { };
		return await this.queryBus.Query<GetBudgetsToExport, List<GetBudgetsToExportInfo>?>(query);
	}

	private string GenerateLocalCsvFilePath()
	{
		string localPath = "data"; ////src\api\app\data
		Directory.CreateDirectory(localPath);
		DateTime now = DateTime.Now;
		string formattedDate = now.ToString("yyyy-MM-dd");
		string fileName = Guid.NewGuid().ToString() + formattedDate + ".csv";
		return Path.Combine(localPath, fileName);
	}

	private string WriteBudgetsToCsvFile(List<GetBudgetsToExportInfo> budgets, string filePath)
	{
		// Write text to the file
		using (var writer = new StreamWriter(filePath))
		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteHeader<GetBudgetsToExportInfo>();
			csv.NextRecord();
			foreach (var budget in budgets)
			{
				csv.WriteRecord(budget);
				csv.NextRecord();
			}
		}

		return filePath;
	}
}