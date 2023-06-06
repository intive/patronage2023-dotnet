using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

using Moq;

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.BudgetTransactions;

/// <summary>
/// Class that contains Get Budget Transaction Validator tests.
/// </summary>
public class GetBudgetTransactionsValidatorTests : AbstractIntegrationTests
{
	private readonly Mock<ICategoryProvider> categoryProvider;
	private readonly Mock<IRepository<BudgetAggregate, BudgetId>> budgetRepositoryMock;
	private readonly IValidator<GetBudgetTransactions> instance;


	/// <summary>
	/// Constructor of GetBudgetTransactionsValidatorTests
	/// </summary>
	public GetBudgetTransactionsValidatorTests(MsSqlTests fixture) 
		: base(fixture)
	{
		this.categoryProvider = new Mock<ICategoryProvider>();
		this.budgetRepositoryMock = new Mock<IRepository<BudgetAggregate, BudgetId>>();
		this.instance = new GetBudgetTransactionValidator(this.budgetRepositoryMock.Object, this.categoryProvider.Object);
	}

	/// <summary>
	/// Validation should return Validation Error List when budget with that not existing in database.
	/// </summary>
	[Fact]
	public async Task Validate_WhenBudgetIdDoesNotExistInDatabase_ShouldHaveValidationErrorForBudgetId()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must not be existing BudgetId in database.
		int pageIndex = 1;
		int pageSize = new Faker().Random.Number(1, 10);

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = await this.instance.TestValidateAsync(getBudgetTransactions);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.BudgetId);
	}

	/// <summary>
	/// Validation should return Validation Error List when page index or page size is lower or equal 0.
	/// </summary>
	[Fact]
	public async Task Validate_WhenPageIndexAndSizeIsLowerOrEqual0_ShouldHaveValidationErrorForPageIndexAndPageSize()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		int pageIndex = 0;
		int pageSize = 0;

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = await this.instance.TestValidateAsync(getBudgetTransactions);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.PageSize);
		result.ShouldHaveValidationErrorFor(x => x.PageIndex);
	}

	/// <summary>
	/// Validation should return empty list of errors when budget with that id existing in database.
	/// </summary>
	[Fact]
	public async Task Validate_WhenAllPropertiesAreValid_ShouldNoReturnValidationErrors()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		int pageIndex = new Faker().Random.Number(1, 10);
		int pageSize = new Faker().Random.Number(1, 10);
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var userId = new UserId(new Faker().Random.Guid());
		var limit = new Money(value, Currency.PLN);
		var period = new Period(new Faker().Date.Recent(), new Faker().Date.Recent().AddMonths(1));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, name, userId, limit, period, icon, description);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = await this.instance.TestValidateAsync(getBudgetTransactions);

		//Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}