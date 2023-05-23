using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// A record representing the command list for adding a new user budgets.
/// </summary>
/// <param name="UserBudgetList">BudgetUser to add.</param>
public record AddUserBudgetList(List<AddUserBudget> UserBudgetList) : ICommand;

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
		var userBudgetList = command.UserBudgetList.Select(x => UserBudgetAggregate.Create(x.Id, x.UserId, x.BudgetId, x.UserRole));

		foreach (var item in userBudgetList)
		{
			await this.userBudgetRepository.Persist(item);
		}
	}
}