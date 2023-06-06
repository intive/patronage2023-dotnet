using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetTransactionAggregateGetBudgetTransactionsToExportInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Entity to be mapped.</param>
	/// <returns>Returns Budget details information.</returns>
	public static IQueryable<GetBudgetTransactionTransferInfo> MapToGetBudgetTransactionTransferInfo(this IQueryable<BudgetTransactionAggregate> query) =>
		query.Select(x => new GetBudgetTransactionTransferInfo
		{
			Name = x.Name,
			CategoryType = x.CategoryType.ToString(),
			Date = x.BudgetTransactionDate.ToString(),
			TransactionType = x.TransactionType.ToString(),
			Value = x.Value.ToString(),
			Status = x.Status.ToString(),
		});
}