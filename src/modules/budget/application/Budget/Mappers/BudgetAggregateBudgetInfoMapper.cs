using System.Linq.Expressions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
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
	/// <returns>Returns <ref name="BudgetInfo"/>Budget information.</returns>
	public static Expression<Func<BudgetAggregate, BudgetInfo>> Map =>
		 entity => new BudgetInfo() { Id = entity.BudgetId, Name = entity.Name, CreatedOn = entity.CreatedOn };
}