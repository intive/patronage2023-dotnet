using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Handles the domain event when a UserBudget is deleted.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class UserBudgetDeletedDomainEventHandler : IDomainEventHandler<UserBudgetDeletedDomainEvent>
{
	/// <inheritdoc/>
	public Task Handle(UserBudgetDeletedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}