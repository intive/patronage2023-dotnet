using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Application.Extensions;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;

/// <summary>
/// Get dates query.
/// </summary>
public record GetBudgetsReport() : IQuery<BudgetsReport<BudgetAmount>>
{
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
/// Get Budgets Report handler.
/// </summary>
public class GetBudgetsReportQueryHandler : IQueryHandler<GetBudgetsReport, BudgetsReport<BudgetAmount>>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsReportQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	public GetBudgetsReportQueryHandler(BudgetDbContext budgetDbContext, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// GetBudgetsReport query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns> BudgetsReport which holds lists of incomes and expenses, TotalBalance,TrendValue,PeriodValue. </returns>
	public async Task<BudgetsReport<BudgetAmount>> Handle(GetBudgetsReport query, CancellationToken cancellationToken)
	{
		var budgets = this.budgetDbContext.Budget.AsQueryable();
		var transactions = this.budgetDbContext.Transaction.AsQueryable();
		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);

		var budgetsList = budgets.Where(x => x.UserId == userId).ToList();

		var transactionsList =
			(
			from budget in budgetsList
			join budgetTransaction in transactions
						on budget.Id equals budgetTransaction.BudgetId
			select budgetTransaction).AsQueryable();

		if (!budgetsList.Any())
		{
			return new BudgetsReport<BudgetAmount> { };
		}

		decimal totalBalance = 0;
		decimal periodValue = 0;
		decimal budgetsStartValue = 0;

		budgetsStartValue = transactionsList
				.NotCancelled()
				.Where(x => x.BudgetTransactionDate < query.StartDate)
				.Sum(x => x.Value);

		totalBalance = transactionsList
				.NotCancelled()
				.Sum(x => x.Value);

		periodValue = transactionsList
				.NotCancelled()
				.Within(query.StartDate, query.EndDate)
				.Sum(x => x.Value);

		var budgetsIncomeList = await transactionsList
				.NotCancelled()
				.WithType(TransactionType.Income)
				.Within(query.StartDate, query.EndDate)
				.MapToBudgetAmount()
					.GroupBy(x => x.DatePoint.Date)
					.Select(x => new BudgetAmount
					{
						DatePoint = x.Key,
						Value = x.Sum(x => x.Value),
					})
					.ToListAsync(cancellationToken: cancellationToken);

		var budgetsExpensesList = await transactionsList
				.NotCancelled()
				.WithType(TransactionType.Expense)
				.Within(query.StartDate, query.EndDate)
				.MapToBudgetAmount()
					.GroupBy(x => x.DatePoint.Date)
					.Select(x => new BudgetAmount
					{
						DatePoint = x.Key,
						Value = (-1) * x.Sum(x => x.Value),
					})
					.ToListAsync(cancellationToken: cancellationToken);

		decimal? trendValue = budgetsStartValue > 0 ? periodValue / budgetsStartValue * 100 : null;
		var result = new BudgetsReport<BudgetAmount> { Incomes = budgetsIncomeList, Expenses = budgetsExpensesList, TotalBalance = totalBalance, PeriodValue = periodValue, TrendValue = trendValue };
		return result;
	}
}