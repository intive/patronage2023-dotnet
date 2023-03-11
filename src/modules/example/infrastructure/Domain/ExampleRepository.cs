namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

/// <summary>
/// Example aggregate repository.
/// </summary>
public class ExampleRepository : IExampleRepository
{
	private readonly DomainEventDispatcher domainEventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public ExampleRepository(DomainEventDispatcher domainEventDispatcher)
	{
		this.domainEventDispatcher = domainEventDispatcher;
	}

	/// <summary>
	/// Retrieves example aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<ExampleAggregate> GetById(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public async Task Persist(ExampleAggregate example)
	{
		await this.domainEventDispatcher.Publish(example.UncommittedEvents);
	}
}