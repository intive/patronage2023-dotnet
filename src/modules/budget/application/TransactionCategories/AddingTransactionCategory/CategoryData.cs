namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Represents a budget category.
/// </summary>
public record CategoryData
{
	/// <summary>
	/// Gets or sets the icon associated with the budget category.
	/// </summary>
	public string? Icon { get; init; }

	/// <summary>
	/// Gets or sets the name associated with the budget category.
	/// </summary>
	public string? Name { get; init; }
}