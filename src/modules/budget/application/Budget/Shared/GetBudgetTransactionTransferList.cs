namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Command to retrieve a list of budget transactions transfer information.
/// </summary>
public record GetBudgetTransactionTransferList()
{
	/// <summary>
	/// Contains a list of budget transactions transfer details.
	/// </summary>
	public List<GetBudgetTransactionTransferInfo> BudgetTransactionsList { get; init; } = default!;
}