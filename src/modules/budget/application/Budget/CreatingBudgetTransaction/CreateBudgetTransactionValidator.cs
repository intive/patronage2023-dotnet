using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Budget Transaction validator class.
/// </summary>
public class CreateBudgetTransactionValidator : AbstractValidator<CreateBudgetTransaction>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public CreateBudgetTransactionValidator(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(transaction => transaction.Id).NotNull();
		this.RuleFor(transaction => transaction.Type).Must(x => Enum.IsDefined(typeof(TransactionType), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name).NotEmpty().NotNull().Length(3, 58);
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().Must(this.IsValueAppropriateToType)
			.WithMessage("Value must be positive for income or negative for expense");
		this.RuleFor(transaction => transaction.Category).Must(x => Enum.IsDefined(typeof(CategoryType), x)).NotEmpty().NotNull();
		this.RuleFor(transaction => new { transaction.BudgetId, transaction.TransactionDate }).MustAsync(async (x, cancellation) => await this.IsDateInBudgetPeriod(x.BudgetId, x.TransactionDate, cancellation)).WithMessage("Transaction date is outside the budget period.");
		this.RuleFor(transaction => transaction.BudgetId).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
	}

	private async Task<bool> IsBudgetExists(Guid budgetGuid, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetRepository.GetById(budgetId);
		return budget != null;
	}

	private bool IsValueAppropriateToType(CreateBudgetTransaction budgetTransaction, decimal amount)
	{
		if (budgetTransaction.Type == TransactionType.Income)
		{
			return amount > 0;
		}

		if (budgetTransaction.Type == TransactionType.Expense)
		{
			return amount < 0;
		}

		return false;
	}

	private async Task<bool> IsDateInBudgetPeriod(Guid budgetGuid, DateTime transactionDate, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(budgetGuid);
		var budget = await this.budgetRepository.GetById(budgetId);
		if (budget == null)
		{
			return false;
		}

		return transactionDate >= budget!.Period.StartDate && transactionDate <= budget.Period.EndDate;
	}
}