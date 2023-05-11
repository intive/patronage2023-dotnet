namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
/// GetBudgetsToExport.
/// </summary>
public record GetBudgetsToExportInfo
{
	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; init; } = null!;

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