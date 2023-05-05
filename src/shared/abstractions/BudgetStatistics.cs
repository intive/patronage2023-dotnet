namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Model for Retriving data for Budget statistics.
/// </summary>
/// <typeparam name="T">Type of elements in collection.</typeparam>
public class BudgetStatistics<T>
{
	/// <summary>
	/// List of items of generic type.
	/// </summary>
	public List<T> Items { get; set; } = null!;

	/// <summary>
	/// Trend value.
	/// </summary>
	public decimal TrendValue { get; set; }

	/// <summary>
	/// Period value.
	/// </summary>
	public decimal PeriodValue { get; set; }

	/// <summary>
	/// Total Budget value.
	/// </summary>
	public decimal TotalBudgetValue { get; set; }
}