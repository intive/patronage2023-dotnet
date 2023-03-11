using System.Diagnostics;
using Intive.Patronage2023.Modules.Example.Contracts.Events;
using MediatR;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain.EventHandlers
{
	/// <summary>
	/// Example name updated domain event handler.
	/// </summary>
	public class ExampleNameUpdatedDomainEventHandler : INotificationHandler<ExampleNameUpdatedDomainEvent>
	{
		/// <summary>
		/// Handle the notification.
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <param name="cancellationToken">Cancelation token.</param>
		/// <returns>Task.</returns>
		public Task Handle(ExampleNameUpdatedDomainEvent notification, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			Debug.WriteLine($"Example create domian event. {nameof(ExampleNameUpdatedDomainEventHandler)}");
			return Task.CompletedTask;
		}
	}
}
