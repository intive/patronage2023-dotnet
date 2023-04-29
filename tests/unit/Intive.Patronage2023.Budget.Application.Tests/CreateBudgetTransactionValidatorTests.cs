using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Validator tests.
/// </summary>
public class CreateBudgetTransactionValidatorTests
{
	private readonly IValidator<CreateBudgetTransaction> createBudgetTransactionValidator;
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Contructor of CreateBudgetTransactionValidator.
	/// </summary>
	public CreateBudgetTransactionValidatorTests(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.createBudgetTransactionValidator = new CreateBudgetTransactionValidator(budgetRepository);
	}

	/// <summary>
	/// Validator should return true when all properties are valid.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenAllPropertiesAreValid_ShouldReturnTrue()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = new Faker().Random.Enum<TransactionType>();
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal(19, 4);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();
		if (type == TransactionType.Expense)
			value *= -1;
		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = this.createBudgetTransactionValidator.TestValidate(createBudgetTransaction);

		//Assert
		result.ShouldNotHaveAnyValidationErrors();
	}

	/// <summary>
	/// Validator should return false when all properties are valid.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenBudgetIdNotExistedInDatabase_ShouldReturnFalse()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be not existing BudgetId in database.
		var type = new Faker().Random.Enum<TransactionType>();
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal(19, 4);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();
		if (type == TransactionType.Expense)
			value *= -1;
		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = this.createBudgetTransactionValidator.TestValidate(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.BudgetId);
	}

	/// <summary>
	/// Validator should return false when income type transaction have negative value.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenIncomeTypeWithNegativeValue_ShouldReturnFalse()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal(19, 4);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();
		value *= -1;

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = this.createBudgetTransactionValidator.TestValidate(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.Value);
	}

	/// <summary>
	/// Validator should return false when expense type transaction have positive value.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenExpenseTypeWithPositiveValue_ShouldReturnFalse()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal(19, 4);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = this.createBudgetTransactionValidator.TestValidate(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.Value);
	}

	/// <summary>
	/// Validator should return false when tranaction created date is older than month.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenTransactionDataIsOlderThanMonth_ShouldReturnFalse()
	{
		//Arrange
		var id = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal(19, 4);
		var category = new Faker().Random.Enum<CategoryType>();
		var createdDate = new Faker().Date.Recent();

		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, name, value, category, createdDate);

		//Act
		var result = this.createBudgetTransactionValidator.TestValidate(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.TransactionDate);
	}
}