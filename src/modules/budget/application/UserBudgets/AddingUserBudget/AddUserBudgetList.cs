using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// A record representing the command list for adding a new user budgets.
/// </summary>
/// <param name="BudgetId">Budget id to add a users.</param>
/// <param name="UsersIds">List of users id to add.</param>
public record AddUserBudgetList(Guid BudgetId, Guid[] UsersIds) : ICommand;

/// <summary>
/// The corresponding command handler for the AddUserBudget command.
/// </summary>
public class HandleAddUserBudgetList : ICommandHandler<AddUserBudgetList>
{
	private readonly IRepository<UserBudgetAggregate, Guid> userBudgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddUserBudgetList"/> class.
	/// </summary>
	/// <param name="userBudgetRepository">Repository that manages User Budget aggregate root.</param>
	public HandleAddUserBudgetList(IRepository<UserBudgetAggregate, Guid> userBudgetRepository)
	{
		this.userBudgetRepository = userBudgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(AddUserBudgetList command, CancellationToken cancellationToken)
	{
		var userBudgetList = command.UsersIds.Select(userId => UserBudgetAggregate.Create(Guid.NewGuid(), new UserId(userId), new BudgetId(command.BudgetId), UserRole.BudgetUser));

		foreach (var item in userBudgetList)
		{
			await this.userBudgetRepository.Persist(item);
		}
	}
}