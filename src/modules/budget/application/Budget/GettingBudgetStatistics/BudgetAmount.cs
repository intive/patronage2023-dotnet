namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;

/// <summary>
/// Model for statistics retrivial.
/// </summary>
public record BudgetAmount()
{
	/// <summary>
	/// Value of transaction.
	/// </summary>
	public decimal Value { get; init; }

	/// <summary>
	/// Date Point.
	/// </summary>
	public string DatePoint { get; init; } = null!;
}