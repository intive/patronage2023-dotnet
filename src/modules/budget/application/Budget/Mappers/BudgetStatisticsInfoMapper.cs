using System.Linq.Expressions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetStatisticsInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <returns>Returns <ref name="BudgetInfo"/>Transaction information.</returns>
	public static Expression<Func<BudgetTransactionAggregate, BudgetAmount>> Map =>
		entity => new BudgetAmount
		{
			Value = entity.Value,
			DatePoint = entity.BudgetTransactionDate,
		};
}