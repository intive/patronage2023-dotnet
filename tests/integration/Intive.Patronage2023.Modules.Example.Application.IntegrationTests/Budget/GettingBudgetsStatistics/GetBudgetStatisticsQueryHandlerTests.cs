using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistics;
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

namespace Intive.Patronage2023.Modules.Budget.Application.IntegrationTests.Budget.GettingBudgetsStatistics;

///<summary>
///This class contains integration tests for getting Budget Statistics in the Budget module of the Patronage2023 application.
///</summary>
public class GetBudgetStatisticsQueryHandlerTests : AbstractIntegrationTests
{
	private readonly Mock<IExecutionContextAccessor>? contextAccessor;
	private readonly GetBudgetStatisticQueryHandler instance;
	private readonly BudgetDbContext dbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetStatisticsQueryHandlerTests"/> class.
	/// </summary>
	/// <param name="fixture">The database fixture.</param>
	public GetBudgetStatisticsQueryHandlerTests(MsSqlTests fixture)
		: base(fixture)
	{
		var scope = this.WebApplicationFactory.Services.CreateScope();
		this.dbContext = scope.ServiceProvider.GetRequiredService<BudgetDbContext>();
		this.contextAccessor = new Mock<IExecutionContextAccessor>();
		this.instance = new GetBudgetStatisticQueryHandler(this.dbContext);
	}


	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler returns correct TotalBudgetValue (which is sum of all transactions in your budget).
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithTransactions_ShouldCalculateTotalBudgetValueAsSumOfTransactions()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());
			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				i,
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);
			this.dbContext.Transaction.Add(income);
		}
		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate,
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);


		result.Should().NotBeNull();
		result.TotalBudgetValue.Should().Be(45);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler returns correct period value (which is sum of transaction values between start date and end date).
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithTransactions_ShouldCalculatePeriodValueAsSumOfTransactionsBetweenTwoDates()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());
			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				i,
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);
			this.dbContext.Transaction.Add(income);
		}
		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate,
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);
		result.Should().NotBeNull();
		result.PeriodValue.Should().Be(45);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler returns correct budgetTransactionsValues (which is a list of transaction values grouped and sum by day).
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledBudgetOnBudgetWithTransactions_ShouldCalculateBudgetBalance()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());
			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				i,
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);
			this.dbContext.Transaction.Add(income);
		}
		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate.AddDays(2),
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);

		result.Items.First().Value.Should().Be(1);
		result.Items.Last().Value.Should().Be(45);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler counts expenses.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithExpenses_ShouldCalculateBudgetBalance()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());
			var expenseId = new TransactionId(Guid.NewGuid());

			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(i + 10),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			var expense = BudgetTransactionAggregate.Create(
				expenseId,
				budgetId,
				TransactionType.Expense,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(-i),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			this.dbContext.Transaction.Add(income);
			this.dbContext.Transaction.Add(expense);
		}



		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate.AddDays(2),
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);

		result.Items.Last().Value.Should().Be(100);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler does not count cancelled transactions.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithCancelledTransactions_ShouldCalculateBudgetBalance()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());;

			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(i + 10),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			if(i % 2 == 0)
			{
				income.CancelTransaction();
			}
			this.dbContext.Transaction.Add(income);
		}



		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate.AddDays(2),
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);

		result.Items.Last().Value.Should().Be(75);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler should not count cancelled transactions and should count Expenses.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithCancelledTransactionsAndExpenses_ShouldCalculateBudgetBalance()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 0; i < 10; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());
			var expenseId = new TransactionId(Guid.NewGuid());

			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(i + 10),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			var expense = BudgetTransactionAggregate.Create(
				expenseId,
				budgetId,
				TransactionType.Expense,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(-i),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			if (i % 2 == 0)
			{
				income.CancelTransaction();
			}
			this.dbContext.Transaction.Add(income);
			this.dbContext.Transaction.Add(expense);
		}



		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate.AddDays(2),
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);

		result.Items.Last().Value.Should().Be(30);
	}

	///<summary>
	///Integration test that verifes if GetBudgetStatisticQueryHandler counts sum of transactions by day.
	///</summary>
	[Fact]
	public async Task Handle_WhenCalledOnBudgetWithTransactions_ShouldCalculateBudgetBalanceEveryTransaction()
	{
		// Arrange
		var userId = new UserId(Guid.NewGuid());
		var budgetId = new BudgetId(Guid.NewGuid());
		var period = new Period(new Faker().Date.Recent(), DateTime.Now.AddDays(20));
		var budget = BudgetAggregate.Create(
			budgetId,
			new Faker().Random.Word(),
			userId,
			new Money(new Faker().Random.UShort(1000, 2000), new Faker().Random.Enum<Currency>()),
			period,
			new Faker().Lorem.Paragraph(),
			new Faker().Random.Word());

		var userBudget = UserBudgetAggregate.Create(Guid.NewGuid(), userId, budgetId, UserRole.BudgetOwner);

		this.dbContext.UserBudget.Add(userBudget);
		this.contextAccessor!.Setup(x => x.GetUserId()).Returns(userId.Value);
		this.contextAccessor.Setup(x => x.IsAdmin()).Returns(false);
		this.dbContext.Add(budget);

		for (int i = 1; i < 6; i++)
		{
			var transactionPeriod = period.StartDate.AddDays(i);
			var incomeId = new TransactionId(Guid.NewGuid());

			var income = BudgetTransactionAggregate.Create(
				incomeId,
				budgetId,
				TransactionType.Income,
				new Faker().Random.Word(),
				new Faker().Random.Word(),
				(i),
				new Faker().Random.Enum<CategoryType>(),
				transactionPeriod);

			this.dbContext.Transaction.Add(income);
		}

		await this.dbContext.SaveChangesAsync();

		var budgetStatisticsQuery = new GetBudgetStatistics
		{
			Id = budgetId.Value,
			StartDate = period.StartDate,
			EndDate = period.EndDate,
		};


		// Act
		var result = await this.instance.Handle(budgetStatisticsQuery, CancellationToken.None);

		result.Items.Should().BeEquivalentTo(new List<BudgetAmount>()
		{
			new BudgetAmount() { Value = 0 },
			new BudgetAmount() { Value = 1 },
			new BudgetAmount() { Value = 3 },
			new BudgetAmount() { Value = 6 },
			new BudgetAmount() { Value = 10 },
			new BudgetAmount() { Value = 15 },
		},x => x.Excluding(y => y.DatePoint));
	}
}