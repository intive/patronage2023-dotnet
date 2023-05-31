using Intive.Patronage2023.Modules.Budget.Contracts.Provider;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;

/// <summary>
/// Represents a category provider that combines categories from multiple sources.
/// </summary>
public class CompositeCategoryProvider : ICategoryProvider
{
	private readonly List<ICategoryProvider> providers;

	/// <summary>
	/// Initializes a new instance of the <see cref="CompositeCategoryProvider"/> class.
	/// </summary>
	/// <param name="providers">1.</param>
	public CompositeCategoryProvider(List<ICategoryProvider> providers)
	{
		this.providers = providers;
	}

	/// <summary>
	/// Gets all transaction categories from all available sources.
	/// </summary>
	/// <returns>A list of transaction categories.</returns>
	public List<TransactionCategory> GetAll()
	{
		var categories = this.providers.Select(provider => provider.GetAll());
		var mergedCategories = categories.SelectMany(category => category).ToList();
		return mergedCategories;
	}
}