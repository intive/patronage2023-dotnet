using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Interface of repository for BudgetTransactionAggregate.
/// </summary>
public interface IBudgetTransactionRepository : IRepository<BudgetTransactionAggregate, TransactionId>
{
}