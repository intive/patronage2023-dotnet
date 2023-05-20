using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
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
	/// <param name="entity">Entity to be mapped.</param>
	/// <returns>Returns Budget details information.</returns>
	public static GetBudgetTransferInfo Map(BudgetAggregate entity) =>
		new GetBudgetTransferInfo
		{
			Name = entity.Name,
			IconName = entity.Icon,
			Description = string.IsNullOrEmpty(entity.Description) ? string.Empty : entity.Description,
			Currency = entity.Limit.Currency.ToString(),
			Value = entity.Limit.Value.ToString(),
			StartDate = entity.Period.StartDate.ToString(),
			EndDate = entity.Period.EndDate.ToString(),
		};
}