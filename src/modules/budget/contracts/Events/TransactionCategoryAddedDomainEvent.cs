using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Represents a domain event that is raised when a transaction category is added.
/// </summary>
public class TransactionCategoryAddedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TransactionCategoryAddedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">The identifier of the transaction category.</param>
	/// <param name="budgetId">The identifier of budget.</param>
	/// <param name="icon">The icon associated with the transaction category.</param>
	/// <param name="name">The name of the transaction category.</param>
	public TransactionCategoryAddedDomainEvent(TransactionCategoryId id, BudgetId budgetId, Icon icon, string name)
	{
		this.Id = id;
		this.BudgetId = budgetId;
		this.Icon = icon;
		this.Name = name;
	}

	/// <summary>
	/// Gets the unique identifier of the transaction category.
	/// </summary>
	public TransactionCategoryId Id { get; private set; }

	/// <summary>
	/// Gets the unique identifier of the budget.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// Gets the icon associated with the transaction category.
	/// </summary>
	public Icon Icon { get; private set; }

	/// <summary>
	/// Gets the name of the transaction category.
	/// </summary>
	public string Name { get; private set; }
}