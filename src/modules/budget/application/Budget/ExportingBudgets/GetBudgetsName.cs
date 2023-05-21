using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
/// <summary>
///  A record representing the query to retrieve information about a user's role in a specific budget.
/// </summary>
public record GetBudgetsName() : IQuery<GetBudgetsNameInfo?>;

/// <summary>
/// This Class is a query handler for the GetUserBudgetRole query.
/// </summary>
public class GetBudgetsNameQueryHandler : IQueryHandler<GetBudgetsName, GetBudgetsNameInfo?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsNameQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Object representing the database context used to retrieve data.</param>
	/// <param name="contextAccessor">Object representing an accessor for the current execution context.</param>
	public GetBudgetsNameQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
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
	public async Task<GetBudgetsNameInfo?> Handle(GetBudgetsName query, CancellationToken cancellationToken)
	{
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
		var entity = await this.budgetDbContext.Budget.Where(x => x.UserId == userId).Select(x => x.Name).ToListAsync(cancellationToken);

		return entity is null ? BudgetAggregateGetBudgetsNameInfoMapper.Map(null) : BudgetAggregateGetBudgetsNameInfoMapper.Map(entity);
	}
}