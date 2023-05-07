#pragma warning disable IDE0005
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
#pragma warning restore IDE0005
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// This class represents the relationship between a user and a budget in the context of the application.
/// </summary>
public class UserBudgetAggregate : Aggregate, IEntity<Guid>
{
	private UserBudgetAggregate()
	{
	}

	private UserBudgetAggregate(Guid id, UserId userId, BudgetId budgetId, UserRole userRole)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var userBudgetCreated = new UserBudgetAddedDomainEvent(id, userId, budgetId, userRole);
		this.Apply(userBudgetCreated, this.Handle);
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

	/// <summary>
	/// Create New UserBudgetAggregate Object.
	/// </summary>
	/// <param name="id">Id.</param>
	/// <param name="userId">UserId.</param>
	/// <param name="budgetId">BudgetId.</param>
	/// <param name="userRole">User Role.</param>
	/// <returns>New UserBudgetAggregate Object.</returns>
	public static UserBudgetAggregate Create(Guid id, UserId userId, BudgetId budgetId, UserRole userRole)
	{
		return new UserBudgetAggregate(id, userId, budgetId, userRole);
	}

	private void Handle(UserBudgetAddedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.UserId = @event.UserId;
		this.BudgetId = @event.BudgetId;
		this.UserRole = @event.UserRole;
	}
}