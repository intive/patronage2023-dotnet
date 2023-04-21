using Intive.Patronage2023.Modules.Budget.Contracts;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// Budget information.
/// </summary>
public record BudgetInfo()
{
	/// <summary>
	/// Budget id.
	/// </summary>
	public BudgetId Id { get; init; }

	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; init; } = null!;

	/// <summary>
	/// Budget creation date.
	/// </summary>
	public DateTime CreatedOn { get; init; }
}