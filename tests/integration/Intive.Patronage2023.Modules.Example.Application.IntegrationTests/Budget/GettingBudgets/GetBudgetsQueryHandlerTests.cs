using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.Budget.GettingBudgets;

///<summary>
///This class contains integration tests for the Budget module of the Patronage2023 application.
///</summary>
public class GetBudgetsQueryHandlerTests : AbstractIntegrationTests
{
	private readonly Mock<IExecutionContextAccessor>? contextAccessor;
	private readonly GetBudgetsQueryHandler instance;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsQueryHandlerTests"/> class.
	/// </summary>
	/// <param name="fixture">The database fixture.</param>
	public GetBudgetsQueryHandlerTests(MsSqlTests fixture)
		: base(fixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.dbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
		this.contextAccessor = new Mock<IExecutionContextAccessor>();
		this.instance = new GetBudgetsQueryHandler(this.contextAccessor.Object, this.dbContext);
	}

	///<summary>
	///Unit test to verify that the GetBudgetsQueryHandler returns a PagedList of budget items.
	///The test creates a budget item in the database, retrieves it using the query handler, and verifies that the result is not null and contains the expected item.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledFirstPage_ShouldReturnPagedList()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var command = BudgetAggregate.Create(
			budgetId,
			"example name",
			userId,
			new Money(1, (Currency)1),
			new Period(DateTime.Now, DateTime.Now.AddDays(1)),
			"icon",
			"description");

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(command);
		await this.dbContext.SaveChangesAsync();
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

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
	}
}