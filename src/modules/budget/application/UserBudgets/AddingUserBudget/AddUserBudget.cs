using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// Add User Budget command.
/// </summary>
/// <param name="Id">Record Id.</param>
/// <param name="UserId">User Id.</param>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="UserRole">User role.</param>
public record AddUserBudget(Guid Id, UserId UserId, BudgetId BudgetId, UserRole UserRole) : ICommand;

/// <summary>
/// AddUserBudget Command Handler.
/// </summary>
public class HandleAddUserBudget : ICommandHandler<AddUserBudget>
{
	private readonly IRepository<UserBudgetAggregate, Guid> userBudgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleAddUserBudget"/> class.
	/// </summary>
	/// <param name="userBudgetRepository">Repository that manages User Budget aggregate root.</param>
	public HandleAddUserBudget(IRepository<UserBudgetAggregate, Guid> userBudgetRepository)
	{
		this.userBudgetRepository = userBudgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(AddUserBudget command, CancellationToken cancellationToken)
	{
		var userBudget = UserBudgetAggregate.Create(command.Id, command.UserId, command.BudgetId, command.UserRole);

		await this.userBudgetRepository.Persist(userBudget);
	}
}