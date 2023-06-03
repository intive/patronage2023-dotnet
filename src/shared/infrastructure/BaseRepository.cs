using System.Text.Json;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.EventDispachers;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// Base repository class.
/// </summary>
/// <typeparam name="T">Type of aggregate.</typeparam>
/// <typeparam name="TKey">Type of aggregate key.</typeparam>
public abstract class BaseRepository<T, TKey> : IRepository<T, TKey>
	where T : Aggregate, IEntity<TKey>
{
	private readonly DbContext dbContext;
	private readonly IEventDispatcher<IEvent> eventDispatcher;

	/// <summary>
	/// Initializes a new instance of the <see cref="BaseRepository{T, TKey}"/> class.
	/// </summary>
	/// <param name="dbContext">Database context.</param>
	/// <param name="eventDispatcher">Event dispatcher.</param>
	public BaseRepository(DbContext dbContext, IEventDispatcher<IEvent> eventDispatcher)
	{
		this.dbContext = dbContext;
		this.eventDispatcher = eventDispatcher;
	}

	/// <inheritdoc/>
	public async Task<T?> GetById(TKey id) => await this.dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id!.Equals(id));

	/// <inheritdoc/>
	public async Task<IList<T>> GetByIds(TKey[] id) => await this.dbContext.Set<T>().Where(x => id.Contains(x.Id)).ToListAsync();

	/// <inheritdoc/>
	public async Task Persist(T aggregate)
	{
		await this.eventDispatcher.Publish(aggregate.UncommittedEvents);
		this.HandleEvents(aggregate.UncommittedEvents);

		if (!this.dbContext.Set<T>().Local.Contains(aggregate))
		{
			this.dbContext.Set<T>().Add(aggregate);
		}
		else
		{
			this.dbContext.Set<T>().Update(aggregate);
		}

		await this.dbContext.SaveChangesAsync();
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
			this.dbContext.Set<DomainEventStore>().Add(newEvent);
		}
	}
}