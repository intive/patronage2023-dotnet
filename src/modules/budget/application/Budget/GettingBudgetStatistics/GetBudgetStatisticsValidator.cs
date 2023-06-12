using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistics;

/// <summary>
/// GetBudgetStatisticValidator class.
/// </summary>
public class GetBudgetStatisticsValidator : AbstractValidator<GetBudgetStatistics>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetStatisticsValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public GetBudgetStatisticsValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.Id).MustAsync(this.IsBudgetExists).WithErrorCode("1.11").NotEmpty().NotNull();
		this.RuleFor(budget => budget.StartDate).NotEmpty().WithErrorCode("1.5").NotNull().LessThan(budget => budget.EndDate).WithErrorCode("1.7");
		this.RuleFor(budget => budget.EndDate).NotEmpty().WithErrorCode("1.6").NotNull();
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}
}