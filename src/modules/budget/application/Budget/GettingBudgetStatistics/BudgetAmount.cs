namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;

/// <summary>
/// Model for statistics retrivial.
/// </summary>
public record BudgetAmount()
{
	/// <summary>
	/// Value of transaction.
	/// </summary>
	public decimal Value { get; set; }

	/// <summary>
	/// Date Point.
	/// </summary>
	public DateTime DatePoint { get; init; }
}