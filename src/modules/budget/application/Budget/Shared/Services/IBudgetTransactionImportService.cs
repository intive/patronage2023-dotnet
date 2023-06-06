using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Import;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetTransactionImportService interface defines a contract for services that handle the import of budget transaction data.
/// </summary>
public interface IBudgetTransactionImportService
{
	/// <summary>
	/// Imports budget transaction data from the provided file.
	/// </summary>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <param name="file">The file containing the budget transaction data to import.</param>
	/// <returns>A <see cref="Task{TResult}"/>Representing the result of the asynchronous operation.</returns>
	Task<GetImportResult<BudgetTransactionAggregateList>> Import(BudgetId budgetId, IFormFile file);
}