#pragma warning disable IDE0005
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
#pragma warning restore IDE0005
using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// This class is responsible for handling the "UserBudgetAddedDomainEvent" domain event, which occurs after a user adds a new budget.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class UserBudgetAddedDomainEventHandler : IDomainEventHandler<UserBudgetAddedDomainEvent>
{
	/// <summary>
	/// This method implements the IDomainEventHandler interface and is responsible for handling the domain event.
	/// </summary>
	/// <param name="notification">The domain event object that is passed to the handler.</param>
	/// <param name="cancellationToken">A token that is used to indicate if the operation should be canceled.</param>
	/// <returns>Task.</returns>
	public Task Handle(UserBudgetAddedDomainEvent notification, CancellationToken cancellationToken)
	{
		cancellationToken.ThrowIfCancellationRequested();

		// TODO: Use logger
		return Task.CompletedTask;
	}
}