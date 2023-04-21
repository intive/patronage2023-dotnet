using Intive.Patronage2023.Shared.Infrastructure.Helpers;
using Intive.Patronage2023.Modules.Budget.Contracts;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Model of Income and Expense.
/// </summary>
public record BudgetTransactionInfo()
{
	/// <summary>
	/// Transaction Type.
	/// </summary>
	public TransactionTypes TransactionType { get; init; }

	/// <summary>
	/// Transaction Id.
	/// </summary>
	public TransactionId TransactionId { get; init; }

	/// <summary>
	/// Budget Transaction Id.
	/// </summary>
	public BudgetId BudgetId { get; init; }

	/// <summary>
	/// Transaction Name.
	/// </summary>
	public string Name { get; init; } = null!;

	/// <summary>
	/// Transaction Value.
	/// </summary>
	public decimal Value { get; init; }

	/// <summary>
	/// Transaction Date.
	/// </summary>
	public DateTime BudgetTransactionDate { get; init; }

	/// <summary>
	/// Transaction Category.
	/// </summary>
	public CategoriesType CategoryType { get; init; }
}