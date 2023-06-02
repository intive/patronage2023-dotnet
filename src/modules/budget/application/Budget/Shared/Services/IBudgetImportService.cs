using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// IBudgetImportService interface defines a contract for services that handle the importation of budget data.
/// </summary>
public interface IBudgetImportService
{
	/// <summary>
	/// Imports budget data from the provided file.
	/// </summary>
	/// <param name="file">The file containing the budget data to import.</param>
	/// <returns>A <see cref="Task{TResult}"/>Representing the result of the asynchronous operation.</returns>
	Task<GetImportBudgetsResult> Import(IFormFile file);
}