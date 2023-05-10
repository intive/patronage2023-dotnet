using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.Events;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
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
	/// This method implements the IDomainEventHandler interface and is responsible for handling the domain event.
	/// </summary>
	/// <param name="notification">The domain event object that is passed to the handler.</param>
	/// <param name="cancellationToken">A token that is used to indicate if the operation should be canceled.</param>
	/// <returns>The method creates a new instance of the "AddUserBudget" command and sends it to the command bus.
	/// The command is used to add the newly created budget to the UserBudget table.
	/// Finally, the method throws a cancellation exception if the operation was canceled.</returns>
	public async Task Handle(BudgetCreatedDomainEvent notification, CancellationToken cancellationToken)
	{
		var userId = notification.UserId;
		var addUserBudget = new AddUserBudget(Guid.NewGuid(), userId, notification.Id, UserRole.BudgetOwner);
		await this.commandBus.Send(addUserBudget);

		// TODO: Use logger
		cancellationToken.ThrowIfCancellationRequested();
	}
}