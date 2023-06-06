using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// Validator for the GetTransactionCategories query.
/// </summary>
public class GetTransactionCategoryValidator : AbstractValidator<GetTransactionCategories>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoryValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages BudgetAggregate.</param>
	public GetTransactionCategoryValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(category => category.BudgetId)
			.NotEmpty().NotNull()
			.MustAsync(this.BudgetExists).WithMessage("Budget does not exist.");
	}

	private async Task<bool> BudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var category = await this.budgetRepository.GetById(budgetId);
		return category != null;
	}
}