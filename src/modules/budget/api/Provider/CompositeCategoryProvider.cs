using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

namespace Intive.Patronage2023.Modules.Budget.Api.Provider;

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
	public List<TransactionCategory> GetAll() => this.providers.SelectMany(provider => provider.GetAll()).ToList();
}