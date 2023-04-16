using System.ComponentModel;
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Transaction of aggregate root.
/// </summary>
public class TransactionAggregate : Aggregate
{
	private TransactionAggregate(Guid id, Guid budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var transactionCreated = new TransactionCreatedDomainEvent(id, budgetId, transactionType, name, value, categoryType, createdOn);
		this.Apply(transactionCreated, this.Handle);
	}

	/// <summary>
	/// Transaction identifier.
	/// </summary>
	/// [DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afb6")]
	public Guid Id { get; private set; }

	/// <summary>
	/// Reference to budget ID.
	/// </summary>
	[DefaultValue("3fa85f64-5717-4562-b3fc-2c963f66afa6")]
	public Guid BudgetId { get; private set; }

	/// <summary>
	/// Transaction eg. income/expanse.
	/// </summary>
	public TransactionTypes TransactionType { get; set; }

	/// <summary>
	/// Transaction name.
	/// </summary>
	[DefaultValue("Type your Income/Expanse name.")]
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Value of new created income/expanse.
	/// </summary>
	[DefaultValue(typeof(decimal), "1")]
	public decimal Value { get; set; }

	/// <summary>
	/// Category eg. "Home spendings," "Subscriptions," "Car," "Grocery".
	/// </summary>
	public Categories CategoryType { get; set; }

	/// <summary>
	/// Transaction creation date.
	/// </summary>
	[DefaultValue(typeof(DateTime), "now")]
	public DateTime CreatedOn { get; private set; }

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
	/// <returns>New aggregate.</returns>
	public static TransactionAggregate Create(Guid id, Guid budgetId, TransactionTypes transactionType, string name, decimal value, Categories categoryType, DateTime createdOn)
	{
		return new TransactionAggregate(id, budgetId, transactionType, name, value, categoryType, createdOn);
	}

	private void Handle(TransactionCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.BudgetId = @event.BudgetId;
		this.TransactionType = @event.TransactionType;
		this.Name = @event.Name;
		this.Value = @event.Value;
		this.CategoryType = @event.CategoryType;
		this.CreatedOn = @event.CreatedOn;
	}
}