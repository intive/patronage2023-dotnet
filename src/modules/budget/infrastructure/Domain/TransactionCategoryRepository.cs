using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Transaction Categories Repository.
/// </summary>
public class TransactionCategoryRepository : BaseRepository<TransactionCategoryAggregate, TransactionCategoryId>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="TransactionCategoryRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context.</param>
	/// <param name="eventDispatcher">Event dispatcher.</param>
	public TransactionCategoryRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> eventDispatcher)
		: base(budgetDbContext, eventDispatcher)
	{
	}
}