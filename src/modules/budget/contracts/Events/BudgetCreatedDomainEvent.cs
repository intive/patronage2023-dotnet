using Intive.Patronage2023.Shared.Infrastructure.Domain.OwnedEntities;
using Intive.Patronage2023.Shared.Infrastructure.Events;

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
	/// <param name="userId">Budget owner user id.</param>
	/// <param name="limit">Budget Limit.</param>
	/// <param name="period">Budget Duration.</param>
	/// <param name="iconName">Budget Icon.</param>
	/// <param name="description">Budget Describtion.</param>
	public BudgetCreatedDomainEvent(Guid id, string name, Guid userId, BudgetLimit limit, BudgetPeriod period, string description, string iconName)
	{
		this.Id = id;
		this.Name = name;
		this.UserId = userId;
		this.Limit = limit;
		this.Period = period;
		this.Description = description;
		this.Icon = iconName;
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Budget owner user Id.
	/// </summary>
	public Guid UserId { get; private set; }

	/// <summary>
	/// Budget limit.
	/// </summary>
	public BudgetLimit Limit { get; private set; } = default!;

	/// <summary>
	/// Budget time span.
	/// </summary>
	public BudgetPeriod Period { get; private set; } = default!;

	/// <summary>
	/// Budget icon.
	/// </summary>
	public string? Icon { get; private set; }

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; private set; }
}