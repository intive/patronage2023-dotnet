using Intive.Patronage2023.Modules.Budget.Contracts.Provider;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// Represents information about transaction categories.
/// </summary>
public record TransactionCategoriesInfo(List<TransactionCategory>? Categories);