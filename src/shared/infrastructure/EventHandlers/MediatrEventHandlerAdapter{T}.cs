using Intive.Patronage2023.Shared.Abstractions.Events;

using MediatR;

namespace Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

/// <summary>
/// Implementation of adapter that handles events by mediatR.
/// </summary>
/// <typeparam name="T">Type of event to handle.</typeparam>
public class MediatrEventHandlerAdapter<T> : INotificationHandler<T>
where T : IEvent, INotification
{
	private readonly IDomainEventHandler<T> inner;

	/// <summary>
	/// Initializes a new instance of the <see cref="MediatrEventHandlerAdapter{T}"/> class.
	/// </summary>
	/// <param name="inner">Handler of domain event.</param>
	public MediatrEventHandlerAdapter(IDomainEventHandler<T> inner)
	{
		this.inner = inner;
	}

	/// <inheritdoc/>
	public Task Handle(T notification, CancellationToken cancellationToken)
	{
		return this.inner.Handle(notification, cancellationToken);
	}
}