using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

///
/// <summary>
/// Create Budget command.
/// </summary>
public record GetBudgetTransferList()
{
	/// <summary>
	/// BudgetsList object.
	/// </summary>
	public List<GetBudgetTransferInfo> BudgetsList { get; init; } = default!;
}