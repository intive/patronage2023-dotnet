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
	/// <param name="entity">Entity to be mapped.</param>
	/// <returns>Returns <ref name="BudgetInfo"/>BudgetInfo.</returns>
	public static BudgetInfo Map(BudgetAggregate entity) =>
		 new(entity.BudgetId, entity.Name, entity.CreatedOn);
}