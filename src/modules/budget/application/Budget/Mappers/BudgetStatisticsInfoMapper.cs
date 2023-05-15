using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetStatisticsInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <returns>Returns <ref name="query"/>Transaction information.</returns>
	public static IQueryable<BudgetAmount> MapToBudgetAmount(this IQueryable<BudgetTransactionAggregate> query) =>
		query.Select(x => new BudgetAmount
		{
			Value = x.Value,
			DatePoint = x.BudgetTransactionDate.ToISO8601(),
		});
}