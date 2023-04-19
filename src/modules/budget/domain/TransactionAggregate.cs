using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Transaction of aggregate root.
/// </summary>
public class TransactionAggregate : Aggregate
{
	private TransactionAggregate(TransactionId transactionId, BudgetId budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn)
	{
		var transactionCreated = new TransactionCreatedDomainEvent(transactionId, budgetId.Value, transactionType, name, value, categoryType, createdOn);
		this.Apply(transactionCreated, this.Handle);
	}

	/// <summary>
	/// Reference to Transaction ID.
	/// </summary>
	public TransactionId TransactionId { get; private set; }

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Transaction eg. income/expanse.
	/// </summary>
	public TransactionTypes TransactionType { get; set; }

	/// <summary>
	/// Transaction name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Value of new created income/expanse.
	/// </summary>
	public decimal Value { get; set; }

	/// <summary>
	/// Category eg. "Home spendings," "Subscriptions," "Car," "Grocery".
	/// </summary>
	public Categories CategoryType { get; set; }

	/// <summary>
	/// Transaction creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Create Transaction.
	/// </summary>
	/// <param name="transactionId">Transaction Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="transactionType">Enum of Income or Expanse.</param>
	/// <param name="name">Name of income or expanse.</param>
	/// <param name="value">Value of income or expanse.</param>
	/// <param name="categoryType">Enum of income/expanse Categories.</param>
	/// <param name="createdOn">Creation of new income or expanse date.</param>
	/// <returns>New aggregate.</returns>
	public static TransactionAggregate Create(TransactionId transactionId, BudgetId budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn)
	{
		return new TransactionAggregate(transactionId, budgetId, transactionType, name, value, categoryType, createdOn);
	}

	private void Handle(TransactionCreatedDomainEvent @event)
	{
		this.TransactionId = @event.TransactionId;
		this.BudgetId = @event.BudgetId;
		this.TransactionType = @event.TransactionType;
		this.Name = @event.Name;
		this.Value = @event.Value;
		this.CategoryType = @event.CategoryType;
		this.CreatedOn = @event.CreatedOn;
	}
}