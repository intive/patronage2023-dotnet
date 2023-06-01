using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Handles the domain event when a transaction category is added.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class TransactionCategoryAddedDomainEventHandler : IDomainEventHandler<TransactionCategoryAddedDomainEvent>
{
	/// <summary>
	/// Handles the notification of the transaction category being added.
	/// </summary>
	/// <param name="notification">The notification of the transaction category being added.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>A task representing the completion of the handling process.</returns>
	public Task Handle(TransactionCategoryAddedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}