using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class GetBudgetsToExport.
/// </summary>
public class GetBudgetsToExportAsync
{
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsToExportAsync"/> class.
	/// GetBudgetsToExport.
	/// </summary>
	/// <param name="queryBus">The Query bus used for executing queries.</param>
	public GetBudgetsToExportAsync(IQueryBus queryBus)
	{
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Retrieves all budgets to be exported.
	/// </summary>
	/// <returns>A list of budget objects to be exported. Returns null if no budgets are found.</returns>
	public async Task<List<GetBudgetsToExportInfo>?> Export()
	{
		var query = new GetBudgetsToExport() { };
		return await this.queryBus.Query<GetBudgetsToExport, List<GetBudgetsToExportInfo>?>(query);
	}
}