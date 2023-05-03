using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Soft Delete domain event.
/// </summary>
public class BudgetSoftDeletedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetSoftDeletedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="status">Soft Delete Status.</param>
	public BudgetSoftDeletedDomainEvent(BudgetId id, Status status)
	{
		this.Id = id;
		this.Status = status;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; }

	/// <summary>
	/// Soft Delete Status.
	/// </summary>
	public Status Status { get; }
}