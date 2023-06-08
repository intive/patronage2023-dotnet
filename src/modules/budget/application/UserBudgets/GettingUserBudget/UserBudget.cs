using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
/// UserBudget.
/// </summary>
/// <param name="Id">UserBudget Id.</param>
/// <param name="UserId">User id.</param>
/// <param name="BudgetId">Budget id.</param>
/// <param name="UserRole">Role of user.</param>
/// <param name="IsFavourite">Favourite flag.</param>
public record UserBudget(Guid Id, UserId UserId, BudgetId BudgetId, UserRole UserRole, bool IsFavourite);