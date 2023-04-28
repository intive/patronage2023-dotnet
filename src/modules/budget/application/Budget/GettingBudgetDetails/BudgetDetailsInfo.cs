using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// Budget information.
/// </summary>
public record BudgetDetailsInfo()
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
	/// Budget owner user Id.
	/// </summary>
	public Guid UserId { get; init; }

	/// <summary>
	/// Budget limit.
	/// </summary>
	public decimal Limit { get; init; }

	/// <summary>
	/// Budget Currency.
	/// </summary>
	public string Currency { get; init; } = null!;

	/// <summary>
	/// Budget start date.
	/// </summary>
	public DateTime StartDate { get; init; }

	/// <summary>
	/// Budget end date.
	/// </summary>
	public DateTime EndDate { get; init; }

	/// <summary>
	/// Budget icon.
	/// </summary>
	public string? Icon { get; init; }

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; init; }
}