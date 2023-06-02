using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;

/// <summary>
/// 2.
/// </summary>
public class GetTransactionCategoryValidator : AbstractValidator<GetTransactionCategories>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetTransactionCategoryValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository.</param>
	public GetTransactionCategoryValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(category => category.BudgetId)
			.NotEmpty().NotNull()
			.MustAsync(this.BudgetExsist).WithMessage("Budget does not exist.");
	}

	private async Task<bool> BudgetExsist(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var category = await this.budgetRepository.GetById(budgetId);
		return category != null;
	}
}