using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// User Budget Domain Event.
/// </summary>
public class UserBudgetAddedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserBudgetAddedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Index id.</param>
	/// <param name="userId">User Id.</param>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="userRole">User Role.</param>
	public UserBudgetAddedDomainEvent(Guid id, UserId userId, BudgetId budgetId, UserRole userRole)
	{
		this.Id = id;
		this.UserId = userId;
		this.BudgetId = budgetId;
		this.UserRole = userRole;
	}

	/// <summary>
	/// Table Index.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// The "UserId" property is a UserId object that identifies the user.
	/// </summary>
	public UserId UserId { get; set; }

	/// <summary>
	/// The "BudgetId" property is a BudgetId object that identifies the budget.
	/// </summary>
	public BudgetId BudgetId { get; set; }

	/// <summary>
	/// The "Type" property is a UserRole enum that specifies the user's role in relation to the budget.
	/// </summary>
	public UserRole UserRole { get; set; }
}