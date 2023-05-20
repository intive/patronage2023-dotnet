using Bogus;

using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudgetRole;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.Mappers;
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

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.UserBudget.GettingUserRole;

/// <summary>
/// Unit tests for the GetUserRoleQueryHandler class.
/// </summary>
public class GetUserRoleQueryHandlerTests : AbstractIntegrationTests
{
	private Mock<IExecutionContextAccessor> contextAccessor;
	private readonly GetUserBudgetRoleQueryHandler instance;
	private readonly BudgetDbContext dbContext;
	
	/// <summary>
	/// Initializes a new instance of the GetUserRoleQueryHandlerTests class.
	/// </summary>
	public GetUserRoleQueryHandlerTests(MsSqlTests fixture)
	: base(fixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.dbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
		this.contextAccessor = new Mock<IExecutionContextAccessor>();
		this.instance = new GetUserBudgetRoleQueryHandler(this.dbContext, this.contextAccessor.Object);
	}
	
	/// <summary>
	/// Tests the Handle method when called, it should return the user's budget role.
	/// </summary>
	[Fact]
	public async Task Handle_WhenCalled_ShouldReturnUserBudgetRole()
	{
		// User Budget
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		var userBudget = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		
		//Budget
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());


		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Budget.Add(budget);
		this.dbContext.UserBudget.Add(userBudget);
		await this.dbContext.SaveChangesAsync();

		var query = new GetUserBudgetRole
		{
			BudgetId = budgetId,
		};

		var result = await this.instance.Handle(query, new CancellationToken());
		
		Assert.Equal(UserBudgetAggregateRoleInfoMapper.Map(userRole), result);
	}
}