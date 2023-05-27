using Intive.Patronage2023.Shared.Infrastructure;

namespace Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

/// <summary>
/// Represents the identifier of a transaction category.
/// </summary>
public record struct TransactionCategoryId : IEntityId<Guid>
{
	/// <summary>
	/// Value of Id.
	/// </summary>
	public Guid Value { get; }
}