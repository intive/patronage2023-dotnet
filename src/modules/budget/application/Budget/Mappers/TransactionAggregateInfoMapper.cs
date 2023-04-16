using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class TransactionAggregateInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="entity">Entity to be mapped.</param>
	/// <returns>Returns <ref name="TransactionInfo"/>BudgetInfo.</returns>
	public static TransactionInfo Map(TransactionAggregate entity) =>
		 new(entity.TransactionType, entity.BudgetId, entity.Name, entity.Value, entity.CreatedOn, entity.CategoryType);
}