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
	public static GetBudgetsToExportInfo Map(BudgetAggregate entity) =>
		new GetBudgetsToExportInfo(
			entity.Name,
			entity.CreatedOn.ToString(),
			entity.Icon,
			string.IsNullOrEmpty(entity.Description) ? string.Empty : entity.Description,
			entity.Limit.Value.ToString(),
			entity.Limit.Currency.ToString(),
			entity.Period.StartDate.ToString(),
			entity.Period.EndDate.ToString());
}