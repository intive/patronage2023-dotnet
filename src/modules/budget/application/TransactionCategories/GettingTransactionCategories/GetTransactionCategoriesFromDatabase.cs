using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// Represents a request to retrieve categories for a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget for which to retrieve the categories.</param>
public record GetTransactionCategoriesFromDatabase(BudgetId BudgetId) : IQuery<TransactionCategoriesInfo>;

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetTransactionCategoriesFromDatabaseQueryHandler : IQueryHandler<GetTransactionCategoriesFromDatabase, TransactionCategoriesInfo>
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoriesFromDatabaseQueryHandler"/> class.
	/// </summary>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetTransactionCategoriesFromDatabaseQueryHandler(IExecutionContextAccessor contextAccessor, BudgetDbContext budgetDbContext)
	{
		this.contextAccessor = contextAccessor;
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<TransactionCategoriesInfo> Handle(GetTransactionCategoriesFromDatabase query, CancellationToken cancellationToken)
	{
		var categories = this.budgetDbContext.BudgetTransactionCategory.AsQueryable();

		var entities = categories.Where(x => x.BudgetId == query.BudgetId);

		var transactionCategoriesList = await entities.MapToBudgetTransactionCategoriesInfo().ToListAsync(cancellationToken: cancellationToken);

		return new TransactionCategoriesInfo(transactionCategoriesList);
	}
}