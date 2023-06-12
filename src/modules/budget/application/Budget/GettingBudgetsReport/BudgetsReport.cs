namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;

/// <summary>
/// Model for Retriving data for Budgets Report.
/// </summary>
/// <typeparam name="T">Type of elements in collection.</typeparam>
public class BudgetsReport<T>
{
	/// <summary>
	/// List of Incomes.
	/// </summary>
	public List<T> Incomes { get; set; } = null!;

	/// <summary>
	/// List of Expenses.
	/// </summary>
	public List<T> Expenses { get; set; } = null!;

	/// <summary>
	/// Trend value.
	/// </summary>
	public decimal? TrendValue { get; set; }

	/// <summary>
	/// Period value.
	/// </summary>
	public decimal PeriodValue { get; set; }

	/// <summary>
	/// Total Budget value.
	/// </summary>
	public decimal TotalBalance { get; set; }
}