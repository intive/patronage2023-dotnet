using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// The UserBudgetRepository class is a repository for UserBudgetAggregate objects.
/// </summary>
internal class UserBudgetRepository : BaseRepository<UserBudgetAggregate, Guid>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserBudgetRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context used to persist the entities.</param>
	/// <param name="eventDispatcher">Is used to dispatch events after persistence operations have been completed.</param>
	public UserBudgetRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> eventDispatcher)
		: base(budgetDbContext, eventDispatcher)
	{
	}
}