namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Budget aggregate repository.
/// </summary>
public class BudgetRepository : IBudgetRepository
{
	private readonly BudgetDbContext BudgetDbContext;
	private readonly IEventDispatcher<IEvent> domainEventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetRepository"/> class.
	/// </summary>
	/// <param name="BudgetDbContext">Database context.</param>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public BudgetRepository(BudgetDbContext BudgetDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
	{
		this.BudgetDbContext = BudgetDbContext;
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves Budget aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<BudgetAggregate> GetById(Guid id)
		=> this.BudgetDbContext.Budget.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="Budget">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(BudgetAggregate Budget)
	{
		await this.domainEventDispatcher.Publish(Budget.UncommittedEvents);
		this.BudgetDbContext.Budget.Add(Budget);
		await this.BudgetDbContext.SaveChangesAsync();
	}
}