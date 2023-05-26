using Azure.Storage.Blobs;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Services;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// BudgetExportService class provides functionalities to export budgets to a .csv file and upload it to Azure Blob Storage.
/// </summary>
public class BudgetExportService : IBudgetExportService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IBlobStorageService blobStorageService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetExportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="blobStorageService">BlobStorageService.</param>
	public BudgetExportService(IExecutionContextAccessor contextAccessor, IBlobStorageService blobStorageService)
	{
		this.contextAccessor = contextAccessor;
		this.blobStorageService = blobStorageService;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">GetBudgetsListToExport.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<string?> Export(GetBudgetTransferList? budgets)
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.blobStorageService.CreateBlobContainerIfNotExists(containerName);
		string uri = await this.blobStorageService.UploadToBlobStorage(budgets!, containerClient);
		return uri;
	}
}