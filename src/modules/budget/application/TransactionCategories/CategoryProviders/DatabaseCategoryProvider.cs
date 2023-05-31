using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;

/// <summary>
/// Represents a category provider that retrieves transaction categories from a database.
/// </summary>
public class DatabaseCategoryProvider : ICategoryProvider
{
	private readonly BudgetDbContext dbContext;
	private readonly BudgetId budgetId;

	/// <summary>
	/// Initializes a new instance of the <see cref="DatabaseCategoryProvider"/> class.
	/// </summary>
	/// <param name="dbContext">The query bus used to query the database.</param>
	/// <param name="budgetId">The budget ID used to retrieve categories for a specific budget.</param>
	public DatabaseCategoryProvider(BudgetDbContext dbContext, BudgetId budgetId)
	{
		this.budgetId = budgetId;
		this.dbContext = dbContext;
	}

	/// <summary>
	/// Retrieves all transaction categories from the database for given id.
	/// </summary>
	/// <returns>A list of <see cref="TransactionCategory"/> objects representing the transaction categories.</returns>
	public List<TransactionCategory> GetAll()
	{
		var categories = this.dbContext.BudgetTransactionCategory.AsQueryable();

		var entities = categories.Where(x => x.BudgetId == this.budgetId);

		var transactionCategoriesList = entities.MapToBudgetTransactionCategoriesInfo().ToList();

		return transactionCategoriesList;
	}
}