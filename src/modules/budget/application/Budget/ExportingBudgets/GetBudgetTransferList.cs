namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

///
/// <summary>
/// Create Budget command.
/// </summary>
public record GetBudgetTransferList()
{
	/// <summary>
	/// BudgetsList object.
	/// </summary>
	public List<GetBudgetTransferInfo> BudgetsList { get; set; } = default!;
}