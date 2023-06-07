using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.DeleteUserBudget;

/// <summary>
/// Represent record for delete UserBudget.
/// </summary>
public record DeleteUserBudgetList(Guid[] UserBudgetIds) : ICommand;

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
		var userBudgetsList = await this.userBudgetRepository.GetByIds(command.UserBudgetIds);

		foreach (var userBudget in userBudgetsList)
		{
			userBudget.Delete();

			await this.userBudgetRepository.RemovePersist(userBudget);
		}
	}
}