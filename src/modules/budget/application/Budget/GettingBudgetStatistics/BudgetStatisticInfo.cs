using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
/// <summary>
/// Model for statistics retrivial.
/// </summary>
public record BudgetStatisticInfo()
{
	/// <summary>
	/// Budget Id.
	/// </summary>
	public BudgetId BudgetId { get; init; }

	/// <summary>
	/// Transaction Value.
	/// </summary>
	public decimal Value { get; init; }

	/// <summary>
	/// Transaction Date.
	/// </summary>
	public DateTime BudgetTransactionDate { get; init; }
}