using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

/// <summary>
/// Example aggregate repository.
/// </summary>
public class ExampleRepository : BaseRepository<ExampleAggregate, ExampleId>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="exampleDbContext">Database context.</param>
	/// <param name="domainEventDispatcher">Event dispatcher.</param>
	public ExampleRepository(ExampleDbContext exampleDbContext, IEventDispatcher<IEvent> domainEventDispatcher)
		: base(exampleDbContext, domainEventDispatcher)
	{
	}
}