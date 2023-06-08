using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Budget Transaction of aggregate root.
/// </summary>
public class BudgetTransactionAggregate : Aggregate, IEntity<TransactionId>
{
	private BudgetTransactionAggregate()
	{
	}

	private BudgetTransactionAggregate(TransactionId id, BudgetId budgetId, TransactionType transactionType, string name, string email, decimal value, CategoryType categoryType, DateTime budgetTransactionDate, Status status)
	{
		var budgetTransactionCreated = new BudgetTransactionCreatedDomainEvent(id, budgetId, transactionType, name, email, value, categoryType, budgetTransactionDate, status);
		this.Apply(budgetTransactionCreated, this.Handle);
	}

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Reference to transaction ID.
	/// </summary>
	public TransactionId Id { get; private set; }

	/// <summary>
	/// Budget Transaction eg. income/Expense.
	/// </summary>
	public TransactionType TransactionType { get; private set; }

	/// <summary>
	/// Budget Transaction name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Budget Transaction creator email.
	/// </summary>
	public string Email { get; private set; } = default!;

	/// <summary>
	/// Value of new created income/Expense.
	/// </summary>
	public decimal Value { get; private set; }

	/// <summary>
	/// Transaction Category name.
	/// </summary>
	public CategoryType CategoryType { get; private set; }

	/// <summary>
	/// Budget Transaction creation date.
	/// </summary>
	public DateTime BudgetTransactionDate { get; private set; }

	/// <summary>
	/// Budget Transaction creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Status of transaction.
	/// </summary>
	public Status Status { get; private set; } = default;

	/// <summary>
	/// Create Budget Transaction.
	/// </summary>
	/// <param name="id">Transaction Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="transactionType">Enum of Income or Expense.</param>
	/// <param name="name">Name of income or Expense.</param>
	/// <param name="email">Transaction creator email.</param>
	/// <param name="value">Value of income or Expense.</param>
	/// <param name="categoryType">Category of Income or Expense.</param>
	/// <param name="budgetTransactionDate">Date of Creating Transaction.</param>
	/// <param name="status">Transaction status, default Active (optional).</param>
	/// <returns>New aggregate.</returns>
	public static BudgetTransactionAggregate Create(TransactionId id, BudgetId budgetId, TransactionType transactionType, string name, string email, decimal value, CategoryType categoryType, DateTime budgetTransactionDate, Status status = default)
	{
		return new BudgetTransactionAggregate(id, budgetId, transactionType, name, email, value, categoryType, budgetTransactionDate, status);
	}

	/// <summary>
	/// This method updates the "soft delete" flag for budget transactions.
	/// </summary>
	public void SoftRemove()
	{
		this.CheckRule(new StatusDeletedCannotBeSetTwiceBusinessRule(this.Status));

		var evt = new BudgetTransactionSoftDeletedDomainEvent(this.Id, Status.Deleted);

		this.Apply(evt, this.Handle);
	}

	/// <summary>
	/// This method updates the flag  to Cancelled for budget transactions.
	/// </summary>
	public void CancelTransaction()
	{
		this.CheckRule(new StatusCancelledCannotBeSetTwiceBusinessRule(this.Status));
		this.CheckRule(new StatusDeletedCannotBeSetTwiceBusinessRule(this.Status));

		var evt = new BudgetTransactionCancelledDomainEvent(this.Id, Status.Cancelled);

		this.Apply(evt, this.Handle);
	}

	private void Handle(BudgetTransactionCancelledDomainEvent @event)
	{
		this.Status = @event.Status;
	}

	private void Handle(BudgetTransactionSoftDeletedDomainEvent @event)
	{
		this.Status = @event.Status;
	}

	private void Handle(BudgetTransactionCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.BudgetId = @event.BudgetId;
		this.TransactionType = @event.TransactionType;
		this.Name = @event.Name;
		this.Email = @event.Email;
		this.Value = @event.Value;
		this.CategoryType = @event.CategoryType;
		this.BudgetTransactionDate = @event.BudgetTransactionDate;
		this.CreatedOn = @event.Timestamp;
		this.Status = @event.Status;
	}
}