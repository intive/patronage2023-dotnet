using Intive.Patronage2023.Shared.Abstractions.Events;

namespace Intive.Patronage2023.Shared.Infrastructure.EventDispachers
{
	/// <summary>
	/// Event dispatcher base interface.
	/// </summary>
	/// <typeparam name="TEvent">Type of event.</typeparam>
	public interface IEventDispatcher<TEvent>
		where TEvent : IEvent
	{
		/// <summary>
		/// Publish events to multiple handlers.
		/// </summary>
		/// <param name="eventsList">List of events.</param>
		/// <returns>Task.</returns>
		Task Publish(List<TEvent> eventsList);
	}
}
