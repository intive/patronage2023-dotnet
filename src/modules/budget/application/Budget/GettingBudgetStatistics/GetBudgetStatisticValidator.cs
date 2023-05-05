using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;

/// <summary>
/// GetBudgetStatisticValidator class.
/// </summary>
public class GetBudgetStatisticValidator : AbstractValidator<GetBudgetStatistic>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetStatisticValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public GetBudgetStatisticValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.Id).NotEmpty().NotNull();
		this.RuleFor(budget => budget.StartDate).NotEmpty().NotNull().LessThan(budget => budget.EndDate);
		this.RuleFor(budget => budget.EndDate).NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}
}