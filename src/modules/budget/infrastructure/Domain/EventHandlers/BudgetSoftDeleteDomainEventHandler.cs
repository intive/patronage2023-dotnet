using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain.EventHandlers;

/// <summary>
/// This class is a domain event handler that handles the "BudgetSoftDeleteDomainEvent" event.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class BudgetSoftDeleteDomainEventHandler : IDomainEventHandler<BudgetSoftDeleteDomainEvent>
{
	/// <summary>
	/// This method is the entry point for handling the "BudgetSoftDeleteDomainEvent" event.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public Task Handle(BudgetSoftDeleteDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}