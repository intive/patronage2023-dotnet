using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// This class is a domain event handler that handles the "BudgetTransactionCancelledDomainEventHandler" event.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class BudgetTransactionCancelledDomainEventHandler : IDomainEventHandler<BudgetTransactionCancelledDomainEvent>
{
	/// <summary>
	/// This method is the entry point for handling the "BudgetTransactionCancelledDomainEvent" event.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public Task Handle(BudgetTransactionCancelledDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}