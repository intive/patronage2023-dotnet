using Bogus;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Moq;
using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Test that checks the behavior of a command handler class "CreateBudgetTransaction".
/// </summary>
public class CreateBudgetTransactionCommandHandlerTests
{
	private readonly Mock<IRepository<BudgetTransactionAggregate, TransactionId>> budgetTransactionRepositoryMock;
	private readonly Mock<IKeycloakService> keycloakServiceMock;
	private readonly Mock<IExecutionContextAccessor> contextAccessorMock;
	private readonly HandleCreateBudgetTransaction instance;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetTransactionCommandHandlerTests"/> class.
	/// </summary>
	public CreateBudgetTransactionCommandHandlerTests()
	{
		this.budgetTransactionRepositoryMock = new Mock<IRepository<BudgetTransactionAggregate, TransactionId>>();
		this.keycloakServiceMock = new Mock<IKeycloakService>();
		this.contextAccessorMock = new Mock<IExecutionContextAccessor>();
		this.instance = new HandleCreateBudgetTransaction(this.budgetTransactionRepositoryMock.Object, this.keycloakServiceMock.Object, this.contextAccessorMock.Object);
	}

	/// <summary>
	/// Test that check if the method correctly creates an instance of "BudgetTransactionAggregate" with the expected values
	/// and then passes that instance to a mocked implementation of "IBudgetTransactionRepository" to persist it.
	/// The test uses a "Mock" object to simulate the behavior of the "IBudgetTransactionRepository"
	/// interface and verify that the "Persist" method is called once with the expected values.
	/// </summary>
	[Fact]
	public async void Handle_WhenValidCommandWithIncomePassed_ShouldCreateBudgetTransaction()
	{
		// Arrange
		var cancellationToken = CancellationToken.None;
		var id = new TransactionId(new Faker().Random.Guid());
		var userid = new Faker().Random.Guid();
		var budgetId = new BudgetId(new Faker().Random.Guid());
		var type = new Faker().Random.Enum<TransactionType>();
		string name = new Faker().Name.FirstName();
		string username = new Faker().Random.Word();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();

		if (type == TransactionType.Expense)
			value *= -1;

		this.contextAccessorMock.Setup(x => x.GetUserId()).Returns(userid);
		this.keycloakServiceMock.Setup(x => x.GetUsernameById(userid.ToString(), cancellationToken)).ReturnsAsync(username);
		// Act
		await this.instance.Handle(new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate), cancellationToken);

		// Assert
		this.budgetTransactionRepositoryMock.Verify(
			r =>
				r.Persist(It.Is<BudgetTransactionAggregate>(e =>
				e.Id == id &&
				e.BudgetId == budgetId &&
				e.TransactionType == type &&
				e.Name == name &&
				e.Username == username &&
				e.Value == value &&
				e.CategoryType == category &&
				e.BudgetTransactionDate == createdDate)),
			Times.Once);
	}
}