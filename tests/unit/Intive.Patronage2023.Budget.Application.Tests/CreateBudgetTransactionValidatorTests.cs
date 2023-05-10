using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

using Moq;

using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Create Budget Transaction Validator tests.
/// </summary>
public class CreateBudgetTransactionValidatorTests
{
	private readonly Mock<IRepository<BudgetAggregate, BudgetId>> budgetRepositoryMock;
	private readonly IValidator<CreateBudgetTransaction> createBudgetTransactionValidator;
	/// <summary>
	/// Constructor of CreateBudgetTransactionValidator.
	/// </summary>
	public CreateBudgetTransactionValidatorTests()
	{
		this.budgetRepositoryMock = new Mock<IRepository<BudgetAggregate, BudgetId>>();
		this.createBudgetTransactionValidator = new CreateBudgetTransactionValidator(this.budgetRepositoryMock.Object);
	}

	/// <summary>
	/// Validation should return Empty Validation Errors list when all properties are valid.
	/// </summary>
	[Fact]
	public async Task Validate_WhenAllPropertiesAreValid_ShouldNotHaveAnyValidationErrors()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();
		var userId = new UserId(new Faker().Random.Guid());
		var limit = new Money(value, Currency.PLN);
		var period = new Period(new Faker().Date.Recent(), new Faker().Date.Recent().AddMonths(1));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, name, userId, limit, period, icon, description);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);
		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = await this.createBudgetTransactionValidator.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	/// <summary>
	/// Validation should return Validation Error List when budget with given id does not exist in database.
	/// </summary>
	[Fact]
	public async Task Validate_WhenBudgetIdDoesNotExistInDatabase_ShouldHaveValidationErrorForBudgetId()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be not existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = await this.createBudgetTransactionValidator.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.BudgetId);
	}

	/// <summary>
	/// Validation should return Validation Error List when income type transaction have negative value.
	/// </summary>
	[Fact]
	public async Task Validate_WhenIncomeTypeWithNegativeValue_ShouldHaveValidationErrorForValue()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();
		value *= -1;

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = await this.createBudgetTransactionValidator.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.Value);
	}

	/// <summary>
	/// Validation should return Validation Error List when expense type transaction have positive value.
	/// </summary>
	[Fact]
	public async Task Validate_WhenExpenseTypeWithPositiveValue_ShouldHaveValidationErrorForValue()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Expense;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = await this.createBudgetTransactionValidator.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.Value);
	}

	/// <summary>
	/// Validation should return Validation Error List when transaction created date is older than month.
	/// </summary>
	[Fact]
	public async Task Validate_WhenTransactionDataIsOlderThanMonth_ShouldHaveValidationErrorForTransactionDate()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var category = new Faker().Random.Enum<CategoryType>();
		var transactionData = DateTime.UtcNow.AddMonths(-2);

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, transactionData);

		//Act
		var result = await this.createBudgetTransactionValidator.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.TransactionDate);
	}
}