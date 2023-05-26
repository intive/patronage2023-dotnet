using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.ImportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;

/// <summary>
/// This is a static class named BudgetAggregateGetBudgetsNameInfoMapper with
/// a single public static method named Map that takes a nullable list of strings
/// as its parameter and returns a GetBudgetsNameInfo object.
/// </summary>
public static class BudgetAggregateGetBudgetsNameInfoMapper
{
	/// <summary>
	/// The Map method initializes a new GetBudgetsNameInfo object and sets its BudgetName property to the provided entity parameter.
	/// If the entity parameter is null, the BudgetName property is also set to null.
	/// </summary>
	/// <param name="entity">A nullable List of strings that represents the names of budgets.</param>
	/// <returns>A new instance of the GetBudgetsNameInfo class with the BudgetName property set to the value of the entity parameter.</returns>
	public static GetBudgetsNameInfo Map(List<string>? entity) =>
		new()
		{
			BudgetName = entity,
		};
}