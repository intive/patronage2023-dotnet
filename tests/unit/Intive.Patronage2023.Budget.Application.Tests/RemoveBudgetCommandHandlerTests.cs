using Bogus;

using Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

using Moq;

using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Class that contains Tests for RemoveBudgetCommandHandler.
/// </summary>
public class RemoveBudgetCommandHandlerTests
{
	private readonly Mock<IRepository<BudgetAggregate, BudgetId>> budgetRepositoryMock;
	private readonly HandleRemoveBudget instance;

	/// <summary>
	/// Initializes a new instance of the <see cref="RemoveBudgetCommandHandlerTests"/> class.
	/// </summary>
	public RemoveBudgetCommandHandlerTests()
	{
		this.budgetRepositoryMock = new Mock<IRepository<BudgetAggregate, BudgetId>>();
		this.instance = new HandleRemoveBudget(this.budgetRepositoryMock.Object);
	}

	/// <summary>
	/// This unit test verifies whether calling Remove Budget Handle method on existing budget sets the budget status to Deleted.
	/// </summary>
	[Fact]
	public async Task Handle_WhenCalledOnExistingBudget_ShouldSetBudgetStatusToDeleted()
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
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);

		var removeBudget = new RemoveBudget(id.Value);
		var cancellationToken = CancellationToken.None;

		// Act
		await this.instance.Handle(removeBudget, cancellationToken);

		// Assert
		this.budgetRepositoryMock.Verify(x =>
			x.Persist(It.Is<BudgetAggregate>(e =>
				e.Status == Status.Deleted)));
	}
}