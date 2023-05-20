using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetAggregateBudgetInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Entity to be mapped.</param>
	/// <param name="favouriteBudgetDictionary">Dictionary with all budets fourite flag..</param>
	/// <returns>Returns budgets informations.</returns>
	public static IQueryable<BudgetInfo> MapToBudgetInfo(this IQueryable<BudgetAggregate> query, List<BudgetId> favouriteBudgetDictionary) =>
		query.Select(x => new BudgetInfo
		{
			Id = x.Id,
			Name = x.Name,
			CreatedOn = x.CreatedOn,
			Icon = x.Icon,
			IsFavourite = favouriteBudgetDictionary.Contains(x.Id),
		});
}