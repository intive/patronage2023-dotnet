namespace Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

/// <summary>
/// This enum represents user role in the budget.
/// </summary>
public enum UserRole
{
	/// <summary>
	/// A user with the "BudgetOwner" role has full control over
	/// a budget and can perform any operation on it.
	/// </summary>
	BudgetOwner = 1,

	/// <summary>
	/// A user with the "BudgetUser" role has limited access
	/// and can only perform certain actions on the budget.
	/// </summary>
	BudgetUser = 2,
}