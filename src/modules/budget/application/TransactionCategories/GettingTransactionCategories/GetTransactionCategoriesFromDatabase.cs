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
public record GetTransactionCategoriesFromDatabase(BudgetId BudgetId) : IQuery<TransactionCategoriesInfo>;

/// <summary>
/// Get Budgets handler.
/// </summary>
public class GetTransactionCategoriesFromDatabaseQueryHandler : IQueryHandler<GetTransactionCategoriesFromDatabase, TransactionCategoriesInfo>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoriesFromDatabaseQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetTransactionCategoriesFromDatabaseQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgets query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public Task<TransactionCategoriesInfo> Handle(GetTransactionCategoriesFromDatabase query, CancellationToken cancellationToken)
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