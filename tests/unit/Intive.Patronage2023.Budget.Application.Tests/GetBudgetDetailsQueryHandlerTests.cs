using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Xunit;
using Currency = Intive.Patronage2023.Shared.Infrastructure.Domain.Currency;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "GetBudgetDetailsQueryHandlerTests".
/// </summary>
public class GetBudgetDetailsQueryHandlerTests
{
	/// <summary>
	/// Test that check if the query returns expected values from database.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public async Task Handle_WhenCalledOnExistingBudget_ShouldReturnBudgetDetailsInfo()
	{
		// Arrange
		var id = new BudgetId(Guid.NewGuid());
		string budgetName = new Faker().Random.Word();
		var userId = Guid.NewGuid();
		var limit = new Money(new Faker().Random.Decimal(min: .1M), Currency.PLN);
		var startDate = new Faker().Date.Recent();
		var endDate = startDate.AddDays(1);
		var period = new Period(startDate, endDate);
		string icon = new Faker().Random.Word();
		string description = new Faker().Lorem.Paragraph();

		var budget = BudgetAggregate.Create(id, budgetName, userId, limit, period, icon, description);

		//this.budgetDbContext.Budget.Add(budget);
		//this.budgetDbContext.SaveChanges();

		var query = new GetBudgetDetails { Id = id.Value };
		var cancellationToken = CancellationToken.None;
		var handler = new GetBudgetDetailsQueryHandler(null!); // TODO: Use integration tests db context.

		// Act
		var result = await handler.Handle(query, cancellationToken);

		// Assert
		result.Should().NotBeNull().And.BeEquivalentTo(BudgetAggregateBudgetDetailsInfoMapper.Map(budget));
	}

	/// <summary>
	/// Test that check if the query returns null when budget does not exist in database.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public async Task Handle_WhenTriesToGetNonExistingBudget_ShouldReturnNull()
	{
		var id = Guid.NewGuid();

		var query = new GetBudgetDetails { Id = id };
		var cancellationToken = CancellationToken.None;
		var handler = new GetBudgetDetailsQueryHandler(null!); // TODO: Use integration tests db context.

		// Act
		var result = await handler.Handle(query, cancellationToken);

		// Assert
		result.Should().BeNull();
	}
}