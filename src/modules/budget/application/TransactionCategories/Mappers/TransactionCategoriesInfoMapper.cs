using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.Mappers;

/// <summary>
/// Provides mapping methods for TransactionCategoryAggregate to TransactionCategory.
/// </summary>
public static class TransactionCategoriesInfoMapper
{
	/// <summary>
	/// Maps an IQueryable of TransactionCategoryAggregate to IQueryable of TransactionCategory.
	/// </summary>
	/// <param name="query">The entity to be mapped.</param>
	/// <returns>Returns an IQueryable of transaction categories.</returns>
	public static IQueryable<TransactionCategory> MapToBudgetTransactionCategoriesInfo(this IQueryable<TransactionCategoryAggregate> query) =>
		query.Select(x => new TransactionCategory
		{
			CategoryId = x.Id.Value,
			Icon = x.Icon,
			Name = x.Name,
		});
}