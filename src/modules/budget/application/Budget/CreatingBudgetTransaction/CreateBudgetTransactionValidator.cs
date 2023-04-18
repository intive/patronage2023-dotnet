using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Budget Transaction validator class.
/// </summary>
public class CreateBudgetTransactionValidator : AbstractValidator<CreateBudgetTransaction>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CreateBudgetTransactionValidator"/> class.
	/// </summary>
	public CreateBudgetTransactionValidator()
	{
		this.RuleFor(transaction => transaction.Type).NotEmpty().NotNull();
		this.RuleFor(transaction => transaction.Name)
			.NotEmpty()
			.NotNull()
			.Length(3, 58);
		this.RuleFor(transaction => transaction.Value).NotEmpty().NotNull().GreaterThan(0);
		this.RuleFor(transaction => transaction.Category).NotEmpty().NotNull();
	}
}