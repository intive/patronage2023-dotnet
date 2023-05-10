using FluentAssertions;

using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.Budget.GettingBudgets;

///<summary>
///This class contains integration tests for the Example module of the Patronage2023 application.
///</summary>
public class GetBudgetsQueryHandlerTests : AbstractIntegrationTests
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsQueryHandlerTests"/> class.
	/// </summary>
	/// <param name="fixture">The database fixture.</param>
	public GetBudgetsQueryHandlerTests(MsSqlTests fixture)
		: base(fixture)
	{
	}

	///<summary>
	///Unit test to verify that the GetBudgetsQueryHandler returns a PagedList of budget items.
	///The test creates a budget item in the database, retrieves it using the query handler, and verifies that the result is not null and contains the expected item.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledFirstPage_ShouldReturnPagedList()
	{
		// Arrange
		var scope = this.WebApplicationFactory.Services.CreateScope();
		var dbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
		var command = BudgetAggregate.Create(
			new(Guid.NewGuid()),
			"example name",
			Guid.NewGuid(),
			new Money(1, (Currency)1),
			new Period(DateTime.Now, DateTime.Now.AddDays(1)),
			"icon",
			"description");

		dbContext.Add(command);
		await dbContext.SaveChangesAsync();
		var query = new GetBudgets
		{
			PageSize = 1,
			PageIndex = 1,
			Search = "",
			SortDescriptors = new List<SortDescriptor>
			{
				new SortDescriptor
				{
					ColumnName = "name",
					SortAscending = true
				}
			}
		};

		var handler = new GetBudgetsQueryHandler(dbContext);

		// Act
		var result = await handler.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
	}
}