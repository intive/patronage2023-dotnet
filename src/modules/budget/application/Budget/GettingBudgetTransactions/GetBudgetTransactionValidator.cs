using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;

/// <summary>
/// GetBudgetsValidator class.
/// </summary>
public class GetBudgetTransactionValidator : AbstractValidator<GetBudgetTransactions>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	/// <param name="queryBus">The query bus used for getting transaction categories.</param>
	public GetBudgetTransactionValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository, IQueryBus queryBus)
	{
		this.queryBus = queryBus;
		this.budgetRepository = budgetRepository;
		this.RuleFor(budget => budget.PageIndex).GreaterThan(0);
		this.RuleFor(budget => budget.PageSize).GreaterThan(0);
		this.RuleFor(budget => budget.TransactionType).Must(x => x is null || Enum.IsDefined(typeof(TransactionType), x));
		this.RuleFor(budget => new { budget.CategoryTypes, budget.BudgetId })
			.MustAsync(async (x, cancellation) => await this.AreAllCategoriesDefined(x.CategoryTypes, x.BudgetId, cancellation))
			.WithMessage("One or more categories are not defined.");
		this.RuleFor(budget => budget.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private async Task<bool> AreAllCategoriesDefined(string[]? categoryTypes, BudgetId budgetId, CancellationToken cancellation)
	{
		if (categoryTypes is null)
		{
			return true;
		}

		var query = new GetTransactionCategories(budgetId);
		var categoriesInfo = await this.queryBus.Query<GetTransactionCategories, TransactionCategoriesInfo>(query);
		return categoryTypes.All(categoryType => categoriesInfo.Categories!.Any(category => category.Name == categoryType));
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetGuid, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetGuid);
		return budget != null;
	}
}