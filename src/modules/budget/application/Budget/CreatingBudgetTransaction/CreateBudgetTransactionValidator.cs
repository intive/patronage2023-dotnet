using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Contracts.Helpers;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

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
		this.RuleFor(transaction => transaction.Type).Must(x => Enum.IsDefined(typeof(TransactionType), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name).NotEmpty().NotNull().Length(3, 58);
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().Must(this.IsValueAppropriateToType)
			.WithMessage("Value must be positive for income or negative for expense");
		this.RuleFor(transaction => transaction.Category).Must(x => Enum.IsDefined(typeof(CategoryType), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.TransactionDate).Must(date => date >= DateTime.Now.AddMonths(-1));
		this.RuleFor(transaction => transaction.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetRepository.GetById(budgetId);
		if (budget == null)
		{
			return false;
		}

		return true;
	}

	private bool IsValueAppropriateToType(CreateBudgetTransaction budgetTransaction, decimal amount)
	{
		if (budgetTransaction.Type == TransactionType.Income)
		{
			return amount > 0;
		}
		else if (budgetTransaction.Type == TransactionType.Expense)
		{
			return amount < 0;
		}

		return false;
	}
}