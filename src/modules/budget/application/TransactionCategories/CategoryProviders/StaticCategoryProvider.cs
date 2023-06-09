using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;

/// <summary>
/// Provides a implementation returning build in transaction categories.
/// </summary>
public class StaticCategoryProvider : ICategoryProvider
{
	/// <summary>
	/// Retrieves build in transaction categories.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <returns>A list of TransactionCategory objects representing different transaction categories.</returns>
	public List<TransactionCategory> GetForBudget(BudgetId budgetId) => new()
	{
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("1", "#1E4C40", "#F1FBF6"),
			Name = "HomeSpendings",
		},
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("2", "#643400", "#FFF3E5"),
			Name = "Subscriptions",
		},
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("3", "#003150", "#E0F3FF"),
			Name = "Car",
		},
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("4", "#5A092F", "#FDE7F1"),
			Name = "Grocery",
		},
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("5", "#1E4C40", "#A3EAC9"),
			Name = "Salary",
		},
		new TransactionCategory
		{
			CategoryId = default,
			Icon = new Icon("6", "#D1A11F", "#FFF7E0"),
			Name = "Refund",
		},
	};
}