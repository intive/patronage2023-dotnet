using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;

using Xunit;

namespace Intive.Patronage2023.Budget.Application.Tests;

/// <summary>
/// Class that contains Get Budget Transaction Validator tests.
/// </summary>
public class GetBudgetTransactionsValidatorTests
{
	private readonly IBudgetRepository budgetRepository;
	private readonly IValidator<GetBudgetTransactions> getBudgetTransactionValidator;

	/// <summary>
	/// Constructor of GetBudgetTransactionsValidatorTests
	/// </summary>
	/// <param name="budgetRepository">Injected budget repository.</param>
	public GetBudgetTransactionsValidatorTests(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
		this.getBudgetTransactionValidator = new GetBudgetTransactionValidator(this.budgetRepository);
	}

	/// <summary>
	/// Validator should return false when budget with that not existing in database.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenBudgetIdNotExistingInDatabase_ShouldReturnFalse()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must not be existing BudgetId in database.
		int pageIndex = 1;
		int pageSize = new Faker().Random.Number(1, 10);

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = this.getBudgetTransactionValidator.TestValidate(getBudgetTransactions);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.BudgetId);
	}

	/// <summary>
	/// Validator should return false when page index or page size is lower or equal 0.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenPageIndexOrAndSizeIsLowerOrEqual0_ShouldReturnFalse()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		int pageIndex = 0;
		int pageSize = -1;

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = this.getBudgetTransactionValidator.TestValidate(getBudgetTransactions);

		//Assert
		result.ShouldHaveValidationErrorFor(x => x.PageSize);
		result.ShouldHaveValidationErrorFor(x => x.PageIndex);
	}

	/// <summary>
	/// Validator should return true when budget with that id existing in database.
	/// </summary>
	[Fact(Skip = "Test must be skipped till it can be executed using integration test context.")]
	public void Validator_WhenAllPropertiesAreValid_ShouldReturnTrue()
	{
		//Arrange
		var budgetId = new BudgetId(new Faker().Random.Guid()); //TODO: It must be existing BudgetId in database.
		int pageIndex = new Faker().Random.Number(1, 10);
		int pageSize = new Faker().Random.Number(1, 10);

		var getBudgetTransactions = new GetBudgetTransactions
		{
			PageSize = pageSize,
			PageIndex = pageIndex,
			BudgetId = budgetId,
		};

		//Act
		var result = this.getBudgetTransactionValidator.TestValidate(getBudgetTransactions);

		//Assert
		result.ShouldNotHaveAnyValidationErrors();
	}
}