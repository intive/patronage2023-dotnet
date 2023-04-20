using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget of aggregate root.
/// </summary>
public class BudgetAggregate : Aggregate
{
	private BudgetAggregate(BudgetId budgetId, string name)
	{
		if (budgetId.Value == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var budgetCreated = new BudgetCreatedDomainEvent(budgetId, name);
		this.Apply(budgetCreated, this.Handle);
	}

	/// <summary>
	/// Budget identifier.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Budget name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Budget creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Create Budget.
	/// </summary>
	/// <param name="budgetId">Unique identifier.</param>
	/// <param name="name">Budget name.</param>
	/// <returns>New aggregate.</returns>
	public static BudgetAggregate Create(BudgetId budgetId, string name)
	{
		return new BudgetAggregate(budgetId, name);
	}

	/// <summary>
	/// Update Budget name.
	/// </summary>
	/// <param name="name">New name.</param>
	public void UpdateName(string name)
	{
		this.CheckRule(new SuperImportantBudgetBusinessRule(name));

		var evt = new BudgetNameUpdatedDomainEvent(this.BudgetId, name);

		this.Apply(evt, this.Handle);
	}

	private void Handle(BudgetNameUpdatedDomainEvent @event)
	{
		this.Name = @event.NewName;
	}

	private void Handle(BudgetCreatedDomainEvent @event)
	{
		this.BudgetId = @event.BudgetId;
		this.Name = @event.Name;
		this.CreatedOn = @event.Timestamp;
	}
}