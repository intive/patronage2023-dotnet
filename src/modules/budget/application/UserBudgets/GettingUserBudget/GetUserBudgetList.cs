using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
/// Get UserBudget list by BudgetId.
/// </summary>
/// <param name="BudgetId">Budget id.</param>
public record GetUserBudgetList(BudgetId BudgetId) : IQuery<List<UserBudget>>;

/// <summary>
/// GetBudgetUsers Query Handler.
/// </summary>
public class HandleGetBudgetUsers : IQueryHandler<GetUserBudgetList, List<UserBudget>>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetBudgetUsers"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	public HandleGetBudgetUsers(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <inheritdoc/>
	public async Task<List<UserBudget>> Handle(GetUserBudgetList query, CancellationToken cancellationToken)
	{
		var budgetUsersIds = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId.Equals(query.BudgetId) && x.UserRole != UserRole.BudgetOwner)
			.Select(x => new UserBudget(x.Id, x.UserId, x.BudgetId, x.UserRole, x.IsFavourite))
			.ToListAsync(cancellationToken: cancellationToken);

		return budgetUsersIds;
	}
}