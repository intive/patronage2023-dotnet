using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.DeleteUserBudget;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.ShareBudget;

/// <summary>
/// Validation object add users to budget.
/// </summary>
/// <param name="UsersIds">Users ids.</param>
/// <param name="BudgetId">Owner id.</param>
public record ShareBudget(Guid[] UsersIds, Guid BudgetId) : ICommand;

/// <summary>
/// Divide User Budget.
/// </summary>
public class HandleShareBudget : ICommandHandler<ShareBudget>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;
	private readonly BudgetDbContext budgetDbContext;
	private readonly ICommandBus commandBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleShareBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="commandBus">Command bus.</param>
	public HandleShareBudget(IRepository<BudgetAggregate, BudgetId> budgetRepository, BudgetDbContext budgetDbContext, ICommandBus commandBus)
	{
		this.budgetRepository = budgetRepository;
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

		if (userBudgetListToAdd.Count > 0)
		{
			var userBudgetList = new AddUserBudgetList(userBudgetListToAdd);

			await this.commandBus.Send(userBudgetList);
		}

		var usersIdToDelete = currentBudgetUsersIds
			.Where(x => !command.UsersIds.Contains(x))
			.Select(x => new UserId(x))
			.ToList();

		if (usersIdToDelete.Count > 0)
		{
			var listToDelete = new DeleteUserBudgetList(budgetId, usersIdToDelete);

			await this.commandBus.Send(listToDelete);
		}
	}
}