using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// UserBudget isFavourite updated domain event.
/// </summary>
public class UserBudgetFavouriteUpdatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserBudgetFavouriteUpdatedDomainEvent"/> class.
	/// </summary>
	/// <param name="isNewBudgetFavourite">New IsFavourite value.</param>
	public UserBudgetFavouriteUpdatedDomainEvent(bool isNewBudgetFavourite)
	{
		this.IsNewBudgetFavourite = isNewBudgetFavourite;
	}

	/// <summary>
	/// New UserBudget isFavourite value.
	/// </summary>
	public bool IsNewBudgetFavourite { get; set; }
}