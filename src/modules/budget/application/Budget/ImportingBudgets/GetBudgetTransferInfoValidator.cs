using System.Globalization;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;

/// <summary>
/// Validator for the GetBudgetTransferInfo model.
/// </summary>
public class GetBudgetTransferInfoValidator : AbstractValidator<GetBudgetTransferInfo>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransferInfoValidator"/> class.
	/// </summary>
	public GetBudgetTransferInfoValidator()
	{
		this.RuleFor(budget => budget.Name)
			.NotEmpty()
			.WithMessage("Budget name is missing")
			.WithErrorCode("1.2");

		this.RuleFor(budget => budget.IconName)
			.NotEmpty()
			.WithMessage("Budget icon name is missing")
			.WithErrorCode("1.13");

		this.RuleFor(budget => budget.Currency)
			.Must(this.IsCurrencyDefined)
			.WithMessage("The selected currency is not supported.")
			.WithErrorCode("1.10");

		this.RuleFor(budget => budget.Value)
			.NotEmpty()
			.WithMessage("Budget value is missing")
			.WithErrorCode("1.8")
			.Must(this.BeValidDecimal)
			.WithMessage("Budget value is not a valid decimal number")
			.WithErrorCode("1.14");

		this.RuleFor(budget => budget.StartDate)
			.NotEmpty()
			.WithMessage("Budget start date is missing")
			.WithErrorCode("1.5")
			.Must(this.BeValidDate)
			.WithMessage("Budget start date is not a valid date")
			.WithErrorCode("1.15");

		this.RuleFor(budget => budget.EndDate)
			.NotEmpty()
			.WithMessage("Budget end date is missing")
			.WithErrorCode("1.6")
			.Must(this.BeValidDate)
			.WithMessage("Budget end date is not a valid date")
			.WithErrorCode("1.16")
			.GreaterThan(budget => budget.StartDate)
			.WithMessage("Budget start date cannot be later than or equal to end date")
			.WithErrorCode("1.7");
	}

	private bool BeValidDecimal(string value)
	{
		return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
	}

	private bool BeValidDate(string value)
	{
		return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
	}

	private bool IsCurrencyDefined(string value)
	{
		if (int.TryParse(value, out int parsedValue))
		{
			return Enum.IsDefined(typeof(Currency), parsedValue);
		}

		return Enum.IsDefined(typeof(Currency), value);
	}
}