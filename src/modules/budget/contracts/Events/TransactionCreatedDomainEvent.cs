using Intive.Patronage2023.Shared.Infrastructure.Events;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Transaction created domain event.
/// </summary>
public class TransactionCreatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TransactionCreatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Transaction Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="transactionType">Enum of Income or Expanse.</param>
	/// <param name="name">Name of income or expanse.</param>
	/// <param name="value">Value of income or expanse.</param>
	/// <param name="categoryType">Enum of income/expanse Categories.</param>
	/// <param name="createdOn">Creation of new income or expanse date.</param>
	public TransactionCreatedDomainEvent(Guid id, Guid budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn)
	{
		this.Id = id;
		this.Name = name;
		this.Value = value;
		this.CategoryType = categoryType;
		this.CreatedOn = createdOn;
		this.BudgetId = new BudgetId(budgetId);
		this.TransactionType = transactionType;
	}

	/// <summary>
	/// Transaction identifier.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Transaction name.
	/// </summary>
	public string Name { get; private set; }

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
}