using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget Transaction created domain event.
/// </summary>
public class BudgetTransactionCreatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionCreatedDomainEvent"/> class.
	/// </summary>
	/// <param name="transactionId">Budget Transaction Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="transactionType">Enum of Income or Expense.</param>
	/// <param name="name">Name of income or Expense.</param>
	/// <param name="value">Value of income or Expense.</param>
	/// <param name="categoryType">Enum of income/Expense Categories.</param>
	/// <param name="transactionDate">Creation of new income or Expense date.</param>
	public BudgetTransactionCreatedDomainEvent(TransactionId transactionId, BudgetId budgetId, TransactionType transactionType, string name, decimal value, CategoryType categoryType, DateTime transactionDate)
	{
		this.Id = transactionId;
		this.Name = name;
		this.Value = value;
		this.CategoryType = categoryType;
		this.BudgetId = budgetId;
		this.TransactionType = transactionType;
		this.BudgetTransactionDate = transactionDate;
	}

	/// <summary>
	/// Budget Transaction identifier.
	/// </summary>
	public TransactionId Id { get; private set; }

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Budget Transaction name.
	/// </summary>
	public string Name { get; private set; }

	/// <summary>
	/// Budget Transaction eg. income/Expense.
	/// </summary>
	public TransactionType TransactionType { get; set; }

	/// <summary>
	/// Category eg. "Home spendings," "Subscriptions," "Car," "Grocery".
	/// </summary>
	public CategoryType CategoryType { get; set; }

	/// <summary>
	/// Value of new created income/Expense.
	/// </summary>
	public decimal Value { get; set; }

	/// <summary>
	/// Budget Transaction creation date.
	/// </summary>
	public DateTime BudgetTransactionDate { get; set; }
}