using System.Diagnostics;
using Intive.Patronage2023.Modules.Example.Contracts.Events;
using MediatR;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain.EventHandlers
{
	/// <summary>
	/// Example created domain event handler.
	/// </summary>
	public class ExampleCreatedDomainEventHandler : INotificationHandler<ExampleCreatedDomainEvent>
	{
		/// <summary>
		/// Handle the notification.
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <param name="cancellationToken">Cancelation token.</param>
		/// <returns>Task.</returns>
		public Task Handle(ExampleCreatedDomainEvent notification, CancellationToken cancellationToken)
		{
			Debug.WriteLine($"Example create domian event. {nameof(ExampleCreatedDomainEventHandler)}");
			cancellationToken.ThrowIfCancellationRequested();
			return Task.CompletedTask;
		}
	}
}
