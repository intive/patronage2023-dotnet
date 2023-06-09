using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
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
	private readonly ICategoryProvider categoryProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoriesQueryHandler"/> class.
	/// </summary>
	/// <param name="categoryProvider">Budget dbContext.</param>
	public GetTransactionCategoriesQueryHandler(ICategoryProvider categoryProvider)
	{
		this.categoryProvider = categoryProvider;
	}

	/// <summary>
	/// Handles the GetTransactionCategories query.
	/// </summary>
	/// <param name="query">The GetTransactionCategories query.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation that returns the TransactionCategoriesInfo.</returns>
	public Task<TransactionCategoriesInfo> Handle(GetTransactionCategories query, CancellationToken cancellationToken)
	{
		var categories = this.categoryProvider.GetForBudget(query.BudgetId);
		return Task.FromResult(new TransactionCategoriesInfo(categories));
	}
}