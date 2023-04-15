using System.Text.Json;

using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Budget aggregate repository.
/// </summary>
public class TransactionRepository : ITransactionRepository
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IEventDispatcher<IEvent> domainEventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="TransactionRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context.</param>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public TransactionRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
	{
		this.budgetDbContext = budgetDbContext;
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves Budget aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<TransactionAggregate> GetById(Guid id)
		=> this.budgetDbContext.Transaction.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="transaction">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(TransactionAggregate transaction)
	{
		await this.domainEventDispatcher.Publish(transaction.UncommittedEvents);
		this.HandleEvents(transaction.UncommittedEvents);
		this.budgetDbContext.Transaction.Add(transaction);
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
	}
}