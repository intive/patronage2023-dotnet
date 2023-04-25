using System.Text.Json;

using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Intive.Patronage2023.Modules.Budget.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Budget aggregate repository.
/// </summary>
public class BudgetRepository : IBudgetRepository
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IEventDispatcher<IEvent> domainEventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetRepository"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Database context.</param>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public BudgetRepository(BudgetDbContext budgetDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
	{
		this.budgetDbContext = budgetDbContext;
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves Budget aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<BudgetAggregate> GetById(BudgetId id)
		=> this.budgetDbContext.Budget.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="budget">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(BudgetAggregate budget)
	{
		await this.domainEventDispatcher.Publish(budget.UncommittedEvents);
		this.HandleEvents(budget.UncommittedEvents);
		this.budgetDbContext.Budget.Add(budget);
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