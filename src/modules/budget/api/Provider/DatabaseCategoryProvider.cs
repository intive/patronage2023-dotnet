using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Api.Provider;

/// <summary>
/// Represents a category provider that retrieves transaction categories from a database.
/// </summary>
public class DatabaseCategoryProvider : ICategoryProvider
{
	private readonly IQueryBus queryBus;
	private readonly BudgetId budgetId;

	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseCategoryProvider"/> class.
	/// </summary>
	/// <param name="queryBus">The query bus used to query the database.</param>
	/// <param name="budgetId">The budget ID used to retrieve categories for a specific budget.</param>
	public DatabaseCategoryProvider(IQueryBus queryBus, BudgetId budgetId)
	{
		this.queryBus = queryBus;
		this.budgetId = budgetId;
	}

	/// <summary>
	/// Retrieves all transaction categories from the database for given id.
	/// </summary>
	/// <returns>A list of <see cref="TransactionCategory"/> objects representing the transaction categories.</returns>
	public List<TransactionCategory> GetAll()
	{
		var query = new GetTransactionCategoriesFromDatabase(new BudgetId(this.budgetId.Value));
		var categories = this.queryBus.Query<GetTransactionCategoriesFromDatabase, TransactionCategoriesInfo>(query);
		return categories.Result.BudgetCategoryList!;
	}
}