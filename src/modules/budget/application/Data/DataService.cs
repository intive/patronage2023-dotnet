using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
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

		return blobClient.Uri.AbsoluteUri;
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