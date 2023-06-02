namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Command to retrieve a list of budget transactions import information.
/// </summary>
public record GetBudgetTransactionImportList()
{
	/// <summary>
	/// Contains a list of budget transactions import details.
	/// </summary>
	public List<GetBudgetTransactionImportInfo> BudgetTransactionsList { get; init; } = default!;
}