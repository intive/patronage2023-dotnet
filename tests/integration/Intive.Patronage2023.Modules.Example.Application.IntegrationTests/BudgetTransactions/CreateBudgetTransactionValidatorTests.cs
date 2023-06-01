using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.BudgetTransactions;

/// <summary>
/// Create Budget Transaction Validator tests.
/// </summary>
public class CreateBudgetTransactionValidatorTests : AbstractIntegrationTests
{
	private readonly IQueryBus queryBus;
	private readonly Mock<IRepository<BudgetAggregate, BudgetId>> budgetRepositoryMock;
	private readonly IValidator<CreateBudgetTransaction> instance;

	/// <summary>
	/// Constructor of CreateBudgetTransactionValidator.
	/// </summary>
	public CreateBudgetTransactionValidatorTests(MsSqlTests fixture)
		: base(fixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.queryBus = scope.ServiceProvider.GetRequiredService<IQueryBus>();
		this.budgetRepositoryMock = new Mock<IRepository<BudgetAggregate, BudgetId>>();
		this.instance = new CreateBudgetTransactionValidator(this.budgetRepositoryMock.Object, this.queryBus);
	}

	/// <summary>
	/// Validation should return Empty Validation Errors list when all properties are valid.
	/// </summary>
	[Fact]
	public async Task Validate_WhenAllPropertiesAreValid_ShouldNotHaveAnyValidationErrors()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		string budgetName = new Faker().Random.Word();
		var userId = new UserId(new Faker().Random.Guid());
		decimal limitValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var limit = new Money(limitValue, Currency.PLN);
		var period = new Period(new DateTime(2022, 04, 13), new DateTime(2023, 05, 13));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, budgetName, userId, limit, period, icon, description);
		var type = TransactionType.Income;
		var id = new TransactionId(new Faker().Random.Guid());
		string transactionName= new Faker().Random.Word();
		decimal transactionValue = new Faker().Random.Decimal((decimal)0.0001, limitValue);
		string category = "Car";
		var createdDate = new Faker().Date.Between(period.StartDate, period.EndDate);
		var createBudgetTransaction = new CreateBudgetTransaction(type, id.Value, budgetId.Value, transactionName, transactionValue, category, createdDate);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);

		//Act
		var result = await this.instance.TestValidateAsync(createBudgetTransaction);

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
		var transactionId = new TransactionId(new Faker().Random.Guid());
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be not existing BudgetId in database.
		var type = TransactionType.Income;
		string name = new Faker().Name.FirstName();
		decimal value = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		string category = "Car";
		var createdDate = new Faker().Date.Recent();
		var createBudgetTransaction = new CreateBudgetTransaction(type, transactionId.Value, budgetId.Value, name, value, category, createdDate);
		
		//Act
		var result = await this.instance.TestValidateAsync(createBudgetTransaction);

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
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		string budgetName = new Faker().Random.Word();
		var userId = new UserId(new Faker().Random.Guid());
		decimal limitValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var limit = new Money(limitValue, Currency.PLN);
		var period = new Period(new DateTime(2022, 04, 13), new DateTime(2023, 05, 13));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, budgetName, userId, limit, period, icon, description);
		var type = TransactionType.Income;
		var transactionId = new TransactionId(new Faker().Random.Guid());
		string transactionName = new Faker().Random.Word();
		decimal transactionValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999)*-1;
		string category = "Car";
		var transactionCreatedDate = new Faker().Date.Between(period.StartDate, period.EndDate);
		var createBudgetTransaction = new CreateBudgetTransaction(type, transactionId.Value, budgetId.Value, transactionName, transactionValue, category, transactionCreatedDate);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);

		//Act
		var result = await this.instance.TestValidateAsync(createBudgetTransaction);

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
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		string budgetName = new Faker().Random.Word();
		var userId = new UserId(new Faker().Random.Guid());
		decimal limitValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var limit = new Money(limitValue, Currency.PLN);
		var period = new Period(new DateTime(2022, 04, 13), new DateTime(2023, 05, 13));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, budgetName, userId, limit, period, icon, description);
		var type = TransactionType.Expense;
		var transactionId = new TransactionId(new Faker().Random.Guid());
		string transactionName = new Faker().Random.Word();
		decimal transactionValue = new Faker().Random.Decimal((decimal)0.0001, limitValue);
		string category = "Car";
		var transactionCreatedDate = new Faker().Date.Between(period.StartDate, period.EndDate);
		var createBudgetTransaction = new CreateBudgetTransaction(type, transactionId.Value, budgetId.Value, transactionName, transactionValue, category, transactionCreatedDate);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);
		

		//Act
		var result = await this.instance.TestValidateAsync(createBudgetTransaction);

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
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		string budgetName = new Faker().Random.Word();
		var userId = new UserId(new Faker().Random.Guid());
		decimal limitValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		var limit = new Money(limitValue, Currency.PLN);
		var period = new Period(new DateTime(2022, 04, 13), new DateTime(2023, 05, 13));
		string? icon = new Faker().Random.String(1, 10);
		string? description = new Faker().Random.String(1, 10);
		var budget = BudgetAggregate.Create(budgetId, budgetName, userId, limit, period, icon, description);
		var type = TransactionType.Income;
		var transactionId = new TransactionId(new Faker().Random.Guid());
		string transactionName = new Faker().Random.Word();
		decimal transactionValue = new Faker().Random.Decimal((decimal)0.0001, (decimal)9999999999999.9999);
		string category = "Car";
		var transactionDate = new Faker().Date.Past(new Faker().Random.Int(1,10), period.StartDate);
		var createBudgetTransaction = new CreateBudgetTransaction(type, transactionId.Value, budgetId.Value, transactionName, transactionValue, category, transactionDate);
		this.budgetRepositoryMock.Setup(x => x.GetById(It.IsAny<BudgetId>())).ReturnsAsync(budget);
		
		//Act
		var result = await this.instance.TestValidateAsync(createBudgetTransaction);

		//Assert
		result.ShouldHaveValidationErrorFor(x => new { x.BudgetId, x.TransactionDate });
	}
}