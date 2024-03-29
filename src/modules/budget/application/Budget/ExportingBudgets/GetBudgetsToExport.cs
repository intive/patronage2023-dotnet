using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

/// <summary>
/// Record GetBudgetsToExport representing a query to retrieve budgets ready for export.
/// </summary>
public record GetBudgetsToExport : IQuery<GetTransferList<GetBudgetTransferInfo>?>;

/// <summary>
/// Handles the GetBudgetsToExport query by fetching the required budgets from the BudgetDbContext.
/// </summary>
public class GetBudgetsToExportQueryHandler : IQueryHandler<GetBudgetsToExport, GetTransferList<GetBudgetTransferInfo>?>
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
	/// Handles the GetBudgetsToExport query.
	/// </summary>
	/// <param name="query">The GetBudgetsToExport query to be handled.</param>
	/// <param name="cancellationToken">A token that may be used to cancel the handle operation.</param>
	/// <returns>A GetBudgetTransferList containing the budgets to be exported, or null if no budgets are found.</returns>
	public async Task<GetTransferList<GetBudgetTransferInfo>?> Handle(GetBudgetsToExport query, CancellationToken cancellationToken)
	{
		bool isAdmin = this.contextAccessor.IsAdmin();
		var budgets = this.budgetDbContext.Budget.AsQueryable();

		if (!isAdmin)
		{
			var userId = new UserId(this.contextAccessor.GetUserId()!.Value);
			var userBudgets = this.budgetDbContext.UserBudget.Where(x => x.UserId == userId).Select(y => y.BudgetId);
			budgets = budgets.Where(x => userBudgets.Contains(x.Id)).AsQueryable();
		}

		var budgetInfos = await budgets.MapToGetBudgetTransferInfo().ToListAsync(cancellationToken: cancellationToken);

		return new GetTransferList<GetBudgetTransferInfo> { CorrectList = budgetInfos };
	}
}