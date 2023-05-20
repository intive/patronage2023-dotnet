using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
/// GetBudgetsToExport.
/// </summary>
public record GetBudgetsToExport : IQuery<GetBudgetTransferList?>;

/// <summary>
/// GetBudgetsToExportQueryHandler.
/// </summary>
public class GetBudgetsToExportQueryHandler : IQueryHandler<GetBudgetsToExport, GetBudgetTransferList?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsToExportQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	public GetBudgetsToExportQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// GetBudgetsToExportQueryHandler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>List of Budgets.</returns>
	public async Task<GetBudgetTransferList?> Handle(GetBudgetsToExport query, CancellationToken cancellationToken)
	{
		bool isAdmin = this.contextAccessor.IsAdmin();
		var budgets = this.budgetDbContext.Budget.AsQueryable();

		if (!isAdmin)
		{
			var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
			var userBudgets = this.budgetDbContext.UserBudget.Where(x => x.UserId == userId).Select(y => y.BudgetId);
			budgets = budgets.Where(x => userBudgets.Contains(x.Id)).AsQueryable();
		}

		var budgetInfos = await budgets.Select(entity => BudgetAggregateGetBudgetsToExportInfoMapper.Map(entity)).ToListAsync();

		return new GetBudgetTransferList { BudgetsList = budgetInfos };
	}
}