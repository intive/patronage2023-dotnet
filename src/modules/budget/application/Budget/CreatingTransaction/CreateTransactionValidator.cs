using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;

/// <summary>
/// Transaction validator class.
/// </summary>
public class CreateTransactionValidator : AbstractValidator<TransactionInfo>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateTransactionValidator"/> class.
	/// </summary>
	public CreateTransactionValidator()
	{
		this.RuleFor(transaction => transaction.TransactionType).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name)
			.NotEmpty()
			.NotNull()
			.Length(3, 58);
		this.RuleFor(transaction => transaction.BudgetId).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().GreaterThan(0);
		this.RuleFor(transaction => transaction.CategoryType).NotEmpty().NotNull();
	}
}