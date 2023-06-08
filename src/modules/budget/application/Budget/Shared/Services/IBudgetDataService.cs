using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// Defines a service for handling data operations including importing, validating, and creating budget data.
/// </summary>
public interface IBudgetDataService
{
	/// <summary>
	/// Converts a collection of budget information from CSV format into a list of BudgetAggregate objects.
	/// </summary>
	/// <param name="budgetsToImport">Collection of budget information to be converted, represented as GetBudgetTransferInfo objects.</param>
	/// <returns>A Task containing a BudgetAggregateList, representing the converted budget information.</returns>
	Task<BudgetAggregateList> MapFrom(IEnumerable<GetBudgetTransferInfo> budgetsToImport);

	/// <summary>
	/// Creates a new budget based on the provided budget information.
	/// If a budget with the same name already exists in the database, a random number is appended to the name.
	/// </summary>
	/// <param name="budget">The budget information used to create the new budget.</param>
	/// <param name="budgetsNames">The existing budget's names used for checking whether the new budget's name already exists in the database.</param>
	/// <returns>Creates a new budget.</returns>
	public GetBudgetTransferInfo? Create(GetBudgetTransferInfo budget, GetBudgetsNameInfo? budgetsNames);

	/// <summary>
	/// Validates the properties of a budget object.
	/// </summary>
	/// <param name="budget">The budget object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget object is valid.</returns>
	public List<string> BudgetValidate(GetBudgetTransferInfo budget);

	/// <summary>
	/// Reads budgets from a CSV file, validates them, and returns a list of valid budgets.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="file">The CSV file containing the budgets to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budgets read from the CSV file.</returns>
	public Task<GetTransferList<GetBudgetTransferInfo>> CreateValidBudgetsList(IFormFile file, CsvConfiguration csvConfig, List<string> errors);
}