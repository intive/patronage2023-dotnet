using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain.EventHandlers
{
	/// <summary>
	/// Example created domain event handler.
	/// </summary>
	public class ExampleCreatedDomainEventHandler : IDomainEventHandler<ExampleCreatedDomainEvent>
	{
		/// <summary>
		/// Handle the notification.
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <param name="cancellationToken">Cancelation token.</param>
		/// <returns>Task.</returns>
		public Task Handle(ExampleCreatedDomainEvent notification, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			// TODO: Use logger
			return Task.CompletedTask;
		}
	}
}
