using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
///  A record representing the query to retrieve information about a user's role in a specific budget.
/// </summary>
public record GetUserBudgetRole() : IQuery<UserBudgetRoleInfo?>
{
	/// <summary>
	///  Object representing the ID of the budget that needs to be retrieved.
	/// </summary>
	public BudgetId BudgetId { get; init; }
}

/// <summary>
/// This Class is a query handler for the GetUserBudgetRole query.
/// </summary>
public class GetUserBudgetRoleQueryHandler : IQueryHandler<GetUserBudgetRole, UserBudgetRoleInfo?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetUserBudgetRoleQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="contextAccessor">Object representing an accessor for the current execution context.</param>
	public GetUserBudgetRoleQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// Representing the asynchronous operation to retrieve information about a user's role in the specified budget.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">Object representing a cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>Returns a UserBudgetRoleInfo object with a UserRole property that returns the user or null role.</returns>
	public async Task<UserBudgetRoleInfo?> Handle(GetUserBudgetRole query, CancellationToken cancellationToken)
	{
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
		var entity = await this.budgetDbContext.UserBudget
			.FirstOrDefaultAsync(x => x.UserId == userId && x.BudgetId == query.BudgetId, cancellationToken: cancellationToken);

		return entity is null ? UserBudgetAggregateRoleInfoMapper.Map(null) : UserBudgetAggregateRoleInfoMapper.Map(entity.UserRole);
	}
}