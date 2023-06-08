#pragma warning disable IDE0005

using Intive.Patronage2023.Modules.Budget.Contracts.Events;

#pragma warning restore IDE0005

using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Domain;

/// <summary>
/// This class represents the relationship between a user and a budget in the context of the application.
/// </summary>
public class UserBudgetAggregate : Aggregate, IEntity<Guid>
{
	private UserBudgetAggregate()
	{
	}

	private UserBudgetAggregate(Guid id, UserId userId, BudgetId budgetId, UserRole userRole, bool isFavourite)
	{
		if (id == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var userBudgetCreated = new UserBudgetAddedDomainEvent(id, userId, budgetId, userRole, isFavourite);
		this.Apply(userBudgetCreated, this.Handle);
	}

	/// <summary>
	///  The unique identifier of the UserBudgetAggregate object.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	/// The "UserId" property is a UserId object that identifies the user.
	/// </summary>
	public UserId UserId { get; private set; }

	/// <summary>
	/// The "BudgetId" property is a BudgetId object that identifies the budget.
	/// </summary>
	public BudgetId BudgetId { get; private set; }

	/// <summary>
	/// The "Type" property is a UserRole enum that specifies the user's role in relation to the budget.
	/// </summary>
	public UserRole UserRole { get; private set; }

	/// <summary>
	/// IsFavourite flag added to budget by user.
	/// </summary>
	public bool IsFavourite { get; private set; }

	/// <summary>
	/// A static factory method that creates a new instance of the UserBudgetAggregate object with the specified parameters.
	/// </summary>
	/// <param name="id">Id.</param>
	/// <param name="userId">UserId.</param>
	/// <param name="budgetId">BudgetId.</param>
	/// <param name="userRole">User Role.</param>
	/// <returns>New UserBudgetAggregate Object.</returns>
	public static UserBudgetAggregate Create(Guid id, UserId userId, BudgetId budgetId, UserRole userRole)
	{
		bool isFavourite = false;
		return new UserBudgetAggregate(id, userId, budgetId, userRole, isFavourite);
	}

	/// <summary>
	/// Update UserBudget favourite flag.
	/// </summary>
	/// <param name="isFavourite">Favourite flag.</param>
	public void UpdateFavourite(bool isFavourite)
	{
		var evt = new UserBudgetFavouriteUpdatedDomainEvent(isFavourite);

		this.Apply(evt, this.Handle);
	}

	/// <summary>
	/// Delete UserBudget.
	/// </summary>
	public void Delete()
	{
		var evt = new UserBudgetDeletedDomainEvent(this.Id);

		this.Apply(evt, this.Handle);
	}

	private void Handle(UserBudgetAddedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.UserId = @event.UserId;
		this.BudgetId = @event.BudgetId;
		this.UserRole = @event.UserRole;
	}

	private void Handle(UserBudgetFavouriteUpdatedDomainEvent @event)
	{
		this.IsFavourite = @event.IsFavourite;
	}

	private void Handle(UserBudgetDeletedDomainEvent @event)
	{
		this.Id = @event.Id;
	}
}