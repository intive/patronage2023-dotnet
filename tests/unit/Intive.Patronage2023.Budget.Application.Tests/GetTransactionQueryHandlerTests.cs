using Bogus;
using FluentAssertions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Test that checks the behavior of a query handler class "GetTransactionQueryHandlerTests".
/// </summary>
public class GetTransactionsQueryHandlerTests
{
	/// <summary>
	/// Test.
	/// </summary>
	public GetTransactionsQueryHandlerTests()
	{
	}
	/// <summary>
	/// Test that check if the query returns expected values from database.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public async Task Handle_WhenCalledOnExistingBudget_ShouldReturnPagedListOfTransactionsForTheBudget()
	{
		// Arrange
		var transactionList = new List<BudgetTransactionAggregate>();
		int pageSize = 10;
		int pageIndex = 1;
		var budgetId = new BudgetId(new Faker().Random.Guid());

		for (int i = 0; i < pageSize; i++)
		{
			var id = new TransactionId(new Faker().Random.Guid());
			var transactionType = TransactionType.Income;
			string? name = new Faker().Random.Word();
			string username = new Faker().Random.Word();
			decimal value = new Faker().Random.Decimal(min: .1M);
			var category = new Faker().Random.Enum<CategoryType>();
			var transactionDate = new Faker().Date.Recent();
			var transaction = BudgetTransactionAggregate.Create(id, budgetId, transactionType, name, username, value, category, transactionDate);
			transactionList.Add(transaction);
			// this.budgetDbContext.Transaction.Add(transaction);
		}

		// this.budgetDbContext.SaveChanges();
		var query = new GetBudgetTransactions() { PageSize = pageSize, PageIndex = pageIndex, BudgetId = budgetId };
		var cancellationToken = CancellationToken.None;
		var instance = new GetTransactionsQueryHandler(null!); // TODO: Use integration tests db context.

		// Act
		var result = await instance.Handle(query, cancellationToken);

		// Assert
		result.Should().NotBeNull().And
			.BeEquivalentTo(transactionList); // TODO: It must return PagedList of BudgetTransactionInfo
	}
}