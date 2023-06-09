using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;

/// <summary>
/// Represents a category provider that combines categories from multiple sources.
/// </summary>
public class CompositeCategoryProvider : ICategoryProvider
{
	private readonly IEnumerable<ICategoryProvider> providers;

	/// <summary>
	/// Initializes a new instance of the <see cref="CompositeCategoryProvider"/> class.
	/// </summary>
	/// <param name="providers">Category providers.</param>
	public CompositeCategoryProvider(IEnumerable<ICategoryProvider> providers)
	{
		this.providers = providers;
	}

	/// <summary>
	/// Gets all transaction categories from all available sources.
	/// </summary>
	/// <param name="budgetId">The budget ID used to retrieve categories for a specific budget.</param>
	/// <returns>A list of transaction categories.</returns>
	public List<TransactionCategory> GetForBudget(BudgetId budgetId)
	{
		var categories = this.providers.Select(provider => provider.GetForBudget(budgetId));
		var mergedCategories = categories.SelectMany(category => category).ToList();
		return mergedCategories;
	}
}