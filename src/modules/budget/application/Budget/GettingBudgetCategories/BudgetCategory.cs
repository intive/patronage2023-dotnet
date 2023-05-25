namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetCategories;

/// <summary>
/// Represents a budget category.
/// </summary>
public abstract record BudgetCategory
{
	/// <summary>
	/// Gets or sets the icon associated with the budget category.
	/// </summary>
	public string? Icon { get; set; }

	/// <summary>
	/// Gets or sets the name associated with the budget category.
	/// </summary>
	public string? Name { get; set; }
}