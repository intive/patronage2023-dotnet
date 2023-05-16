using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="configuration">IConfiguration.</param>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public DataService(IQueryBus queryBus, ICommandBus commandBus, IExecutionContextAccessor contextAccessor, IConfiguration configuration, BudgetDbContext budgetDbContext)
	{
		this.queryBus = queryBus;
		this.commandBus = commandBus;
		this.contextAccessor = contextAccessor;
		this.blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
		this.budgetDbContext = budgetDbContext;
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
	public async Task<List<string>> Import(IFormFile file)
	{
		var errors = new List<string>();

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
			int rowNumber = 0;

			foreach (var budget in budgets)
			{
				var results = this.ValidateBudget(budget);
				rowNumber++;

				if (!results.IsNullOrEmpty())
				{
					foreach (string result in results)
					{
						errors.Add($"row: {rowNumber}| error: {result}");
					}

					continue;
				}

				var budgetInfo = await this.CreateBudgetInfoAsync(budget);
				budgetInfos.Add(budgetInfo);
			}
		}

		return errors;
	}

	private List<string> ValidateBudget(ConvertBudgetsToImport budget)
	{
		var errors = new List<string>();

		if (string.IsNullOrEmpty(budget.Name))
		{
			errors.Add("Budget name is missing");
		}

		if (string.IsNullOrEmpty(budget.Limit))
		{
			errors.Add("Budget limit is missing");
		}
		else if (!decimal.TryParse(budget.Limit, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
		{
			errors.Add("Budget limit is not a valid decimal number");
		}

		if (string.IsNullOrEmpty(budget.Currency))
		{
			errors.Add("Budget currency is missing");
		}

		if (string.IsNullOrEmpty(budget.StarTime))
		{
			errors.Add("Budget start date is missing");
		}
		else if (!DateTime.TryParse(budget.StarTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget start date is not a valid date");
		}

		if (string.IsNullOrEmpty(budget.EndTime))
		{
			errors.Add("Budget end date is missing");
		}
		else if (!DateTime.TryParse(budget.EndTime, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget end date is not a valid date");
		}

		if (!string.IsNullOrEmpty(budget.StarTime) && !string.IsNullOrEmpty(budget.EndTime) &&
			DateTime.Parse(budget.StarTime) >= DateTime.Parse(budget.EndTime))
		{
			errors.Add("Budget start date cannot be later than or equal to end date");
		}

		if (budget.Description == null)
		{
			errors.Add("Budget description cannot be null");
		}

		if (string.IsNullOrEmpty(budget.IconName))
		{
			errors.Add("Budget icon is missing");
		}

		return errors;
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

	private async Task<GetBudgetsToExportInfo> CreateBudgetInfoAsync(ConvertBudgetsToImport budget)
	{
		bool isExistingBudget = await this.budgetDbContext.Budget.AnyAsync(b => b.Name.Equals(budget.Name));
		////&& b.UserId.Equals(this.contextAccessor.GetUserId()));
		string budgetName = isExistingBudget ? budget.Name + new Random().Next(100000, 900001) : budget.Name;

		var budgetInfo = new GetBudgetsToExportInfo
		{
			Name = budgetName,
			Limit = decimal.Parse(budget.Limit),
			Currency = budget.Currency,
			StartDate = DateTime.Parse(budget.StarTime),
			EndDate = DateTime.Parse(budget.EndTime),
			Icon = budget.IconName,
			Description = budget.Description,
		};

		return budgetInfo;
	}
}