namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
///  The record is used to store information about a user's role in relation to a specific budget.
/// </summary>
public record GetBudgetsNameInfo()
{
	/// <summary>
	/// Represents the user's role.
	/// </summary>
	public List<string>? BudgetName { get; init; }
}