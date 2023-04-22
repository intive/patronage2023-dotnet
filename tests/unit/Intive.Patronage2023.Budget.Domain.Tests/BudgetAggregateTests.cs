using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Xunit;

namespace Intive.Patronage2023.Budget.Domain.Tests;

/// <summary>
/// Test that checks the behavior of a aggregate class "BudgetAggregate".
/// </summary>
public class BudgetAggregateTests
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetAggregateTests"/> class.
	/// </summary>
	public BudgetAggregateTests()
	{
	}

	/// <summary>
	/// Budget Aggregate test with proper data.
	/// </summary>
	[Fact]
	public void Create_WhenPassedProperData_ShouldCreateBudgetAggregate()
	{
		// Arrange
		var id = Guid.NewGuid();
		string name = new Faker().Random.Word();
		var userId = Guid.NewGuid();
		var limit = new Money(new Faker().Random.Number(50000), Currency.PLN);
		var period = new Period(new DateTime(2023, 04, 13), new DateTime(2023, 05, 13));
		string icon = new Faker().Random.Word();
		string description = new Faker().Lorem.Sentences();

		// Act
		var budgetAggregate = BudgetAggregate.Create(id, name, userId, limit, period, icon, description);

		// Assert
		budgetAggregate.Should().NotBeNull()
			.And.BeEquivalentTo(new
			{
				Id = id,
				Name = name,
				UserId = userId,
				Limit = limit,
				Period = period
			});
	}

	/// <summary>
	/// Budget aggregate create method test with empty guid Id.
	/// </summary>
	[Fact]
	public void Create_WhenPassedEmptyId_ShouldThrowInvalidOperatorExeption()
	{
		// Arrange
		var id = Guid.Empty;
		string name = new Faker().Random.Word();
		var userId = Guid.NewGuid();
		var limit = new Money(new Faker().Random.Number(50000), Currency.PLN);
		var period = new Period(new DateTime(2023, 04, 13), new DateTime(2023, 05, 13));
		string icon = new Faker().Random.Word();
		string describtion = new Faker().Lorem.Sentences();

		// Act
		Action act = () => BudgetAggregate.Create(id, name, userId, limit, period, icon, describtion);

		// Assert
		act.Should().Throw<InvalidOperationException>();
	}
}