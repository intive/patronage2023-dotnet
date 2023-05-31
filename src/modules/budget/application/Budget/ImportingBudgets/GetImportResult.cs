namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// GetImportResult command.
/// </summary>
public record GetImportResult(BudgetAggregateList BudgetAggregateList, ImportResult ImportResult);