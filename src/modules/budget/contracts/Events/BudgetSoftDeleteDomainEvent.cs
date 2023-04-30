using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Soft Delete domain event.
/// </summary>
public class BudgetSoftDeleteDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetSoftDeleteDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="isDeleted">Soft Delete Flag.</param>
	public BudgetSoftDeleteDomainEvent(BudgetId id, bool isDeleted)
	{
		this.Id = id;
		this.IsDeleted = isDeleted;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; }

	/// <summary>
	/// Soft Delete Flag.
	/// </summary>
	public bool IsDeleted { get; }
}