using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
///  The record is used to store information about a user's role in relation to a specific budget.
/// </summary>
public record UserBudgetRoleInfo
{
	/// <summary>
	/// Represents the user's role.
	/// </summary>
	public UserRole? UserRole { get; init; }
}