namespace Intive.Patronage2023.Modules.Budget.Contracts.Provider;

/// <summary>
/// Provides a contract for retrieving a list of transaction categories.
/// </summary>
public interface ICategoryProvider
{
	/// <summary>
	/// Retrieves all transaction categories.
	/// </summary>
	/// <returns>A list of transaction categories.</returns>
	List<TransactionCategory> GetAll();
}