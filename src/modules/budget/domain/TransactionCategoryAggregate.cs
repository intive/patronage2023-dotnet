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

	private TransactionCategoryAggregate(TransactionCategoryId id, string? icon, string? name)
	{
		var transactionCategoryAggregate = new TransactionCategoryAddedDomainEvent(id, icon, name);
		this.Apply(transactionCategoryAggregate, this.Handle);
	}

	/// <summary>
	/// Gets the unique identifier of the transaction category.
	/// </summary>
	public TransactionCategoryId Id { get; private set; }

	/// <summary>
	/// Gets the icon associated with the transaction category.
	/// </summary>
	public string? Icon { get; private set; }

	/// <summary>
	/// Gets the name of the transaction category.
	/// </summary>
	public string? Name { get; private set; }

	/// <summary>
	/// Creates a new instance of the TransactionCategoryAggregate class.
	/// </summary>
	/// <param name="id">The identifier of the transaction category.</param>
	/// <param name="icon">The icon associated with the transaction category.</param>
	/// <param name="name">The name of the transaction category.</param>
	/// <returns>A new instance of the TransactionCategoryAggregate class.</returns>
	public TransactionCategoryAggregate Create(TransactionCategoryId id, string? icon, string? name)
	{
		return new TransactionCategoryAggregate(id, icon, name);
	}

	private void Handle(TransactionCategoryAddedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Icon = @event.Icon;
		this.Name = @event.Name;
	}
}