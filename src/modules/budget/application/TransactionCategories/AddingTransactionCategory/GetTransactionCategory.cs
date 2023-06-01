namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;

/// <summary>
/// Represents a budget transaction category.
/// </summary>
public record GetTransactionCategory
{
	/// <summary>
	/// Gets or sets the icon associated with the budget transaction category.
	/// </summary>
	public string Icon { get; init; } = default!;

	/// <summary>
	/// Gets or sets the name associated with the budget transaction category.
	/// </summary>
	public string Name { get; init; } = default!;
}