using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;

/// <summary>
/// Transaction validator class.
/// </summary>
public class CreateTransactionValidator : AbstractValidator<CreateTransaction>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateTransactionValidator"/> class.
	/// </summary>
	/// <param name="budgetRepository">budgetRepository, so we can validate BudgetId.</param>
	public CreateTransactionValidator(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.RuleFor(transaction => transaction.Id).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Type).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name)
		.NotEmpty()
		.NotNull()
			.Length(3, 58);
		this.RuleFor(transaction => transaction.BudgetId.Id).MustAsync(this.IsBudgetExists).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().GreaterThan(0);
		this.RuleFor(transaction => transaction.Category).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.CreatedOn).Must(date => date <= DateTime.Now && date >= DateTime.Now.AddMonths(-1))
			.WithMessage("Data has to be from the last month.");
	}

	private async Task<bool> IsBudgetExists(Guid budgetId, CancellationToken cancellationToken)
	{
		var budget = await this.budgetRepository.GetById(budgetId);
		if (budget is null)
		{
			return false;
		}

		return true;
	}
}