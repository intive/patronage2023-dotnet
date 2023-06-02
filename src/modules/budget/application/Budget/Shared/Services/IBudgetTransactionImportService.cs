using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetTransactionImportService interface defines a contract for services that handle the import of budget Transaction data.
/// </summary>
public interface IBudgetTransactionImportService
{
	/// <summary>
	/// Imports budget Transaction data from the provided file.
	/// </summary>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <param name="file">The file containing the budget Transaction data to import.</param>
	/// <returns>A <see cref="Task{TResult}"/>Representing the result of the asynchronous operation.</returns>
	Task<GetImportBudgetTransactionsResult> Import(BudgetId budgetId, IFormFile file);
}