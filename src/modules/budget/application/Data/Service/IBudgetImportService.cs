using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Interface IBudgetExportService.
/// </summary>
public interface IBudgetImportService
{
	/// <summary>
	/// Import method.
	/// </summary>
	/// <param name="file">IFormFile.</param>
	/// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
	Task<ImportResult> Import(IFormFile file);
}