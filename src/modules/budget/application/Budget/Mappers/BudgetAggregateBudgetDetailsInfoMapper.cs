using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Extensions;

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
	public static BudgetDetailsInfo Map(BudgetAggregate entity) =>
		new()
		{
			Id = entity.Id,
			Name = entity.Name,
			UserId = entity.UserId,
			Limit = entity.Limit.Value,
			Currency = entity.Limit.Currency.ToString(),
			StartDate = entity.Period.StartDate.ToTimestamp(),
			EndDate = entity.Period.EndDate.ToTimestamp(),
			Icon = entity.Icon,
			Description = entity.Description,
		};
}