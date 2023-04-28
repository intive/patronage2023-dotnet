using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Create Budget Transaction.
/// </summary>
/// <param name="Type">Type of Transaction.</param>
/// <param name="Id">Transaction Id.</param>
/// <param name="Name">Name of Transaction.</param>
/// <param name="Value">Value must be positive for income or negative for expense.</param>
/// <param name="Category">Transaction Category.</param>
/// <param name="TransactionDate">Transaction Date.</param>
public record CreateTransaction(
	TransactionType Type,
	Guid Id,
	string Name,
	decimal Value,
	CategoryType Category,
	DateTime TransactionDate);