namespace Intive.Patronage2023.Modules.Budget.Contracts.Helpers;

/// <summary>
/// Enumeration of budget transaction types.
/// </summary>
public enum TransactionType
{
	/// <summary>
	/// Income which will be added to your budget.
	/// </summary>
	Income = 1,

	/// <summary>
	/// Expense which will be added to your budget.
	/// </summary>
	Expense = 2,
}