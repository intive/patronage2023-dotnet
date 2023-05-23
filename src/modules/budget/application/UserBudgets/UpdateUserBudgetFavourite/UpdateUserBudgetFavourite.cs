using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.UpdateUserBudgetFavourite;

/// <summary>
/// Update budget favourite flag for user.
/// </summary>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="IsFavourite">Favourite flag.</param>
public record UpdateUserBudgetFavourite(Guid BudgetId, bool IsFavourite) : ICommand;

/// <summary>
/// Update UserBudget favourite flag.
/// </summary>
public class HandleUpdateUserBudgetFavourite : ICommandHandler<UpdateUserBudgetFavourite>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IRepository<UserBudgetAggregate, Guid> userBudgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleUpdateUserBudgetFavourite"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="contextAccessor">Object representing an accessor for the current execution context.</param>
	/// <param name="userBudgetRepository">Repository that manages UserBudget aggregate root.</param>
	public HandleUpdateUserBudgetFavourite(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor, IRepository<UserBudgetAggregate, Guid> userBudgetRepository)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
		this.userBudgetRepository = userBudgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(UpdateUserBudgetFavourite command, CancellationToken cancellationToken)
	{
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
		var budgetId = new BudgetId(command.BudgetId);

		var userBudgetId = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId!.Equals(budgetId) && x.UserId!.Equals(userId))
			.Select(x => x.Id)
			.FirstOrDefaultAsync(cancellationToken: cancellationToken);

		var userBudget = await this.userBudgetRepository.GetById(userBudgetId);

		userBudget!.UpdateFavourite(command.IsFavourite);

		await this.userBudgetRepository.Persist(userBudget);
	}
}