using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

/// <summary>
/// record which represents Budget Id.
/// </summary>
public record struct BudgetId(Guid Value) : IEntityId<Guid>;