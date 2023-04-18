using System;
using Bogus;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Xunit.Abstractions;

namespace BudgetAggregateTests;

public class BudgetAggregateTests
{
	private readonly ITestOutputHelper output;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetAggregateTests"/> class.
	/// </summary>
	/// <param name="output">Parameter is of type "ITestOutputHelper".</param>
	public BudgetAggregateTests(ITestOutputHelper output)
	{
		this.output = output;
	}

	/// <summary>
	/// Budget Aggregate test with proper data.
	/// </summary>
	[Fact]
	public void Create_WhenPassedProperData_ShouldCreateBudgetAggregate()
	{
		// Arrange
		Guid id = Guid.NewGuid();
		string name = new Faker().Random.Word();
		Guid userId = Guid.NewGuid();
		BudgetLimit limit = new BudgetLimit(new Faker().Random.Number(50000), Currency.PLN);
		BudgetPeriod period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));
		string icon = new Faker().Random.Word();
		string describtion = new Faker().Lorem.Sentences();

		// Act
		var budgetAggregate = BudgetAggregate.Create(id, name, userId, limit, period, icon, describtion);

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
		Guid id = Guid.Empty;
		string name = new Faker().Random.Word();
		Guid userId = Guid.NewGuid();
		BudgetLimit limit = new BudgetLimit(new Faker().Random.Number(50000), Currency.PLN);
		BudgetPeriod period = new BudgetPeriod(new DateOnly(2023, 04, 13), new DateOnly(2023, 05, 13));
		string icon = new Faker().Random.Word();
		string describtion = new Faker().Lorem.Sentences();

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() =>
			 BudgetAggregate.Create(id, name, userId, limit, period, icon, describtion));
	}
}