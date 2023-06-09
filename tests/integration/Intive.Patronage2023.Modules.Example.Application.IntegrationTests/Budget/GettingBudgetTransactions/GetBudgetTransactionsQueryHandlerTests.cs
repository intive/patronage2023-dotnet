using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
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

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.Budget.GettingBudgetTransactions;

///<summary>
///This class contains integration tests for getting Transactions in the Budget module of the Patronage2023 application.
///</summary>
public class GetTransactionsQueryHandlerTests : AbstractIntegrationTests
{
	private readonly Mock<IExecutionContextAccessor>? contextAccessor;
	private readonly GetTransactionsQueryHandler instance;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionsQueryHandlerTests"/> class.
	/// </summary>
	/// <param name="fixture">The database fixture.</param>
	public GetTransactionsQueryHandlerTests(MsSqlTests fixture)
		: base(fixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.dbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
		this.contextAccessor = new Mock<IExecutionContextAccessor>();
		this.instance = new GetTransactionsQueryHandler(this.dbContext);
	}

	///<summary>
	///Integration test that verifes if query handler returns all transaction types belonging to given budget.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledFirstPageWithNullTransactionType_ShouldReturnPagedListWithAllTransactionTypes()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			new CategoryType("Salary"),
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			TransactionType = null,
			BudgetId = budgetId,
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
	}

	///<summary>
	///Integration test that verifes if query handler returns only income transactions belonging to given budget.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledFirstPageWithIncomeTransactionType_ShouldReturnPagedListWithOnlyIncomeTransactions()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			new CategoryType("Refund"),
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			TransactionType = TransactionType.Income,
			BudgetId = budgetId,
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
		result.Items.First().TransactionType.Should().Be(TransactionType.Income);
	}

	///<summary>
	///Integration test that verifes if query handler returns only transactions with category 'Car' belonging to given budget.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledWithSalaryCategoryType_ShouldReturnPagedListWithTransactionsWithSalaryCategory()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			new CategoryType("Salary"),
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			TransactionType = null,
			CategoryTypes = new [] { new CategoryType("Salary" ) },
			BudgetId = budgetId,
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(1);
		result.Items.First().CategoryType.CategoryName.Should().Be("Salary");
	}

	///<summary>
	///Integration test that verifes if query handler returns transactions with category 'Car' or 'Grocery' belonging to given budget.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledWithCarAndGroceryCategortyType_ShouldReturnPagedListWithTransactionsWithCarOrGroceryCategories()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var grocery = new CategoryType("Grocery");
		var homeSpendings = new CategoryType("HomeSpendings");
		var car = new CategoryType("Car");
		
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			grocery,
			period.StartDate.AddDays(1));

		var incomeIdv2 = new TransactionId(Guid.NewGuid());
		var incomev2 = BudgetTransactionAggregate.Create(
			incomeIdv2,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			homeSpendings,
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			car,
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(incomev2);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			TransactionType = null,
			CategoryTypes = new[] { new CategoryType("Car"), new CategoryType("Grocery")},
			BudgetId = budgetId,
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
		result.Items.Should().OnlyContain(x => x.CategoryType.CategoryName == "Car" || x.CategoryType.CategoryName == "Grocery");
	}

	///<summary>
	///Integration test that verifes if query handler returns all transactions when categoryTypes is null.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledWithNullCategortyTypes_ShouldReturnPagedListWithAllTransactions()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			new CategoryType("Grocery"),
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			new Faker().Random.Word(),
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			TransactionType = null,
			CategoryTypes = null,
			BudgetId = budgetId,
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
	}

	///<summary>
	///Integration test that verifes if query handler returns only transactions containing search text.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledWithSearchText_ShouldReturnPagedListWithTransactionsContainingSearchText()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(10));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.Decimal(0.1M), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		var incomeId = new TransactionId(Guid.NewGuid());
		var income = BudgetTransactionAggregate.Create(
			incomeId,
			budgetId,
			TransactionType.Income,
			"Food",
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M),
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		var expenseId = new TransactionId(Guid.NewGuid());
		var expense = BudgetTransactionAggregate.Create(
			expenseId,
			budgetId,
			TransactionType.Expense,
			"Foo",
			new Faker().Internet.Email(),
			new Faker().Random.Decimal(0.1M) * -1,
			new CategoryType("Car"),
			period.StartDate.AddDays(1));

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);
		this.dbContext.Transaction.Add(income);
		this.dbContext.Transaction.Add(expense);
		await this.dbContext.SaveChangesAsync();
		var query = new GetBudgetTransactions
		{
			PageSize = 10,
			PageIndex = 1,
			BudgetId = budgetId,
			Search = "Fo"
		};

		// Act
		var result = await this.instance.Handle(query, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();
		result.Items.Should().HaveCount(2);
	}
}