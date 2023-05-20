using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Interface IBudgetExportService.
/// </summary>
public interface IBudgetImportService
{
	/// <summary>
	/// Imports budget data from the provided file.
	/// </summary>
	/// <param name="file">The file containing the budget data to import.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
	Task<GetImportResult> Import(IFormFile file);
}