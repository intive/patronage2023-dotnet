using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.Mappers;

/// <summary>
/// sfasdfasdf.
/// </summary>
public static class UserBudgetAggregateRoleInfoMapper
{
	/// <summary>
	/// sfasdfasdfa.
	/// </summary>
	/// <param name="entity">safsdfadf.</param>
	/// <returns>asdfasdf.</returns>
	public static UserBudgetRoleInfo Map(UserBudgetAggregate entity) =>
		new()
		{
			UserRole = entity.UserRole,
		};
}