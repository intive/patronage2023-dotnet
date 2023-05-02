using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Transaction soft delete domain event.
/// </summary>
public class BudgetTransactionSoftDeleteDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionSoftDeleteDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="status">Soft Delete Status.</param>
	public BudgetTransactionSoftDeleteDomainEvent(TransactionId id, Status status)
	{
		this.Id = id;
		this.Status = status;
	}

	/// <summary>
	/// Budget transaction identifier.
	/// </summary>
	public TransactionId Id { get; }

	/// <summary>
	/// Soft Delete Flag.
	/// </summary>
	public Status Status { get; }
}