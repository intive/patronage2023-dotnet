using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

/// <summary>
/// Represents the identifier of a transaction category.
/// </summary>
public record struct TransactionCategoryId(Guid Value) : IEntityId<Guid>;