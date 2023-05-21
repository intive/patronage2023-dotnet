using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// This is a static class named UserBudgetAggregateRoleInfoMapper with
/// a single public static method named Map that takes a nullable UserRole
/// object as its parameter and returns a UserBudgetRoleInfo object.
/// </summary>
public static class BudgetAggregateGetBudgetsNameInfoMapper
{
	/// <summary>
	/// The Map method initializes a new UserBudgetRoleInfo object and sets its UserRole property to the provided entity parameter.
	/// If the entity parameter is null, the UserRole property is also set to null.
	/// </summary>
	/// <param name="entity">A nullable UserRole object that represents the role of the user for a specific budget.</param>
	/// <returns>A new instance of the UserBudgetRoleInfo class with the UserRole property set to the value of the entity parameter.</returns>
	public static GetBudgetsNameInfo Map(List<string>? entity) =>
		new()
		{
			BudgetName = entity,
		};
}