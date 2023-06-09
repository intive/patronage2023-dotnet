using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// Model of Income and Expense.
/// </summary>
public record BudgetTransactionInfo()
{
	/// <summary>
	/// Transaction Type.
	/// </summary>
	public TransactionType TransactionType { get; init; }

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
	public CategoryType CategoryType { get; init; }

	/// <summary>
	/// Transaction attachment url.
	/// </summary>
	public Uri? AttachmentUrl { get; init; } = null!;

	/// <summary>
	/// Transaction creator email.
	/// </summary>
	public string Email { get; init; } = null!;
}