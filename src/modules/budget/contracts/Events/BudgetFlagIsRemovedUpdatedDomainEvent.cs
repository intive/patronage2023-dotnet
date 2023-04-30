using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget name updated domain event.
/// </summary>
public class BudgetFlagIsRemovedUpdatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetFlagIsRemovedUpdatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="isDeleted">New name.</param>
	public BudgetFlagIsRemovedUpdatedDomainEvent(BudgetId id, bool isDeleted)
	{
		this.Id = id;
		this.IsDeleted = isDeleted;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; }

	/// <summary>
	/// New Budget name.
	/// </summary>
	public bool IsDeleted { get; }
}