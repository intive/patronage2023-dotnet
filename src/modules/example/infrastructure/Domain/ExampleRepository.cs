namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Microsoft.EntityFrameworkCore;

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
	public ExampleRepository(ExampleDbContext exampleDbContext)
	{
		this.exampleDbContext = exampleDbContext;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public ExampleRepository(IEventDispatcher<IEvent> domainEventDispatcher)
	{
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
		this.exampleDbContext.Example.Add(example);
		await this.exampleDbContext.SaveChangesAsync();
	}
}