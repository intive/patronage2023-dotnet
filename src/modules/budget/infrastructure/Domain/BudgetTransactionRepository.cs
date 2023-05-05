using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Budget Transaction aggregate repository.
/// </summary>
public class BudgetTransactionRepository : BaseRepository<BudgetTransactionAggregate, TransactionId>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context.</param>
	/// <param name="eventDispatcher">Event dispatcher.</param>
	public BudgetTransactionRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> eventDispatcher)
			: base(budgetDbContext, eventDispatcher)
	{
	}
}