using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// Defines a service for handling data operations including importing, validating, and creating budget transaction data.
/// </summary>
public interface IBudgetTransactionDataService
{
	/// <summary>
	/// Converts a collection of budget transaction information from CSV format into a list of BudgetTransactionAggregate objects.
	/// </summary>
	/// <param name="budgetTransactionsToImport">Collection of budget transaction information to be converted, represented as GetBudgetTransactionTInfo objects.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <returns>A Task containing a BudgetTransacitonAggregateList, representing the converted budget information.</returns>
	Task<BudgetTransactionAggregateList> ConvertBudgetTransactionsFromCsvToBudgetTransactionAggregate(IEnumerable<GetBudgetTransactionImportInfo> budgetTransactionsToImport, CsvConfiguration csvConfig);

	/// <summary>
	/// Validates the properties of a budget transaction object.
	/// </summary>
	/// <param name="budgetTransaction">The budget transaction object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget transaction object is valid.</returns>
	public Task<List<string>> BudgetTransactionValidate(GetBudgetTransactionImportInfo budgetTransaction);

	/// <summary>
	/// Reads budget transactions from a CSV file, validates them, and returns a list of valid budget transactions.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="budgetId">Import destination budget id.</param>
	/// <param name="file">The CSV file containing the budget transactions to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budget transactions read from the CSV file.</returns>
	public Task<GetBudgetTransactionImportList> CreateValidBudgetTransactionsList(BudgetId budgetId, IFormFile file, CsvConfiguration csvConfig, List<string> errors);
}