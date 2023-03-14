using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain.EventHandlers
{
	/// <summary>
	/// Example name updated domain event handler.
	/// </summary>
	public class ExampleNameUpdatedDomainEventHandler : IDomainEventHandler<ExampleNameUpdatedDomainEvent>
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

			// TODO: Add logging using ILogger
			return Task.CompletedTask;
		}
	}
}
