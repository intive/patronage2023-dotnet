namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// Budget information.
/// </summary>
/// <param name="Id">Budget ID.</param>
/// <param name="Name">Name of Budget.</param>
/// <param name="CreatedOn">Created Date.</param>
public record BudgetInfo(Guid Id, string Name, DateTime CreatedOn);