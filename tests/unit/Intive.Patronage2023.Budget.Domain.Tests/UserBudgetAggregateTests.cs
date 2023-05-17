using Bogus;

using FluentAssertions;

using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;

using Xunit;

namespace Intive.Patronage2023.Budget.Domain.Tests;

/// <summary>
/// Test that checks the behavior of a aggregate class "UserBudgetAggregate".
/// </summary>
public class UserBudgetAggregateTests
{
	/// <summary>
	/// User Budget Aggregate test with proper data.
	/// </summary>
	[Fact]
	public void Create_WhenPassedProperData_ShouldCreateUserBudgetAggregate()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = new Faker().Random.Enum<UserRole>();
		

		// Act
		var userBudgetAggregate = UserBudgetAggregate.Create(id, userId, budgetId, userRole);

		// Assert
		userBudgetAggregate.Should().NotBeNull()
			.And.BeEquivalentTo(new
			{
				Id = id,
				UserId = userId,
				BudgetId = budgetId,
				UserRole = userRole
			});
	}

	/// <summary>
	/// User Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Property_WhenPassedEmptyId_ShouldThrowInvalidOperatorExeption()
	{
		// Arrange
		var id = Guid.Empty;
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;

		// Act
		Action act = () => UserBudgetAggregate.Create(id, userId, budgetId, userRole);

		// Assert
		act.Should().Throw<InvalidOperationException>();
	}
	
	/// <summary>
	/// User Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Property_WhenTryToGetIdProperty_ShouldReturnCorrectId()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		
		//Act
		var instance = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		
		//Assert
		Assert.Equal(instance.Id, id);
	}
	
	/// <summary>
	/// User Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Property_WhenTryToGetUserIdProperty_ShouldReturnCorrectUserId()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		
		//Act
		var instance = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		
		//Assert
		Assert.Equal(instance.UserId, userId);
	}
	
	/// <summary>
	/// User Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Property_WhenTryToGetBudgetIdProperty_ShouldReturnCorrectBudgetId()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		
		//Act
		var instance = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		
		//Assert
		Assert.Equal(instance.BudgetId, budgetId);
	}
	
	/// <summary>
	/// User Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Property_WhenTryToGetUserRoleProperty_ShouldReturnCorrectUserRole()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		
		//Act
		var instance = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		
		//Assert
		Assert.Equal(instance.UserRole, userRole);
	}
}