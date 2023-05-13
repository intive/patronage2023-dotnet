using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;

/// <summary>
/// A record representing the command for adding a new user budget.
/// </summary>
/// <param name="Id">The unique identifier of the record.</param>
/// <param name="UserId">The identifier of the owner of the budget.</param>
/// <param name="BudgetId">The identifier of the budget.</param>
/// <param name="UserRole">The role of the user associated with the budget.</param>
public record AddUserBudget(Guid Id, UserId UserId, BudgetId BudgetId, UserRole UserRole) : ICommand;

/// <summary>
/// The corresponding command handler for the AddUserBudget command.
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