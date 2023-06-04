using System.Linq.Expressions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
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
	/// <returns>Returns <ref name="TransactionInfo"/>BudgetInfo.</returns>
	public static Expression<Func<BudgetTransactionAggregate, BudgetTransactionInfo>> Map =>
		entity => new BudgetTransactionInfo
		{
			TransactionType = entity.TransactionType,
			BudgetId = entity.BudgetId,
			TransactionId = entity.Id,
			Name = entity.Name,
			Value = entity.Value,
			BudgetTransactionDate = entity.BudgetTransactionDate,
			CategoryType = entity.CategoryType,
			AttachmentUrl = entity.AttachmentUrl,
		};
}