namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetTransactionExportService interface defines a contract for services that handle the exportation of budget transactions data.
/// </summary>
public interface IBudgetTransactionExportService
{
	/// <summary>
	/// Exports the budget transactions to a CSV file and uploads it to Azure Blob Storage.
	/// </summary>
	/// <param name="transactions">GetBudgetTransactionListToExport.</param>
	/// <returns>The URI of the uploaded file in the Azure Blob Storage.</returns>
	Task<string?> Export(GetBudgetTransactionTransferList? transactions);
}