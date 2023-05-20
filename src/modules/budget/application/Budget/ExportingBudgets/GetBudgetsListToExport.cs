namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

///
/// <summary>
/// Create Budget command.
/// </summary>
public record GetBudgetsListToExport()
{
	/// <summary>
	/// BudgetsList object.
	/// </summary>
	public List<GetBudgetsToExportInfo> BudgetsList { get; set; } = default!;
}