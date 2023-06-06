using System.Globalization;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;

/// <summary>
/// Validator for the GetBudgetTransactionImportInfo model.
/// </summary>
public class GetBudgetTransactionImportInfoValidator : AbstractValidator<GetBudgetTransactionImportInfo>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionImportInfoValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	public GetBudgetTransactionImportInfoValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;
		this.RuleFor(transaction => transaction.BudgetId)
			.MustAsync(this.IsBudgetExists)
			.WithMessage("Budget with id = {PropertyValue} does not exist.")
			.NotEmpty()
			.WithMessage("Budget id is missing.");
		this.RuleFor(transaction => transaction.Name)
			.NotEmpty()
			.WithMessage("Budget name is missing.")
			.Length(3, 58)
			.WithMessage("Budget name length not in range <3,58>.");
		this.RuleFor(transaction => transaction.TransactionType)
			.NotEmpty()
			.WithMessage("Transaction type is missing")
			.Must(x => Enum.IsDefined(typeof(TransactionType), x))
			.WithMessage("Transaction type must be Income or Expense");
		this.RuleFor(transaction => transaction.Value)
			.NotEmpty()
			.WithMessage("Value is missing.")
			.Must(this.BeAppropriateDecimal)
			.WithMessage("Value must be positive for income and negative for expense");
		this.RuleFor(transaction => transaction.CategoryType)
			.NotEmpty()
			.WithMessage("Category is missing.")
			.Must(x => Enum.IsDefined(typeof(CategoryType), x))
			.WithMessage("Category must be defined in budget");
		this.RuleFor(transaction => transaction.Date)
			.NotEmpty()
			.WithMessage("Date is missing.")
			.Must(this.BeValidDate)
			.WithMessage("Date must be in valid date format.");
		this.RuleFor(transaction => new { transaction.BudgetId, transaction.Date })
			.MustAsync(async (x, cancellation) => await this.IsDateInBudgetPeriod(x.BudgetId, x.Date, cancellation))
			.WithMessage("Transaction date is invalid or outside of the budget period.");
	}

	private bool BeAppropriateDecimal(GetBudgetTransactionTransferInfo budgetTransaction, string value)
	{
		if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
		{
			return false;
		}

		if (!Enum.TryParse(budgetTransaction.TransactionType, out TransactionType type))
		{
			return false;
		}

		if (type == TransactionType.Expense)
		{
			return decimalValue < 0;
		}

		return decimalValue > 0;
	}

	private bool BeValidDate(string value)
	{
		return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
	}

	private async Task<bool> IsDateInBudgetPeriod(BudgetId budgetId, string transactionDate, CancellationToken cancellationToken)
	{
		if (!DateTime.TryParse(transactionDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
		{
			return false;
		}

		var budget = await this.budgetDbContext.Budget.FindAsync(budgetId);
		if (budget == null)
		{
			return false;
		}

		return date >= budget!.Period.StartDate && date <= budget.Period.EndDate;
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		return await this.budgetDbContext.Budget.AnyAsync(x => x.Id == budgetId, cancellationToken);
	}
}