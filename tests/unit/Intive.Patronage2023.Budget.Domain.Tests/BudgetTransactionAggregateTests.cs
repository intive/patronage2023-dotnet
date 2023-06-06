using Bogus;
using FluentAssertions;

using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

using Xunit;

namespace Intive.Patronage2023.Budget.Domain.Tests;

/// <summary>
/// Test that checks the behavior of a aggregate class "BudgetTransactionAggregate".
/// </summary>
public class BudgetTransactionAggregateTests
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionAggregateTests"/> class.
	/// </summary>
	public BudgetTransactionAggregateTests()
	{
	}

	/// <summary>
	/// Budget Transaction Aggregate test with proper data.
	/// </summary>
	[Fact]
	public void Create_WhenPassedProperData_ShouldCreateBudgetTransactionAggregate()
	{
		// Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid());
		var type = new Faker().Random.Enum<TransactionType>();
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var categoryType = new CategoryType("Car");
		var createdDate = new Faker().Date.Recent();
		if (type == TransactionType.Expense)
			value *= -1;

		// Act
		var budgetTransactionAggregate = BudgetTransactionAggregate.Create(id, budgetId, type, name, value, categoryType, createdDate);

		// Assert
		budgetTransactionAggregate.Should()
			.NotBeNull()
			.And
			.BeEquivalentTo(
				new
				{
					Id = id,
					BudgetId = budgetId,
					TransactionType = type,
					Name = name,
					Value = value,
					CategoryType = categoryType,
					BudgetTransactionDate = createdDate
				});
	}
}