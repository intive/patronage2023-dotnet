using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistics;

/// <summary>
/// Get budget statistic query.
/// </summary>
public record GetBudgetStatistics() : IQuery<BudgetStatistics<BudgetAmount>>
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
public class GetBudgetStatisticQueryHandler : IQueryHandler<GetBudgetStatistics, BudgetStatistics<BudgetAmount>>
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
	public async Task<BudgetStatistics<BudgetAmount>> Handle(GetBudgetStatistics query, CancellationToken cancellationToken)
	{
		var transactions = this.budgetDbContext.Transaction.AsQueryable();
		var budgetId = new BudgetId(query.Id);

		decimal budgetValueAtStartDate = transactions
				.For(budgetId)
				.NotCancelled()
				.Where(x => x.BudgetTransactionDate < query.StartDate)
				.Sum(x => x.Value);

		var budgetTransactionValues = await transactions
			.For(budgetId)
			.NotCancelled()
			.Within(query.StartDate, query.EndDate)
			.MapToBudgetAmount()
				.GroupBy(x => x.DatePoint.Date)
				.Select(x => new BudgetAmount
				{
					DatePoint = x.Key,
					Value = x.Sum(x => x.Value),
				})
				.ToListAsync(cancellationToken: cancellationToken);

		if (budgetTransactionValues.Count == 0)
		{
			return new BudgetStatistics<BudgetAmount> { Items = budgetTransactionValues, TotalBudgetValue = 0, PeriodValue = 0, TrendValue = 0 };
		}

		budgetTransactionValues.Insert(0, new BudgetAmount()
		{
			Value = budgetValueAtStartDate,
			DatePoint = budgetTransactionValues[0].DatePoint.AddMinutes(-1),
		});

		for (int i = 1; i < budgetTransactionValues.Count; i++)
		{
			BudgetAmount prevBudget = budgetTransactionValues[i - 1];
			budgetTransactionValues[i] = budgetTransactionValues[i] with {
				Value = prevBudget.Value + budgetTransactionValues[i].Value,
			};
		}

		decimal totalBudgetValue = this.budgetDbContext.Transaction
			.For(budgetId)
			.NotCancelled()
			.Sum(x => x.Value);

		decimal periodValue = this.budgetDbContext.Transaction
			.For(budgetId)
			.NotCancelled()
			.Within(query.StartDate, query.EndDate)
			.Sum(x => x.Value);

		decimal trendValue = budgetTransactionValues[0].Value > 0 ? (budgetTransactionValues.Last().Value - budgetValueAtStartDate) / budgetValueAtStartDate * 100 : 0;

		var result = new BudgetStatistics<BudgetAmount> { Items = budgetTransactionValues, TotalBudgetValue = totalBudgetValue, PeriodValue = periodValue, TrendValue = trendValue };
		return result;
	}
}