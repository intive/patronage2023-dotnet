using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Budget edited domain event handler.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class BudgetEditedDomainEventHandler : IDomainEventHandler<BudgetEditedDomainEvent>
{
	/// <summary>
	/// Handle the notification.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public Task Handle(BudgetEditedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}