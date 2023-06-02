using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetTransactionTransferInfoBudgetTransactionImportInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="list">Entity to be mapped.</param>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <returns>Returns budget transaction details information for import.</returns>
	public static IEnumerable<GetBudgetTransactionImportInfo> MapToBudgetTransactionImportInfo(this IEnumerable<GetBudgetTransactionTransferInfo> list, BudgetId budgetId) =>
		list.Select(x => new GetBudgetTransactionImportInfo
		{
			BudgetId = budgetId,
			Name = x.Name,
			CategoryType = x.CategoryType,
			Date = x.Date,
			TransactionType = x.TransactionType,
			Value = x.Value,
		});
}