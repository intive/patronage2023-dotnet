using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// User budget aggregate repository.
/// </summary>
internal class UserBudgetRepository : BaseRepository<UserBudgetAggregate, Guid>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserBudgetRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context.</param>
	/// <param name="eventDispatcher">Event dispatcher.</param>
	public UserBudgetRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> eventDispatcher)
		: base(budgetDbContext, eventDispatcher)
	{
	}
}