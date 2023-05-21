using Intive.Patronage2023.Modules.Budget.Application.Data;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
/// GetImportResult command.
/// </summary>
public record GetImportResult(BudgetAggregateList BudgetAggregateList, ImportResult ImportResult);