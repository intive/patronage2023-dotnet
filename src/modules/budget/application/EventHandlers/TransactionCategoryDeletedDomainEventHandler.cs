using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Handles the domain event when a transaction category is deleted.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class TransactionCategoryDeletedDomainEventHandler : IDomainEventHandler<TransactionCategoryDeletedDomainEvent>
{
	/// <summary>
	/// Handles the notification of the transaction category being deleted.
	/// </summary>
	/// <param name="notification">The notification of the transaction category being deleted.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>A task representing the completion of the handling process.</returns>
	public Task Handle(TransactionCategoryDeletedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}