using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.UpdateUserBudgetFavourite;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests.UserBudget;

/// <summary>
/// Class that contains Tests for UpdateUserBudgetFavourite CommandHandler.
/// </summary>
public class UpdateUserBudgetFavouriteCommandHandlerTests : IDisposable
{
	private readonly Mock<IRepository<UserBudgetAggregate, Guid>> userBudgetRepositoryMock;
	private readonly Mock<IExecutionContextAccessor> contextAccessorMock;
	private readonly HandleUpdateUserBudgetFavourite instance;
	private readonly SqliteConnection connection;
	private readonly BudgetDbContext context;

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdateUserBudgetFavouriteCommandHandlerTests"/> class.
	/// </summary>
	public UpdateUserBudgetFavouriteCommandHandlerTests()
	{
		this.connection = new SqliteConnection("Filename=:memory:");
		this.connection.Open();

		// These options will be used by the context instances in this test suite, including the connection opened above.
		var contextOptions = new DbContextOptionsBuilder<BudgetDbContext>()
			.UseSqlite(this.connection)
			.Options;

		this.context = new BudgetDbContext(contextOptions);
		this.context.Database.EnsureCreated();

		this.userBudgetRepositoryMock = new Mock<IRepository<UserBudgetAggregate, Guid>>();
		this.contextAccessorMock = new Mock<IExecutionContextAccessor>();
		this.instance = new HandleUpdateUserBudgetFavourite(this.context, this.contextAccessorMock.Object, this.userBudgetRepositoryMock.Object);
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		this.connection.Dispose();
		this.context.Dispose();
	}

	/// <summary>
	/// This unit tests verifies whether calling UpdateUserBudgetFavourite Handle method on UserBudget with IsFavourite flag value false is set to true.
	/// </summary>
	[Fact]
	public async Task Handle_WhenTriesToSetFavouriteFlagToTrue_ShouldSetUserBudgetStatusToTrue()
	{
		// Arrange
		var cancellationToken = CancellationToken.None;

		var id = Guid.NewGuid();
		var userIdGuid = Guid.NewGuid();
		var userId = new UserId(userIdGuid);
		var budgetIdGuid = Guid.NewGuid();
		var budgetId = new BudgetId(budgetIdGuid);
		var userRole = UserRole.BudgetUser;
		var userBudget = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		this.contextAccessorMock.Setup(x => x.GetUserId()).Returns(userIdGuid);

		this.userBudgetRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(userBudget);

		var updateUserBudgetFavourite = new UpdateUserBudgetFavourite(budgetIdGuid, true);

		// Act
		await this.instance.Handle(updateUserBudgetFavourite, cancellationToken);

		// Assert
		this.userBudgetRepositoryMock.Verify(x =>
			x.Persist(It.Is<UserBudgetAggregate>(e => e.IsFavourite)));
	}

	/// <summary>
	/// This unit tests verifies whether calling UpdateUserBudgetFavourite Handle method on UserBudget with IsFavourite flag value true is set to false.
	/// </summary>
	[Fact]
	public async Task Handle_WhenTriesToSetFavouriteFlagToFalse_ShouldSetUserBudgetStatusToFalse()
	{
		// Arrange
		var cancellationToken = CancellationToken.None;

		var id = Guid.NewGuid();
		var userIdGuid = Guid.NewGuid();
		var userId = new UserId(userIdGuid);
		var budgetIdGuid = Guid.NewGuid();
		var budgetId = new BudgetId(budgetIdGuid);
		var userRole = UserRole.BudgetUser;
		var userBudget = UserBudgetAggregate.Create(id, userId, budgetId, userRole);
		this.contextAccessorMock.Setup(x => x.GetUserId()).Returns(userIdGuid);

		this.userBudgetRepositoryMock.Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(userBudget);

		var updateUserBudgetFavouriteToTrue = new UpdateUserBudgetFavourite(budgetIdGuid, true);
		await this.instance.Handle(updateUserBudgetFavouriteToTrue, cancellationToken);

		var updateUserBudgetFavouriteToFalse = new UpdateUserBudgetFavourite(budgetIdGuid, false);

		// Act
		await this.instance.Handle(updateUserBudgetFavouriteToFalse, cancellationToken);

		// Assert
		this.userBudgetRepositoryMock.Verify(x =>
			x.Persist(It.Is<UserBudgetAggregate>(e => !e.IsFavourite)));
	}
}