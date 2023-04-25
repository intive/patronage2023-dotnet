using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Domain.Rules;

using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget of aggregate root.
/// </summary>
public class BudgetAggregate : Aggregate
{
	// For Entity
	private BudgetAggregate(Guid id, string name, Guid userId, string? icon, string? description, DateTime createdOn)
	{
		this.Id = id;
		this.Name = name;
		this.UserId = userId;
		this.Icon = icon;
		this.Description = description;
		this.CreatedOn = createdOn;
	}

	private BudgetAggregate(Guid id, string name, Guid userId, Money limit, Period period, string description, string icon)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var budgetCreated = new BudgetCreatedDomainEvent(id, name, userId, limit, period, description, icon);
		this.Apply(budgetCreated, this.Handle);
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
	public Money Limit { get; private set; } = default!;

	/// <summary>
	/// Budget time span.
	/// </summary>
	public Period Period { get; private set; } = default!;

	/// <summary>
	/// Budget icon.
	/// </summary>
	public string? Icon { get; private set; }

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Budget creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Create Budget.
	/// </summary>
	/// <param name="id">Unique identifier.</param>
	/// <param name="name">Budget name.</param>
	/// <param name="userId">Budget owner user id.</param>
	/// <param name="limit">Budget Limit.</param>
	/// <param name="period">Budget Duration.</param>
	/// <param name="description">Budget Description.</param>
	/// <param name="icon">Budget Icon.</param>
	/// <returns>New aggregate.</returns>
	public static BudgetAggregate Create(Guid id, string name, Guid userId, Money limit, Period period, string description, string icon)
	{
		return new BudgetAggregate(id, name, userId, limit, period, icon, description);
	}

	/// <summary>
	/// Update Budget name.
	/// </summary>
	/// <param name="name">New name.</param>
	public void UpdateName(string name)
	{
		this.CheckRule(new SuperImportantBudgetBusinessRule(name));

		var evt = new BudgetNameUpdatedDomainEvent(this.Id, name);

		this.Apply(evt, this.Handle);
	}

	/// <summary>
	/// Edit budget.
	/// </summary>
	/// <param name="id">Budget id.</param>
	/// <param name="name">Budget name.</param>
	/// <param name="userId">User id.</param>
	/// <param name="limit">Budget Limit.</param>
	/// <param name="period">Budget Duration.</param>
	/// <param name="description">Budget Describtion.</param>
	/// <param name="icon">Budget Icon.</param>
	public void EditBudget(Guid id, string name, Guid userId, Money limit, Period period, string description, string icon)
	{
		var budgetEdited = new BudgetEditedDomainEvent(id, name, userId, limit, period, description, icon);
		this.Apply(budgetEdited, this.Handle);
	}

	private void Handle(BudgetNameUpdatedDomainEvent @event)
	{
		this.Name = @event.NewName;
	}

	private void Handle(BudgetCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
		this.UserId = @event.UserId;
		this.Limit = @event.Limit;
		this.Period = @event.Period;
		this.Icon = @event.Icon;
		this.Description = @event.Description;
		this.CreatedOn = @event.Timestamp;
	}

	private void Handle(BudgetEditedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
		this.UserId = @event.UserId;
		this.Limit = @event.Limit;
		this.Period = @event.Period;
		this.Icon = @event.Icon;
		this.Description = @event.Description;
		this.CreatedOn = @event.Timestamp;
	}
}