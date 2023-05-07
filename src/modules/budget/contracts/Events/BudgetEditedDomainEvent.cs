using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget edited domain event.
/// </summary>
public class BudgetEditedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetEditedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Budget id.</param>
	/// <param name="name">Budget name.</param>
	/// <param name="limit">Budget Limit.</param>
	/// <param name="period">Budget Duration.</param>
	/// <param name="icon">Budget Icon.</param>
	/// <param name="description">Budget Describtion.</param>
	public BudgetEditedDomainEvent(BudgetId id, string name, Money limit, Period period, string description, string icon)
	{
		this.Id = id;
		this.Name = name;
		this.Limit = limit;
		this.Period = period;
		this.Description = description;
		this.Icon = icon;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId Id { get; private set; }

	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Budget limit.
	/// </summary>
	public Money Limit { get; private set; } = default!;

	/// <summary>
	/// Budget time span.
	/// </summary>
	public Period Period { get; private set; } = default!;

	/// <summary>
	/// Budget icon.
	/// </summary>
	public string Icon { get; private set; }

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; private set; }
}