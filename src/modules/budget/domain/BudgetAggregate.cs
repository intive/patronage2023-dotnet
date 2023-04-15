using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget of aggregate root.
/// </summary>
public class BudgetAggregate : Aggregate
{
	private BudgetAggregate(Guid id, string name, List<TransactionAggregate> transaction)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var budgetCreated = new BudgetCreatedDomainEvent(id, name);
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
	/// Budget creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// .
	/// </summary>
	public List<TransactionAggregate>? Transactions { get; set; }

	/// <summary>
	/// Create Budget.
	/// </summary>
	/// <param name="id">Unique identifier.</param>
	/// <param name="name">Budget name.</param>
	/// <param name="transaction">Transaction.</param>
	/// <returns>New aggregate.</returns>
	public static BudgetAggregate Create(Guid id, string name, List<TransactionAggregate> transaction)
	{
		return new BudgetAggregate(id, name, transaction);
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

	private void Handle(BudgetNameUpdatedDomainEvent @event)
	{
		this.Name = @event.NewName;
	}

	private void Handle(BudgetCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
		this.CreatedOn = @event.Timestamp;
	}
}