using Azure.Storage.Blobs;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// BudgetExportService.
/// </summary>
public class BudgetExportService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly DataHelperService dateHelperService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetExportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="dateHelperService">??.</param>
	public BudgetExportService(IExecutionContextAccessor contextAccessor, DataHelperService dateHelperService)
	{
		this.contextAccessor = contextAccessor;
		this.dateHelperService = dateHelperService;
	}

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	public async Task<string?> Export()
	{
		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.dateHelperService.CreateBlobContainerIfNotExists(containerName);
		var budgets = await this.dateHelperService.GetBudgetsToExport();
		string uri = await this.dateHelperService.UploadToBlobStorage(budgets!, containerClient);
		return uri;
	}
}