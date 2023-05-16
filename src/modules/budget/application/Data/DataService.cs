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
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

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
	/// <param name="file">file.</param>
	/// <returns>CSV file.</returns>
	public async Task<string> Import(IFormFile file)
	{
		using var stream = file.OpenReadStream();
		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		using (var csv = new CsvReader(new StreamReader(stream), csvConfig))
		{
			csv.Read();
			var budgets = csv.GetRecords<ConvertBudgetsToImport>();
			var budgetInfos = new List<GetBudgetsToExportInfo>();

			foreach (var budget in budgets)
			{
				string result = this.ValidateBudget(budget);

				if (!result.IsNullOrEmpty())
				{
					return result;
				}

				var budgetInfo = new GetBudgetsToExportInfo
				{
					Name = budget.Name,
					Limit = decimal.Parse(budget.Limit),
					Currency = budget.Currency,
					StartDate = DateTime.Parse(budget.StarTime),
					EndDate = DateTime.Parse(budget.EndTime),
					Icon = budget.IconName,
					Description = budget.Description!,
				};

				budgetInfos.Add(budgetInfo);
			}

			string containerName = this.contextAccessor.GetUserId().ToString()!;
			BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists(containerName);

			string localFilePath = this.GenerateLocalCsvFilePath();
			string filePath = this.WriteBudgetsToCsvFile(budgetInfos, localFilePath);

			BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));
			await blobClient.UploadAsync(filePath, true);
			File.Delete(filePath);

			BlobDownloadInfo download = await blobClient.DownloadAsync();
			////var reader = new StreamReader(download.Content);

			csv.Read();
			var budgetsToImport = csv.GetRecords<ConvertBudgetsToImport>();

			foreach (var budget in budgetsToImport)
			{
				decimal limit = decimal.Parse(budget.Limit, CultureInfo.InvariantCulture);
				var money = new Money(limit, (Currency)Enum.Parse(typeof(Currency), budget.Currency));
				var period = new Period(DateTime.Parse(budget.StarTime), DateTime.Parse(budget.EndTime));

				var newbudget = new CreateBudget(Guid.NewGuid(), budget.Name, Guid.NewGuid(), money, period, budget.Description, budget.IconName);

				await this.commandBus.Send(newbudget);
			}
		}

		return "ok";
	}

	private string ValidateBudget(ConvertBudgetsToImport budget)
	{
		switch (budget)
		{
			case { Name: null or "" }:
				return "Budget name is missing";
			case { Limit: null or "" }:
				return "Budget limit is missing";
			case { Currency: null or "" }:
				return "Budget currency is missing";
			case { StarTime: null or "" }:
				return "Budget start date is missing";
			case { EndTime: null or "" }:
				return "Budget end date is missing";
			case { Description: null }:
				return "Budget description cannot be null";
			case { IconName: null or "" }:
				return "Budget icon is missing";
			case { } when !decimal.TryParse(budget.Limit, NumberStyles.Any, CultureInfo.InvariantCulture, out _):
				return "Budget limit is not a valid decimal number";
			case { } when !DateTime.TryParse(budget.StarTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out _):
				return "Budget start date is not a valid date";
			case { } when !DateTime.TryParse(budget.EndTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out _):
				return "Budget end date is not a valid date";
			case { } when DateTime.Parse(budget.StarTime) >= DateTime.Parse(budget.EndTime):
				return "Budget start date cannot be later than or equal to end date";
			default:
				return string.Empty;
		}
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
		string fileName = Guid.NewGuid().ToString() + ".csv";
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