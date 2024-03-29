using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// Represents an aggregate object for a transaction category.
/// </summary>
public class TransactionCategoryAggregate : Aggregate, IEntity<TransactionCategoryId>
{
	private TransactionCategoryAggregate()
	{
	}

	private TransactionCategoryAggregate(TransactionCategoryId id, BudgetId budgetId, Icon icon, CategoryType categoryType)
	{
		var transactionCategoryAggregate = new TransactionCategoryAddedDomainEvent(id, budgetId, icon, categoryType);
		this.Apply(transactionCategoryAggregate, this.Handle);
	}

	/// <summary>
	/// Gets the unique identifier of the transaction category.
	/// </summary>
	public TransactionCategoryId Id { get; private set; }

	/// <summary>
	/// Gets the unique identifier of the budget id.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Gets the icon associated with the transaction category.
	/// </summary>
	public Icon Icon { get; private set; } = default!;

	/// <summary>
	/// Gets the name of the transaction category.
	/// </summary>
	public CategoryType CategoryType { get; private set; } = default!;

	/// <summary>
	/// Creates a new instance of the TransactionCategoryAggregate class.
	/// </summary>
	/// <param name="id">The identifier of the transaction category.</param>
	/// <param name="budgetId">The Budget identifier to which we add a transaction category.</param>
	/// <param name="icon">The icon associated with the transaction category.</param>
	/// <param name="categoryType">The name of the transaction category.</param>
	/// <returns>A new instance of the TransactionCategoryAggregate class.</returns>
	public static TransactionCategoryAggregate Create(TransactionCategoryId id, BudgetId budgetId, Icon icon, CategoryType categoryType)
	{
		return new TransactionCategoryAggregate(id, budgetId, icon, categoryType);
	}

	/// <summary>
	/// Deletes the transaction category.
	/// </summary>
	public void DeleteCategory()
	{
		var evt = new TransactionCategoryDeletedDomainEvent(this.Id);
		this.Apply(evt, this.Handle);
	}

	private void Handle(TransactionCategoryAddedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.BudgetId = @event.BudgetId;
		this.Icon = @event.Icon;
		this.CategoryType = @event.CategoryType;
	}

	private void Handle(TransactionCategoryDeletedDomainEvent @event)
	{
		this.Id = @event.Id;
	}
}