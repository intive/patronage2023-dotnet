using System.Globalization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="commandBus">The Command bus used for executing commands.</param>
	/// <param name="queryBus">The Query bus used for executing queries.</param>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="configuration">The application's configuration, used for retrieving the connection string for the Blob Storage.</param>
	/// <param name="budgetDbContext">The DbContext for accessing budget data in the database.</param>
	public DataService(IQueryBus queryBus, ICommandBus commandBus, IExecutionContextAccessor contextAccessor, IConfiguration configuration, BudgetDbContext budgetDbContext)
	{
		this.queryBus = queryBus;
		this.commandBus = commandBus;
		this.contextAccessor = contextAccessor;
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<string?> Export()
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists(containerName);
		var budgets = await this.GetBudgetsToExport();
		string uri = await this.UploadToBlobStorage(budgets!, containerClient);
		return uri;
	}

	/// <summary>
	/// Imports budgets from a provided .csv file, validates them, and stores them in the system.
	/// </summary>
	/// <param name="file">The .csv file containing the budgets to be imported.</param>
	/// <returns>A tuple containing a list of any errors encountered during the import process and
	/// the URI of the saved .csv file in the Azure Blob Storage if any budgets were successfully imported.
	/// If no budgets were imported, the URI is replaced with a message stating "No budgets were saved.".</returns>
	public async Task<(List<string>? ErrorsList, string? Uri)> Import(IFormFile file)
	{
		var errors = new List<string>();

		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.CreateBlobContainerIfNotExists(containerName);

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		var budgetInfos = this.ReadAndValidateBudgets(file, csvConfig, errors);

		if (budgetInfos.Count() == 0)
		{
			return (errors, "No budgets were saved.");
		}

		string uri = await this.UploadToBlobStorage(budgetInfos, containerClient);

		string fileName = new Uri(uri).LocalPath;
		await this.ImportBudgetsFromBlobStorage(fileName, containerClient, csvConfig);

		return (errors, uri);
	}

	private async Task<BlobContainerClient> CreateBlobContainerIfNotExists(string containerName)
	{
		var containerClient = this.blobServiceClient.GetBlobContainerClient(containerName);
		await containerClient.CreateIfNotExistsAsync();
		return containerClient;
	}

	private List<string> ValidateBudget(GetBudgetsToExportInfo budget)
	{
		var errors = new List<string>();

		if (string.IsNullOrEmpty(budget.Name))
		{
			errors.Add("Budget name is missing");
		}

		if (string.IsNullOrEmpty(budget.IconName))
		{
			errors.Add("Budget icon name is missing");
		}

		if (string.IsNullOrEmpty(budget.Currency))
		{
			errors.Add("Budget currency is missing");
		}

		if (string.IsNullOrEmpty(budget.Value))
		{
			errors.Add("Budget value is missing");
		}
		else if (!decimal.TryParse(budget.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
		{
			errors.Add("Budget value is not a valid decimal number");
		}

		if (string.IsNullOrEmpty(budget.StartDate))
		{
			errors.Add("Budget start date is missing");
		}
		else if (!DateTime.TryParse(budget.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget start date is not a valid date");
		}

		if (string.IsNullOrEmpty(budget.EndDate))
		{
			errors.Add("Budget end date is missing");
		}
		else if (!DateTime.TryParse(budget.EndDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget end date is not a valid date");
		}

		if (!string.IsNullOrEmpty(budget.StartDate) && !string.IsNullOrEmpty(budget.EndDate) &&
			DateTime.Parse(budget.StartDate) >= DateTime.Parse(budget.EndDate))
		{
			errors.Add("Budget start date cannot be later than or equal to end date");
		}

		return errors;
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

	private async Task<string> UploadToBlobStorage(List<GetBudgetsToExportInfo> budgetInfos, BlobContainerClient containerClient)
	{
		string localFilePath = this.GenerateLocalCsvFilePath();
		string filePath = this.WriteBudgetsToCsvFile(budgetInfos, localFilePath);

		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filePath));

		await blobClient.UploadAsync(filePath, true);
		File.Delete(filePath);

		return blobClient.Uri.AbsoluteUri;
	}

	private async Task ImportBudgetsFromBlobStorage(string filename, BlobContainerClient containerClient, CsvConfiguration csvConfig)
	{
		BlobClient blobClient = containerClient.GetBlobClient(Path.GetFileName(filename));
		BlobDownloadInfo download = await blobClient.DownloadAsync();

		using (var reader = new StreamReader(download.Content))
		{
			using (var csv = new CsvReader(reader, csvConfig))
			{
				csv.Read();
				var budgetsToImport = csv.GetRecords<GetBudgetsToExportInfo>();

				foreach (var budget in budgetsToImport)
				{
					var userId = this.contextAccessor.GetUserId()!.Value;
					decimal limit = decimal.Parse(budget.Value, CultureInfo.InvariantCulture);
					var money = new Money(limit, (Currency)Enum.Parse(typeof(Currency), budget.Currency));
					var period = new Period(DateTime.Parse(budget.StartDate), DateTime.Parse(budget.EndDate));
					string description = string.IsNullOrEmpty(budget.Description) ? string.Empty : budget.Description;

					var newbudget = new CreateBudget(Guid.NewGuid(), budget.Name, userId, money, period, description, budget.IconName);

					await this.commandBus.Send(newbudget);
				}
			}
		}
	}

	private List<GetBudgetsToExportInfo> ReadAndValidateBudgets(IFormFile file, CsvConfiguration csvConfig, List<string> errors)
	{
		var budgetInfos = new List<GetBudgetsToExportInfo>();
		using var stream = file.OpenReadStream();
		using (var csv = new CsvReader(new StreamReader(stream), csvConfig))
		{
			csv.Read();
			var budgets = csv.GetRecords<GetBudgetsToExportInfo>().ToList();
			int rowNumber = 0;

			foreach (var budget in budgets)
			{
				var results = this.ValidateBudget(budget);
				rowNumber++;

				if (results.Any())
				{
					foreach (string result in results)
					{
						errors.Add($"row: {rowNumber}| error: {result}");
					}

					continue;
				}

				var updateBudget = this.CreateBudgetInfoAsync(budget);
				budgetInfos.Add(updateBudget!);
			}
		}

		return budgetInfos;
	}

	private GetBudgetsToExportInfo? CreateBudgetInfoAsync(GetBudgetsToExportInfo budget)
	{
		bool isExistingBudget = this.budgetDbContext.Budget.Any(b => b.Name.Equals(budget.Name));
		string budgetName = isExistingBudget ? budget.Name + new Random().Next(100000, 900001) : budget.Name;

		var budgetInfo = new GetBudgetsToExportInfo
		{
			Name = budgetName,
			IconName = budget.IconName,
			Description = budget.Description,
			Currency = budget.Currency,
			Value = budget.Value,
			StartDate = budget.StartDate,
			EndDate = budget.EndDate,
		};

		return budgetInfo;
	}
}