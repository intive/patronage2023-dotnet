using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// IBudgetExportService interface defines a contract for services that handle the exportation of budget data.
/// </summary>
public interface IBudgetExportService
{
	/// <summary>
	/// Exports the budgets to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="budgets">GetBudgetsListToExport.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	Task<string?> Export(GetBudgetTransferList? budgets);
}