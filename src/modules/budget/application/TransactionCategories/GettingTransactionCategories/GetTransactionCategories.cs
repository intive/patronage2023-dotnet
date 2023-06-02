using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// Represents a request to retrieve categories for a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget for which to retrieve the categories.</param>
public record GetTransactionCategories(BudgetId BudgetId) : IQuery<TransactionCategoriesInfo>;

/// <summary>
/// Handles the query for retrieving transaction categories from the database.
/// </summary>
public class GetTransactionCategoriesQueryHandler : IQueryHandler<GetTransactionCategories, TransactionCategoriesInfo>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoriesQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetTransactionCategoriesQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// Handles the GetTransactionCategories query.
	/// </summary>
	/// <param name="query">The GetTransactionCategories query.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation that returns the TransactionCategoriesInfo.</returns>
	public Task<TransactionCategoriesInfo> Handle(GetTransactionCategories query, CancellationToken cancellationToken)
	{
		var providers = new List<ICategoryProvider>
		{
			new StaticCategoryProvider(),
			new DatabaseCategoryProvider(this.budgetDbContext, new BudgetId(query.BudgetId.Value)),
		};

		var categories = new CompositeCategoryProvider(providers).GetAll();
		return Task.FromResult(new TransactionCategoriesInfo(categories));
	}
}