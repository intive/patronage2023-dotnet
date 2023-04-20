using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetTransactionInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="entity">Entity to be mapped.</param>
	/// <returns>Returns <ref name="TransactionInfo"/>BudgetInfo.</returns>
	public static BudgetTransactionInfo Map(BudgetTransactionAggregate entity) =>
		 new(entity.TransactionType, entity.TransactionId, entity.Name, entity.Value, entity.CreatedOn, entity.CategoryType);
}