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
	/// <returns>Returns <ref name="GetBudgetsToExportInfo"/>Budget details information.</returns>
	public static GetBudgetsToExportInfo Map(BudgetAggregate entity) =>
		new()
		{
			Name = entity.Name,
			Limit = entity.Limit.Value,
			Currency = entity.Limit.Currency.ToString(),
			StartDate = entity.Period.StartDate,
			EndDate = entity.Period.EndDate,
			Icon = entity.Icon,
			Description = entity.Description,
		};
}