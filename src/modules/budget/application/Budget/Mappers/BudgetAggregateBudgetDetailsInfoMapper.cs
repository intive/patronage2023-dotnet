using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// Mapper class.
/// </summary>
public static class BudgetAggregateBudgetDetailsInfoMapper
{
	/// <summary>
	/// Mapping method.
	/// </summary>
	/// <param name="entity">Entity to be mapped.</param>
	/// <returns>Returns <ref name="BudgetDetailsInfo"/>Budget details information.</returns>
	public static BudgetDetailsInfo MapToDetailsInfo(this BudgetAggregate entity) =>
		new BudgetDetailsInfo
		{
			Id = entity.Id.Value,
			Name = entity.Name,
			UserId = entity.UserId.Value,
			Limit = entity.Limit.Value,
			Currency = entity.Limit.Currency.ToString(),
			StartDate = entity.Period.StartDate.ToISO8601(),
			EndDate = entity.Period.EndDate.ToISO8601(),
			Icon = entity.Icon,
			Description = entity.Description,
		};
}