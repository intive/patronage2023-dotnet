using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;

/// <summary>
/// Get budget details query.
/// </summary>
public record GetUserBudgetRole() : IQuery<UserBudgetRoleInfo?>
{
	/// <summary>
	/// Budget id to retrive details for.
	/// </summary>
	public BudgetId BudgetId { get; set; }
}

/// <summary>
/// Get Budget details handler.
/// </summary>
public class GetBudgetDetailsQueryHandler : IQueryHandler<GetUserBudgetRole, UserBudgetRoleInfo?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetDetailsQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="contextAccessor">sfasdfasd.</param>
	public GetBudgetDetailsQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// GetBudgetDetails query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>BudgetDetailsInfo or null.</returns>
	public async Task<UserBudgetRoleInfo?> Handle(GetUserBudgetRole query, CancellationToken cancellationToken)
	{
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
		var entity = await this.budgetDbContext.UserBudget
			.FirstOrDefaultAsync(x => x.UserId == userId && x.BudgetId == query.BudgetId, cancellationToken: cancellationToken);
		return entity is null ? null : UserBudgetAggregateRoleInfoMapper.Map(entity!);
	}
}