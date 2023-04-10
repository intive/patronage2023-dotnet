﻿namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

using Intive.Patronage2023.Shared.Infrastructure.Events;

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
	public BudgetNameUpdatedDomainEvent(Guid id, string newName)
	{
		this.Id = id;
		this.NewName = newName;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public Guid Id { get; }

	/// <summary>
	/// New Budget name.
	/// </summary>
	public string NewName { get; }
}