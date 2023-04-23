using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Interface of repository for BudgetAggregate.
/// </summary>
public interface IBudgetRepository : IRepository<BudgetAggregate, Guid>
{
}