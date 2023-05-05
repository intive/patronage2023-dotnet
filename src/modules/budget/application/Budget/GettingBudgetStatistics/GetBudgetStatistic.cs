using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;

/// <summary>
/// Get budget statistic query.
/// </summary>
public record GetBudgetStatistic() : IQuery<BudgetStatistics<BudgetStatisticInfo>>
{
	/// <summary>
	/// Budget Id from  which we retrive values.
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	/// Start Date for retrieving values.
	/// </summary>
	public DateTime StartDate { get; set; }

	/// <summary>
	/// End Date for retrieving values.
	/// </summary>
	public DateTime EndDate { get; set; }
}

/// <summary>
/// Get Budget statistic handler.
/// </summary>
public class GetBudgetStatisticQueryHandler : IQueryHandler<GetBudgetStatistic, BudgetStatistics<BudgetStatisticInfo>>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetStatisticQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	public GetBudgetStatisticQueryHandler(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
	}

	/// <summary>
	/// GetBudgetStatistic query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns> List of transaction in given period of time, and List of statistics.</returns>
	public async Task<BudgetStatistics<BudgetStatisticInfo>> Handle(GetBudgetStatistic query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Transaction.AsQueryable();
		var budgetId = new BudgetId(query.Id);

		var budgetValues = await budgets.Select(BudgetStatisticInfoMapper.Map)
			.Where(x => x.BudgetId == budgetId)
			.Where(x => x.BudgetTransactionDate >= query.StartDate && x.BudgetTransactionDate <= query.EndDate)
			.ToListAsync(cancellationToken: cancellationToken);

		decimal totalBudgetValue = this.budgetDbContext.Transaction
			.Where(x => x.BudgetId == budgetId)
			.Sum(x => x.Value);

		decimal periodValue = this.budgetDbContext.Transaction
			.Where(x => x.BudgetId == budgetId)
			.Where(x => x.BudgetTransactionDate >= query.StartDate && x.BudgetTransactionDate <= query.EndDate)
			.Sum(x => x.Value);

		decimal startOfPeriodBudgetValue = this.budgetDbContext.Transaction
			.Where(x => x.BudgetId == budgetId)
			.Where(x => x.BudgetTransactionDate <= query.StartDate)
			.Sum(x => x.Value);

		decimal trendValue = startOfPeriodBudgetValue > 0 ? (periodValue - startOfPeriodBudgetValue) / startOfPeriodBudgetValue * 100 : 0;

		var result = new BudgetStatistics<BudgetStatisticInfo> { Items = budgetValues, TotalBudgetValue = totalBudgetValue, PeriodValue = periodValue, TrendValue = trendValue };
		return result;
	}
}