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
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public BudgetTransactionRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
	{
		this.budgetDbContext = budgetDbContext;
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves Budget Transaction aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<BudgetTransactionAggregate> GetById(TransactionId id)
		=> this.budgetDbContext.Transaction.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="budgetTransaction">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(BudgetTransactionAggregate budgetTransaction)
	{
		await this.domainEventDispatcher.Publish(budgetTransaction.UncommittedEvents);
		this.HandleEvents(budgetTransaction.UncommittedEvents);
		this.budgetDbContext.Transaction.Add(budgetTransaction);
		await this.budgetDbContext.SaveChangesAsync();
	}

	/// <summary>
	/// Update aggregate state.
	/// </summary>
	/// <param name="budgetTransaction">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Update(BudgetTransactionAggregate budgetTransaction)
	{
		await this.budgetDbContext.SaveChangesAsync();
	}

	private void HandleEvents(List<IEvent> uncommittedEvents)
	{
		foreach (var item in uncommittedEvents)
		{
			var newEvent = new DomainEventStore
			{
				CreatedAt = DateTimeOffset.UtcNow,
				Type = item.GetType().FullName,
				Data = JsonSerializer.Serialize(item),
			};
			this.budgetDbContext.DomainEventStore.Add(newEvent);
		}
	/// <param name="eventDispatcher">Event dispatcher.</param>
	public BudgetTransactionRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> eventDispatcher)
			: base(budgetDbContext, eventDispatcher)
	{
	}
}