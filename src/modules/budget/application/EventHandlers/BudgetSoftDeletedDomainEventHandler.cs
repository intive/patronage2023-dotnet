using Intive.Patronage2023.Modules.Budget.Application.Budget.RemovingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// This class is a domain event handler that handles the "BudgetSoftDeleteDomainEvent" event.
/// </summary>
public class BudgetSoftDeletedDomainEventHandler : IDomainEventHandler<BudgetSoftDeletedDomainEvent>
{
	private readonly ICommandBus commandBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetSoftDeletedDomainEventHandler"/> class.
	/// </summary>
	/// <param name="commandBus">Command Bus.</param>
	public BudgetSoftDeletedDomainEventHandler(ICommandBus commandBus)
	{
		this.commandBus = commandBus;
	}

	/// <summary>
	/// This method is the entry point for handling the "BudgetSoftDeleteDomainEvent" event.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public async Task Handle(BudgetSoftDeletedDomainEvent notification, CancellationToken cancellationToken)
	{
		var removeBudgetTransactions = new RemoveBudgetTransactions(notification.Id.Value);
		await this.commandBus.Send(removeBudgetTransactions);

		// TODO: Use logger
		cancellationToken.ThrowIfCancellationRequested();
	}
}