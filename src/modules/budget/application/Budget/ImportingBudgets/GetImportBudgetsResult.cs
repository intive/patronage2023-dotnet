using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// GetImportBudgetsResult command.
/// </summary>
public record GetImportBudgetsResult(BudgetAggregateList BudgetAggregateList, ImportResult ImportResult);