namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Represents detailed information about a budget transfer.
/// </summary>
public record GetBudgetTransferInfo()
{
	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; init; } = null!;

	/// <summary>
	/// Budget icon.
	/// </summary>
	public string IconName { get; init; } = null!;

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; init; }

	/// <summary>
	/// Budget currency.
	/// </summary>
	public string Currency { get; init; } = null!;

	/// <summary>
	/// Budget value.
	/// </summary>
	public string Value { get; init; } = null!;

	/// <summary>
	/// Budget start date.
	/// </summary>
	public string StartDate { get; init; } = null!;

	/// <summary>
	/// Budget end date.
	/// </summary>
	public string EndDate { get; init; } = null!;
}