using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;

/// <summary>
/// GetBudgetsReportValidator class.
/// </summary>
public class GetBudgetsReportValidator : AbstractValidator<GetBudgetsReport>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetsReportValidator"/> class.
	/// </summary>
	public GetBudgetsReportValidator()
	{
		this.RuleFor(budget => budget.StartDate).NotEmpty().NotNull().LessThan(budget => budget.EndDate);
		this.RuleFor(budget => budget.EndDate).NotEmpty().NotNull();
	}
}