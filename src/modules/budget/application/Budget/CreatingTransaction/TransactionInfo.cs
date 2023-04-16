using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;

/// <summary>
/// Model of Income and Expanse.
/// </summary>
/// <param name="TransactionType">Enum of Income or Expanse.</param>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="Name">Name of income or expanse.</param>
/// <param name="Value">Value of income or expanse.</param>
/// <param name="CreatedOn">Creation of new income or expanse date.</param>
/// <param name="CategoryType">Enum of income/expanse Categories.</param>
public record TransactionInfo(TransactionTypes TransactionType, Guid BudgetId, string Name, decimal Value,
	DateTime CreatedOn, Categories CategoryType);