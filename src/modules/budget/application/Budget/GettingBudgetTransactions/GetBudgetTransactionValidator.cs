using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetTransactionValidator : AbstractValidator<GetBudgetTransactions>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public GetBudgetTransactionValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0).WithErrorCode("10.1");
		this.RuleFor(budget => budget.PageSize).GreaterThan(0).WithErrorCode("10.1");
		this.RuleFor(budget => budget.TransactionType).Must(x => x is null || Enum.IsDefined(typeof(TransactionType), x)).WithErrorCode("2.2");
		this.RuleFor(budget => budget.CategoryTypes).Must(this.AreAllCategoriesDefined).WithErrorCode("2.8");
		this.RuleFor(budget => budget.BudgetId).MustAsync(this.IsBudgetExists).WithErrorCode("1.11").NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetGuid, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetGuid);
		return budget != null;
	}

	private bool AreAllCategoriesDefined(CategoryType[]? categoryTypes)
	{
		return categoryTypes is null || categoryTypes.All(categoryType => Enum.IsDefined(typeof(CategoryType), categoryType));
	}
}