namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

///
/// <summary>
/// Create Budget command.
/// </summary>
public record GetBudgetsToExportInfo()
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
	/// Budget Currency.
	/// </summary>
	public string Currency { get; init; } = null!;

	/// <summary>
	/// Budget limit.
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