using Azure.Storage.Blobs;
using Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// BudgetExportService.
/// </summary>
public class BudgetExportService : IBudgetExportService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BlobStorageService blobStorageService;
	private readonly GetBudgetsToExportAsync getBudgetsToExport;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetExportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="getBudgetsToExport">GetBudgetsToExportAsync.</param>
	public BudgetExportService(IExecutionContextAccessor contextAccessor, BlobStorageService blobStorageService, GetBudgetsToExportAsync getBudgetsToExport)
	{
		this.contextAccessor = contextAccessor;
		this.blobStorageService = blobStorageService;
		this.getBudgetsToExport = getBudgetsToExport;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<string?> Export()
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.blobStorageService.CreateBlobContainerIfNotExists(containerName);
		var budgets = await this.getBudgetsToExport.Export();
		string uri = await this.blobStorageService.UploadToBlobStorage(budgets!, containerClient);
		return uri;
	}
}