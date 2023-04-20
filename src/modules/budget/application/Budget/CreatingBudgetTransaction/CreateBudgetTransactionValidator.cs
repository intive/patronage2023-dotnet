using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Budget Transaction validator class.
/// </summary>
public class CreateBudgetTransactionValidator : AbstractValidator<CreateBudgetTransaction>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public CreateBudgetTransactionValidator(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(transaction => transaction.Id).NotNull();
		this.RuleFor(transaction => transaction.BudgetId).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Type).Must(x => Enum.IsDefined(typeof(TransactionTypes), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name).NotEmpty().NotNull().Length(3, 58);
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().GreaterThan(0);
		this.RuleFor(transaction => transaction.Category).Must(x => Enum.IsDefined(typeof(CategoriesType), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.TransactionDate).Must(date => date >= DateTime.Now.AddMonths(-1));
		this.RuleFor(transaction => transaction.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetId);
		if (budget == null)
		{
			return false;
		}

		return true;
	}
}