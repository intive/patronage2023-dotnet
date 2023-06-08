using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetExportService interface defines a contract for services that handle the exportation of budget data.
/// </summary>
public interface IBudgetExportService
{
	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">List of budgets to be exported.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	Task<ExportResult> ExportToStorage(GetTransferList<GetBudgetTransferInfo>? budgets);

	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">List of budgets to be exported.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	Task<ExportResult> Export(GetTransferList<GetBudgetTransferInfo>? budgets);
}