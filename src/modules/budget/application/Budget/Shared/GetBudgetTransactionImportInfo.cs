using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Represents detailed information about a budget transfer.
/// </summary>
public record GetBudgetTransactionImportInfo() : GetBudgetTransactionTransferInfo
{
	/// <summary>
	/// Transaction budget id.
	/// </summary>
	public BudgetId BudgetId { get; init; }
}