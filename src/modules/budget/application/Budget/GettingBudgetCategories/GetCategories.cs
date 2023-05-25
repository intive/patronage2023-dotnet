using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetCategories;

/// <summary>
/// Represents a request to retrieve categories for a specific budget.
/// </summary>
/// <param name="BudgetId">The ID of the budget for which to retrieve the categories.</param>
public record GetCategories(BudgetId BudgetId) : IQuery<BudgetCategoriesInfo>;