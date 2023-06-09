using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Represents a domain event that is raised when a transaction category is deleted.
/// </summary>
public class TransactionCategoryDeletedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TransactionCategoryDeletedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">The identifier of the transaction category.</param>
	public TransactionCategoryDeletedDomainEvent(TransactionCategoryId id)
	{
		this.Id = id;
	}

	/// <summary>
	/// Gets the identifier of the transaction category.
	/// </summary>
	public TransactionCategoryId Id { get; }
}