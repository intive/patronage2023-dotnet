using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class TransactionCategoriesInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Entity to be mapped.</param>
	/// <returns>Returns transaction categories.</returns>
	public static IQueryable<TransactionCategory> MapToBudgetTransactionCategoriesInfo(this IQueryable<TransactionCategoryAggregate> query) =>
		query.Select(x => new TransactionCategory
		{
			Icon = x.Icon,
			Name = x.Name,
		});
}