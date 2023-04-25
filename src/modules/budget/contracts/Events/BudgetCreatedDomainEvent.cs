using Intive.Patronage2023.Shared.Infrastructure.Events;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget created domain event.
/// </summary>
public class BudgetCreatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetCreatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget identifier.</param>
	/// <param name="name">Budget name.</param>
	public BudgetCreatedDomainEvent(BudgetId id, string name)
	{
		this.Id = id;
		this.Name = name;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; }

	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; }
}