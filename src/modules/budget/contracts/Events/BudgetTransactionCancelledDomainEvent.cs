using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Transaction Cancelled domain event.
/// </summary>
public class BudgetTransactionCancelledDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionCancelledDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="status">Soft Cancelled Status.</param>
	public BudgetTransactionCancelledDomainEvent(TransactionId id, Status status)
	{
		this.Id = id;
		this.Status = status;
	}

	/// <summary>
	/// Budget transaction identifier.
	/// </summary>
	public TransactionId Id { get; }

	/// <summary>
	/// Status which mean to be changed to Cancelled.
	/// </summary>
	public Status Status { get; }
}