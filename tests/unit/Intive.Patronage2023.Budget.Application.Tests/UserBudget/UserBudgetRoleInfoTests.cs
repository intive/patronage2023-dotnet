using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests.UserBudget;

/// <summary>
/// This class contains unit tests for the UserBudgetRoleInfo class in the budget application module.
/// </summary>
public class UserBudgetRoleInfoTests
{
	/// <summary>
	/// It tests the property that retrieves the user's role and ensures it returns the correct user role.
	/// </summary>
	[Fact]
	public void Property_WhenTryToGetUserRole_ShouldReturnCorrectUserRole()
	{
		var userRole = UserRole.BudgetOwner;
		var userBudgetRoleInfo = new Modules.Budget.Application.UserBudgets.GettingUserBudget.UserBudgetRoleInfo
		{
			UserRole = userRole
		};
		
		Assert.Equal(userRole, userBudgetRoleInfo.UserRole);
	}
}