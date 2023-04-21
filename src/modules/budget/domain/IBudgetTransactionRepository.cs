using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Intive.Patronage2023.Modules.Budget.Contracts;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Interface of repository for BudgetTransactionAggregate.
/// </summary>
public interface IBudgetTransactionRepository : IRepository<BudgetTransactionAggregate, TransactionId>
{
}