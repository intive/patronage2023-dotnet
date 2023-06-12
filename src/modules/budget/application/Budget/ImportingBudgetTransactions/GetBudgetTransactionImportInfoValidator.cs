using System.Globalization;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
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
	private readonly ICategoryProvider categoryProvider;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionImportInfoValidator"/> class.
	/// </summary>
	/// <param name="budgetDbContext">BudgetDbContext.</param>
	/// <param name="categoryProvider">The provider used to get budget transaction categories.</param>
	public GetBudgetTransactionImportInfoValidator(BudgetDbContext budgetDbContext, ICategoryProvider categoryProvider)
	{
		this.budgetDbContext = budgetDbContext;
		this.categoryProvider = categoryProvider;

		this.RuleFor(transaction => transaction.BudgetId)
			.NotEmpty()
			.WithMessage("Budget id is missing")
			.WithErrorCode("1.12")
			.MustAsync(this.IsBudgetExists)
			.WithMessage("Budget with id = {PropertyValue} does not exist")
			.WithErrorCode("1.11");

		this.RuleFor(transaction => transaction.Name)
			.NotEmpty()
			.WithMessage("Budget name is missing")
			.WithErrorCode("2.3")
			.Length(3, 58)
			.WithMessage("Transaction name length not in range <3,58>.")
			.WithErrorCode("2.4");

		this.RuleFor(transaction => transaction.TransactionType)
			.NotEmpty()
			.WithMessage("Transaction type is missing")
			.WithErrorCode("2.1")
			.Must(x => Enum.IsDefined(typeof(TransactionType), x))
			.WithMessage("Transaction type must be Income or Expense")
			.WithErrorCode("2.2");

		this.RuleFor(transaction => transaction.Value)
			.NotEmpty()
			.WithMessage("Value is missing")
			.WithErrorCode("2.5")
			.Must(this.IsValidDecimal)
			.WithMessage("Value must be valid decimal")
			.WithErrorCode("2.17")
			.Must(this.IsAppropriateToType)
			.WithMessage("Value must be positive for income and negative for expense")
			.WithErrorCode("2.6");

		this.RuleFor(transaction => new
			{
				transaction.BudgetId,
				transaction.CategoryType,
			})
			.NotEmpty()
			.WithMessage("Category cannot be empty.")
			.WithErrorCode("2.7")
			.Must(x => this.IsCategoryDefined(x.BudgetId.Value, x.CategoryType))
			.WithMessage("Category is not defined.")
			.WithErrorCode("2.8");

		this.RuleFor(transaction => transaction.Date)
			.NotEmpty()
			.WithMessage("Date is missing")
			.WithErrorCode("2.13")
			.Must(this.IsValidDate)
			.WithMessage("Date must be in valid date format")
			.WithErrorCode("2.14");

		this.RuleFor(transaction => new { transaction.BudgetId, transaction.Date })
			.MustAsync(async (x, cancellation) => await this.IsDateInBudgetPeriod(x.BudgetId, x.Date, cancellation))
			.WithMessage("Transaction date is invalid or outside of the budget period")
			.WithErrorCode("2.9");

		this.RuleFor(transaction => transaction.Status)
			.NotEmpty()
			.WithMessage("Status is missing")
			.WithErrorCode("2.15")
			.Must(this.IsStatusValid)
			.WithMessage("Status must be either Active or Cancelled")
			.WithErrorCode("2.16");
	}

	private bool IsValidDecimal(string value)
	{
		return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out _);
	}

	private bool IsAppropriateToType(GetBudgetTransactionTransferInfo budgetTransaction, string value)
	{
		// Returning true for both parses, because they are tested somewhere else, and error message might be confusing.
		if (!decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal decimalValue))
		{
			return true;
		}

		if (!Enum.TryParse(budgetTransaction.TransactionType, out TransactionType type))
		{
			return true;
		}

		if (type == TransactionType.Expense)
		{
			return decimalValue < 0;
		}

		return decimalValue > 0;
	}

	private bool IsValidDate(string value)
	{
		return DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
	}

	private async Task<bool> IsDateInBudgetPeriod(BudgetId budgetId, string transactionDate, CancellationToken cancellationToken)
	{
		// Returning true in case failed parse, due to other rule validating value and this one has misleading message.
		if (!DateTime.TryParse(transactionDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
		{
			return true;
		}

		// Other rule checks if budgets exists, true to not confuse with message.
		var budget = await this.budgetDbContext.Budget.FindAsync(budgetId);
		if (budget == null)
		{
			return true;
		}

		return date >= budget!.Period.StartDate && date <= budget.Period.EndDate;
	}

	private async Task<bool> IsBudgetExists(BudgetId budgetId, CancellationToken cancellationToken)
	{
		return await this.budgetDbContext.Budget.AnyAsync(x => x.Id == budgetId, cancellationToken);
	}

	private bool IsStatusValid(string value)
	{
		if (!Enum.TryParse(value, out Status status))
		{
			return false;
		}

		return status is Status.Active or Status.Cancelled;
	}

	private bool IsCategoryDefined(Guid budgetId, string categoryString)
	{
		var category = new CategoryType(categoryString);
		var categories = this.categoryProvider.GetForBudget(new BudgetId(budgetId));
		return categories.Select(x => x.Name).Contains(category.CategoryName);
	}
}