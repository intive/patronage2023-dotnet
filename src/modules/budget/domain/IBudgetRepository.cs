using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Interface of repository for BudgetAggregate.
/// </summary>
public interface IBudgetRepository : IRepository<BudgetAggregate, BudgetId>
{
}