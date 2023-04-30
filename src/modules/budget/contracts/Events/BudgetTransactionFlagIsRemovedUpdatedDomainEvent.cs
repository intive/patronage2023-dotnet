using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Transaction soft delete domain event.
/// </summary>
public class BudgetTransactionFlagIsRemovedUpdatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionFlagIsRemovedUpdatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="isBudgetDeleted">New name.</param>
	public BudgetTransactionFlagIsRemovedUpdatedDomainEvent(TransactionId id, bool isBudgetDeleted)
	{
		this.Id = id;
		this.IsBudgetDeleted = isBudgetDeleted;
	}

	/// <summary>
	/// Budget transaction identifier.
	/// </summary>
	public TransactionId Id { get; }

	/// <summary>
	/// Soft Delete Flag.
	/// </summary>
	public bool IsBudgetDeleted { get; }
}