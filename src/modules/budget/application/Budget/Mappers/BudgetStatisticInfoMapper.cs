using System.Linq.Expressions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetStatisticInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <returns>Returns <ref name="BudgetInfo"/>Transaction information.</returns>
	public static Expression<Func<BudgetTransactionAggregate, BudgetStatisticInfo>> Map =>
		 entity => new BudgetStatisticInfo() { BudgetId = entity.BudgetId, Value = entity.Value, BudgetTransactionDate = entity.BudgetTransactionDate };
}