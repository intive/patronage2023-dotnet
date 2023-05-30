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

		var budgetsList = budgets.Where(x => x.UserId == userId).Select(x => x.Id).ToList();

		if (budgetsList.Count == 0)
		{
			return new BudgetsReport<BudgetAmount> { };
		}

		decimal totalBalance = 0;
		decimal periodValue = 0;
		decimal budgetsStartValue = 0;

		var budgetsIncomeList = new List<BudgetAmount>();
		var budgetsExpensesList = new List<BudgetAmount>();

		foreach (var budgetId in budgetsList)
		{
			budgetsStartValue += transactions
				.For(budgetId)
				.NotCancelled()
				.Where(x => x.BudgetTransactionDate < query.StartDate)
				.Sum(x => x.Value);

			totalBalance += transactions
				.For(budgetId)
				.NotCancelled()
				.Sum(x => x.Value);

			periodValue += transactions
				.For(budgetId)
				.NotCancelled()
				.Within(query.StartDate, query.EndDate)
				.Sum(x => x.Value);

			var tempBudgetIncomeList = await transactions
				.For(budgetId)
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

			var tempBudgetExpensesList = await transactions
				.For(budgetId)
				.NotCancelled()
				.WithType(TransactionType.Expense)
				.Within(query.StartDate, query.EndDate)
				.MapToBudgetAmount()
					.GroupBy(x => x.DatePoint.Date)
					.Select(x => new BudgetAmount
					{
						DatePoint = x.Key,
						Value = x.Sum(x => x.Value),
					})
					.ToListAsync(cancellationToken: cancellationToken);

			foreach (var tempBudgetExpense in tempBudgetExpensesList)
			{
				var existingBudgetExpense = budgetsExpensesList.FirstOrDefault(x => x.DatePoint == tempBudgetExpense.DatePoint);
				if (existingBudgetExpense != null)
				{
					existingBudgetExpense = existingBudgetExpense with
					{
						Value = (-1) * (existingBudgetExpense.Value + tempBudgetExpense.Value),
					};
				}
				else
				{
					var positiveValueTempBudgetExpense = tempBudgetExpense with
					{
						Value = (-1) * tempBudgetExpense.Value,
					};
					budgetsExpensesList.Add(positiveValueTempBudgetExpense);
				}
			}

			foreach (var tempBudgetIncome in tempBudgetIncomeList)
			{
				var existingBudgetIncome = tempBudgetIncomeList.FirstOrDefault(x => x.DatePoint == tempBudgetIncome.DatePoint);
				if (existingBudgetIncome != null)
				{
					existingBudgetIncome = existingBudgetIncome with
					{
						Value = existingBudgetIncome.Value + existingBudgetIncome.Value,
					};
				}
				else
				{
					budgetsIncomeList.Add(tempBudgetIncome);
				}
			}
		}

		decimal? trendValue = budgetsStartValue > 0 ? periodValue / budgetsStartValue * 100 : null;
		var result = new BudgetsReport<BudgetAmount> { Incomes = budgetsIncomeList, Expenses = budgetsExpensesList, TotalBalance = totalBalance, PeriodValue = periodValue, TrendValue = trendValue };
		return result;
	}
}