using System.Text.Json;

using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

/// <summary>
/// Example aggregate repository.
/// </summary>
public class ExampleRepository : IExampleRepository
{
	private readonly ExampleDbContext exampleDbContext;
	private readonly IEventDispatcher<IEvent> domainEventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="exampleDbContext">Database context.</param>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public ExampleRepository(ExampleDbContext exampleDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
	{
		this.exampleDbContext = exampleDbContext;
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves example aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<ExampleAggregate> GetById(Guid id)
		=> this.exampleDbContext.Example.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(ExampleAggregate example)
	{
		await this.domainEventDispatcher.Publish(example.UncommittedEvents);
		this.HandleEvents(example.UncommittedEvents);
		this.exampleDbContext.Example.Add(example);
		await this.exampleDbContext.SaveChangesAsync();
	}

	/// <summary>
	/// Updates aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Update(ExampleAggregate example)
	{
		await this.domainEventDispatcher.Publish(example.UncommittedEvents);
		this.HandleEvents(example.UncommittedEvents);
		this.exampleDbContext.Example.Update(example);
		await this.exampleDbContext.SaveChangesAsync();
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
			this.exampleDbContext.DomainEventStore.Add(newEvent);
		}
	}
}