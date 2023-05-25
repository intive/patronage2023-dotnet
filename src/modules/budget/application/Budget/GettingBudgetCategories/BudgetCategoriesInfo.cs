namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetCategories;

/// <summary>
/// Represents information about budget categories.
/// </summary>
public record BudgetCategoriesInfo
{
	/// <summary>
	/// Gets or sets the list of budget categories.
	/// </summary>
	public List<BudgetCategory>? BudgetCategoryList { get; set; }
}