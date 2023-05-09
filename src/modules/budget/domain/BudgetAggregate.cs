using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget of aggregate root.
/// </summary>
public class BudgetAggregate : Aggregate, IEntity<BudgetId>
{
	// For Entity
	private BudgetAggregate()
	{
	}

	private BudgetAggregate(BudgetId id, string name, UserId userId, Money limit, Period period, string description, string icon)
	{
		if (id.Value == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var budgetCreated = new BudgetCreatedDomainEvent(id, name, userId, limit, period, description, icon);
		this.Apply(budgetCreated, this.Handle);
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
	/// Budget owner user Id.
	/// </summary>
	public UserId UserId { get; private set; }

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
	public string Icon { get; private set; } = default!;

	/// <summary>
	/// Budget describtion.
	/// </summary>
	public string? Description { get; private set; }

	/// <summary>
	/// Budget creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Status of budget.
	/// </summary>
	public Status Status { get; private set; } = default;

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
	public static BudgetAggregate Create(BudgetId id, string name, UserId userId, Money limit, Period period, string description, string icon)
	{
		return new BudgetAggregate(id, name, userId, limit, period, description, icon);
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
	/// Update Flag.
	/// </summary>
	public void SoftRemove()
	{
		this.CheckRule(new StatusDeletedCannotBeSetTwiceBusinessRule(Status.Deleted));

		var evt = new BudgetSoftDeletedDomainEvent(this.Id, Status.Deleted);

		this.Apply(evt, this.Handle);
	}

	/// <summary>
	/// Edit budget.
	/// </summary>
	/// <param name="id">Budget id.</param>
	/// <param name="name">Budget name.</param>
	/// <param name="limit">Budget Limit.</param>
	/// <param name="period">Budget Duration.</param>
	/// <param name="description">Budget Describtion.</param>
	/// <param name="icon">Budget Icon.</param>
	public void EditBudget(BudgetId id, string name, Money limit, Period period, string description, string icon)
	{
		var budgetEdited = new BudgetEditedDomainEvent(id, name, limit, period, description, icon);
		this.Apply(budgetEdited, this.Handle);
	}

	private void Handle(BudgetNameUpdatedDomainEvent @event)
	{
		this.Name = @event.NewName;
	}

	private void Handle(BudgetSoftDeletedDomainEvent @event)
	{
		this.Status = @event.Status;
	}

	private void Handle(BudgetCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
		this.UserId = @event.UserId;
		this.Limit = @event.Limit;
		this.Period = @event.Period;
		this.Description = @event.Description;
		this.Icon = @event.Icon;
		this.CreatedOn = @event.Timestamp;
	}

	private void Handle(BudgetEditedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
		this.Limit = @event.Limit;
		this.Period = @event.Period;
		this.Description = @event.Description;
		this.Icon = @event.Icon;
		this.CreatedOn = @event.Timestamp;
	}
}