using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.DeleteUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.ShareBudget;

/// <summary>
/// Validation object to add and/or delete users to budget.
/// </summary>
/// <param name="UsersIds">Users ids.</param>
/// <param name="BudgetId">Owner id.</param>
public record ShareBudget(Guid[] UsersIds, Guid BudgetId) : ICommand;

/// <summary>
/// Divide users ids to add users to budget and/or delete users form budget commands.
/// </summary>
public class HandleShareBudget : ICommandHandler<ShareBudget>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly ICommandBus commandBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleShareBudget"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="commandBus">Command bus.</param>
	public HandleShareBudget(BudgetDbContext budgetDbContext, ICommandBus commandBus)
	{
		this.budgetDbContext = budgetDbContext;
		this.commandBus = commandBus;
	}

	/// <inheritdoc/>
	public async Task Handle(ShareBudget command, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(command.BudgetId);

		var currentBudgetUsersIds = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId.Equals(budgetId) && x.UserRole != UserRole.BudgetOwner)
			.Select(x => x.UserId.Value)
			.ToListAsync(cancellationToken: cancellationToken);

		var userBudgetListToAdd = command.UsersIds
			.Where(x => !currentBudgetUsersIds.Contains(x))
			.Select(userId => new AddUserBudget(Guid.NewGuid(), new UserId(userId), budgetId, UserRole.BudgetUser))
			.ToList();

		if (userBudgetListToAdd.Any())
		{
			var userBudgetList = new AddUserBudgetList(userBudgetListToAdd);

			await this.commandBus.Send(userBudgetList);
		}

		var usersIdToDelete = currentBudgetUsersIds
			.Where(x => !command.UsersIds.Contains(x))
			.Select(x => new UserId(x))
			.ToList();

		if (usersIdToDelete.Any())
		{
			var listToDelete = new DeleteUserBudgetList(budgetId, usersIdToDelete);

			await this.commandBus.Send(listToDelete);
		}
	}
}