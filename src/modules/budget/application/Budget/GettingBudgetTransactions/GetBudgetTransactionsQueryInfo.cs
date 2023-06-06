using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// Get Number of Records on Page and Page Index.
/// </summary>
/// <param name="PageSize">Max object returned in page.</param>
/// <param name="PageIndex">Page number.</param>
/// <param name="TransactionType">Filter transactions type. Income, Expense or null for all.</param>
/// <param name="CategoryTypes">Filter categories type. Empty or null for all.</param>
/// <param name="Search">Search text.</param>
public record GetBudgetTransactionsQueryInfo(
	int PageSize,
	int PageIndex,
	TransactionType? TransactionType,
	CategoryType[]? CategoryTypes,
	string Search);