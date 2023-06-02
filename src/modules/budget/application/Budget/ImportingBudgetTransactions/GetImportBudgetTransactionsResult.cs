using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;

/// <summary>
/// GetImportBudgetTransactionsResult command.
/// </summary>
public record GetImportBudgetTransactionsResult(BudgetTransactionAggregateList BudgetTransactionAggregateList, ImportResult ImportResult);