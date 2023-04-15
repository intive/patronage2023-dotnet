using Intive.Patronage2023.Modules.Budget.Domain.Helpers;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget of aggregate root.
/// </summary>
public class TransactionAggregate : Aggregate
{
	private TransactionAggregate(Guid id, Guid budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn, BudgetAggregate budgetAggregate)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		////var budgetCreated = new TransactionCreatedDomainEvent(id, budgetId, transactionType, name, value, categoryType, createdOn);
		////this.Apply(budgetCreated, this.Handle);
	}

	/// <summary>
	/// Transaction identifier.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	public Guid BudgetId { get; private set; }

	/// <summary>
	/// Transaction name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Transaction eg. income/expanse.
	/// </summary>
	public TransactionTypes TransactionType { get; set; }

	/// <summary>
	/// Category eg. "Home spendings," "Subscriptions," "Car," "Grocery".
	/// </summary>
	public Categories CategoryType { get; set; }

	/// <summary>
	/// Value of new created income/expanse.
	/// </summary>
	public decimal Value { get; set; }

	/// <summary>
	/// Transaction creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Budget Aggregate.
	/// </summary>
	public BudgetAggregate? BudgetAggregate { get; set; }

	/// <summary>
	/// Create Transaction.
	/// </summary>
	/// <param name="id">Transaction Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="transactionType">Enum of Income or Expanse.</param>
	/// <param name="name">Name of income or expanse.</param>
	/// <param name="value">Value of income or expanse.</param>
	/// <param name="categoryType">Enum of income/expanse Categories.</param>
	/// <param name="createdOn">Creation of new income or expanse date.</param>
	/// <param name="budgetAggregate">Budget Aggregate.</param>
	/// <returns>New aggregate.</returns>
	public static TransactionAggregate Create(Guid id, Guid budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn, BudgetAggregate budgetAggregate)
	{
		return new TransactionAggregate(id, budgetId, transactionType, name, value, categoryType, createdOn, budgetAggregate);
	}

	////private void Handle(TransactionCreatedDomainEvent @event)
	////{
	////	this.Id = @event.Id;
	////	this.Name = @event.Name;
	////	this.CreatedOn = @event.Timestamp;
	////}
}