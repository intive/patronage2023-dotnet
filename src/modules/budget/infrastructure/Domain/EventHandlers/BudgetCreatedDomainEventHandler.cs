using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain.EventHandlers
{
	/// <summary>
	/// Budget created domain event handler.
	/// </summary>
	public class BudgetCreatedDomainEventHandler : IDomainEventHandler<BudgetCreatedDomainEvent>
	{
		/// <summary>
		/// Handle the notification.
		/// </summary>
		/// <param name="notification">Notification.</param>
		/// <param name="cancellationToken">Cancelation token.</param>
		/// <returns>Task.</returns>
		public Task Handle(BudgetCreatedDomainEvent notification, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();

			// TODO: Use logger
			return Task.CompletedTask;
		}
	}
}
