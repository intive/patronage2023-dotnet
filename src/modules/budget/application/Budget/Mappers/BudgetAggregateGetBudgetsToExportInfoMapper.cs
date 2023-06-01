using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetAggregateGetBudgetsToExportInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="query">Entity to be mapped.</param>
	/// <returns>Returns Budget details information.</returns>
	public static IQueryable<GetBudgetTransferInfo> MapToGetBudgetTransferInfo(this IQueryable<BudgetAggregate> query) =>
		query.Select(x => new GetBudgetTransferInfo
		{
			Name = x.Name,
			IconName = x.Icon,
			Description = x.Description ?? string.Empty,
			Currency = ((int)x.Limit.Currency).ToString(),
			Value = x.Limit.Value.ToString(),
			StartDate = x.Period.StartDate.ToString(),
			EndDate = x.Period.EndDate.ToString(),
		});
}