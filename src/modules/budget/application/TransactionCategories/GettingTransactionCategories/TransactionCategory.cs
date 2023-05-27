namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// Represents a budget category.
/// </summary>
public record TransactionCategory
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