namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// Validation object add users to budget.
/// </summary>
/// <param name="UsersIds">Users ids.</param>
/// <param name="BudgetId">Owner id.</param>
public record AddUsersToBudget(
	Guid[] UsersIds,
	Guid BudgetId);