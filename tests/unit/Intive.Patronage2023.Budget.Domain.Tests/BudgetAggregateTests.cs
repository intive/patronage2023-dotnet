using System;
using Xunit;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Example.Domain.Tests;

/// <summary>
/// Budget aggregate tests class.
/// </summary>
public class BudgetAggregateTests
{
	/// <summary>
	/// Budget aggregate create method test with proper data.
	/// </summary>
	[Fact]
	public void Create_WhenPassedProperData_ShouldCreateBudgetAggregate()
	{
		// Arrange
		var id = Guid.NewGuid();
		var name = "Test Budget";
		var userId = Guid.NewGuid();
		var limit = new BudgetLimit(1000, Currency.PLN);
		var period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));

		// Act
		var budgetAggregate = BudgetAggregate.Create(id, name, userId, limit, period);

		// Assert
		Assert.NotNull(budgetAggregate);
		Assert.Equal(id, budgetAggregate.Id);
		Assert.Equal(name, budgetAggregate.Name);
		Assert.Equal(userId, budgetAggregate.UserId);
		Assert.Equal(limit, budgetAggregate.Limit);
		Assert.Equal(period, budgetAggregate.Period);
	}

	/// <summary>
	/// Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Create_WhenPassedEmptyId_ShouldThrowInvalidOperatorExeption()
	{
		// Arrange
		var id = Guid.Empty;
		var name = "Test Budget";
		var userId = Guid.NewGuid();
		var limit = new BudgetLimit(1000, Currency.PLN);
		var period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() =>
			BudgetAggregate.Create(id, name, userId, limit, period));
	}
}