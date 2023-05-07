#pragma warning disable IDE0005
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
#pragma warning restore IDE0005
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Budget created domain event handler.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class UserBudgetAddedDomainEventHandler : IDomainEventHandler<UserBudgetAddedDomainEvent>
{
	/// <summary>
	/// Handle the notification.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public Task Handle(UserBudgetAddedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}