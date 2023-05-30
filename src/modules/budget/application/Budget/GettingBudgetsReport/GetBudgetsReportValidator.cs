using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;

/// <summary>
/// GetBudgetsReportValidator class.
/// </summary>
public class GetBudgetsReportValidator : AbstractValidator<GetBudgetsReport>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsReportValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public GetBudgetsReportValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.RuleFor(budget => budget.StartDate).NotEmpty().NotNull().LessThan(budget => budget.EndDate);
		this.RuleFor(budget => budget.EndDate).NotEmpty().NotNull();
	}
}