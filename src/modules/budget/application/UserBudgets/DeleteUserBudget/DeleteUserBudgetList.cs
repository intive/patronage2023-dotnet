using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.DeleteUserBudget;

/// <summary>
/// Represent record for delete UserBudget.
/// </summary>
public record DeleteUserBudgetList(BudgetId BudgetId, List<UserId> UsersIds) : ICommand;

/// <summary>
/// The corresponding command handler for the DeleteUserBudget command.
/// </summary>
public class HandleDeleteUserBudget : ICommandHandler<DeleteUserBudgetList>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IRepository<UserBudgetAggregate, Guid> userBudgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleDeleteUserBudget"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="userBudgetRepository">Repository that manages User Budget aggregate root.</param>
	public HandleDeleteUserBudget(BudgetDbContext budgetDbContext, IRepository<UserBudgetAggregate, Guid> userBudgetRepository)
	{
		this.budgetDbContext = budgetDbContext;
		this.userBudgetRepository = userBudgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(DeleteUserBudgetList command, CancellationToken cancellationToken)
	{
		var userBudgetIds = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId!.Equals(command.BudgetId) && command.UsersIds.Any(y => x.UserId!.Equals(y)))
			.Select(x => x.Id)
			.ToArrayAsync(cancellationToken: cancellationToken);

		var userBudgetsList = await this.userBudgetRepository.GetByIds(userBudgetIds);

		foreach (var userBudget in userBudgetsList)
		{
			userBudget.Delete();

			this.budgetDbContext.UserBudget.Remove(userBudget);

			await this.userBudgetRepository.Persist(userBudget);
		}
	}
}