using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
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
	private readonly ICategoryProvider categoryProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	/// <param name="categoryProvider">The provider used to get budget transaction categories.</param>
	public GetBudgetTransactionValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository, ICategoryProvider categoryProvider)
	{
		this.budgetRepository = budgetRepository;
		this.categoryProvider = categoryProvider;
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0);
		this.RuleFor(budget => budget.PageSize).GreaterThan(0);
		this.RuleFor(budget => budget.TransactionType).Must(x => x is null || Enum.IsDefined(typeof(TransactionType), x));
		this.RuleFor(budget => new { budget.CategoryTypes, budget.BudgetId })
			.Must(x => this.AreAllCategoriesDefined(x.CategoryTypes, x.BudgetId))
			.WithMessage("One or more categories are not defined.");
		this.RuleFor(budget => budget.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private bool AreAllCategoriesDefined(CategoryType[]? categoryTypes, BudgetId budgetId)
	{
		if (categoryTypes is null)
		{
			return true;
		}

		var categories = this.categoryProvider.GetForBudget(budgetId);
		return categoryTypes.All(categoryType => categories.Any(category => category.Name == categoryType.CategoryName));
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetGuid, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetGuid);
		return budget != null;
	}
}