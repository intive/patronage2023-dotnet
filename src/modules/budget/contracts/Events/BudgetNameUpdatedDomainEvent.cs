using Intive.Patronage2023.Shared.Infrastructure.Events;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget name updated domain event.
/// </summary>
public class BudgetNameUpdatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetNameUpdatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="newName">New name.</param>
	public BudgetNameUpdatedDomainEvent(BudgetId id, string newName)
	{
		this.Id = id;
		this.NewName = newName;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; }

	/// <summary>
	/// New Budget name.
	/// </summary>
	public string NewName { get; }
}