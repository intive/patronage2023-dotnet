using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetTransactionExportService interface defines a contract for services that handle the exportation of budget transactions data.
/// </summary>
public interface IBudgetTransactionExportService
{
	/// <summary>
	/// Exports the budget transactions to a CSV file and uploads it to storage, allowing to download it.
	/// </summary>
	/// <param name="transactions">Collection of the transactions to be exported.</param>
	/// <returns>The URI of the uploaded file.</returns>
	Task<ExportResult> ExportToStorage(GetTransferList<GetBudgetTransactionTransferInfo>? transactions);

	/// <summary>
	/// Exports the budget transactions to a CSV file and uploads it to storage, allowing to download it.
	/// </summary>
	/// <param name="transactions">Collection of the transactions to be exported.</param>
	/// <returns>The URI of the uploaded file.</returns>
	Task<Patronage2023.Shared.Infrastructure.ImportExport.Export.FileDescriptor> Export(GetTransferList<GetBudgetTransactionTransferInfo>? transactions);
}