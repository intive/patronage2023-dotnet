using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.EventHandlers;

namespace Intive.Patronage2023.Modules.Budget.Application.EventHandlers;

/// <summary>
/// Budget created domain event handler.
/// </summary>
public class BudgetCreatedDomainEventHandler : IDomainEventHandler<BudgetCreatedDomainEvent>
{
	private readonly ICommandBus commandBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetCreatedDomainEventHandler"/> class.
	/// </summary>
	/// <param name="commandBus">Command Bus.</param>
	public BudgetCreatedDomainEventHandler(ICommandBus commandBus)
	{
		this.commandBus = commandBus;
	}

	/// <summary>
	/// Handle the notification.
	/// </summary>
	/// <param name="notification">Notification.</param>
	/// <param name="cancellationToken">Cancelation token.</param>
	/// <returns>Task.</returns>
	public async Task Handle(BudgetCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		var userId = new UserId(notification.UserId);
		var removeBudgetTransactions = new AddUserBudget(Guid.NewGuid(), userId, notification.Id, UserRole.BudgetOwner);
		await this.commandBus.Send(removeBudgetTransactions);

		// TODO: Use logger
		cancellationToken.ThrowIfCancellationRequested();
	}
}