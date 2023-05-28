using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

namespace Intive.Patronage2023.Modules.Budget.Api.Provider;

/// <summary>
/// Provides a implementation returning build in transaction categories.
/// </summary>
public class StaticCategoryProvider : ICategoryProvider
{
	/// <summary>
	/// Retrieves build in transaction categories.
	/// </summary>
	/// <returns>A list of TransactionCategory objects representing different transaction categories.</returns>
	public List<TransactionCategory> GetAll() => new List<TransactionCategory>
	{
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "1",
			Name = "Home Spendings",
		},
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "2",
			Name = "Subscriptions",
		},
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "3",
			Name = "Car",
		},
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "4",
			Name = "Grocery",
		},
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "5",
			Name = "Salary",
		},
		new TransactionCategory
		{
			CategoryId = Guid.NewGuid(),
			Icon = "6",
			Name = "Refund",
		},
	};
}