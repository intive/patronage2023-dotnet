using Intive.Patronage2023.Shared.Abstractions.Events;
using MediatR;

namespace Intive.Patronage2023.Shared.Infrastructure.EventHandlers
{
	/// <summary>
	/// Domain event handler base interface.
	/// </summary>
	/// <typeparam name="TEvent">Event type.</typeparam>
	public interface IDomainEventHandler<TEvent> : INotificationHandler<TEvent>
		where TEvent : IEvent
	{
	}
}
