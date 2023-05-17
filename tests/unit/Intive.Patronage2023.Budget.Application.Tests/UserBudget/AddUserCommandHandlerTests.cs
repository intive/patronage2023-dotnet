using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Domain;

using Moq;

using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests.UserBudget;

/// <summary>
/// Test suite for verifying the behavior of the query handler class "AddUserBudgetCommnadHandler".
/// </summary>
public class AddUserBudgetCommandHandlerTests
{
	private readonly Mock<IRepository<UserBudgetAggregate, Guid>> userBudgetRepository;
	private readonly HandleAddUserBudget instance;
	/// <summary>
	/// Initializes a new instance of the <see cref="AddUserBudgetCommandHandlerTests"/> class.
	/// </summary>
	public AddUserBudgetCommandHandlerTests()
	{
		this.userBudgetRepository = new Mock<IRepository<UserBudgetAggregate, Guid>>();
		this.instance = new HandleAddUserBudget(this.userBudgetRepository.Object);
	}
	/// <summary>
	/// Test that check if the method correctly creates an instance of "UserBudgetAggregate" with the expected values.
	/// </summary>
	[Fact]
	public async Task Handle_WhenValidCommandPassed_ShouldPersistUserBudgetInRepository()
	{
		// Arrange
		var id = Guid.NewGuid();
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var userRole = UserRole.BudgetOwner;
		var userBudget = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		this.userBudgetRepository.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(userBudget);
		var addUserBudget = new AddUserBudget(id, userId, budgetId, userRole);
		
		//Act
		await this.instance.Handle(addUserBudget, new CancellationToken());
		
		//Assert
		this.userBudgetRepository.Verify(x =>
			x.Persist(It.Is<UserBudgetAggregate>(e =>
				e.Id == id &&
				e.BudgetId == budgetId &&
				e.UserId == userId &&
				e.UserRole == userRole)));
	}
}