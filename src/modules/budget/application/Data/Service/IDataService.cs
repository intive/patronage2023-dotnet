using Azure.Storage.Blobs;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Defines a service for handling data operations including importing, validating, and creating budget data.
/// </summary>
public interface IDataService
{
	/// <summary>
	/// Downloads a CSV file containing a list of budgets from Azure Blob Storage and imports the budgets into the application.
	/// </summary>
	/// <param name="filename">The name of the blob to be downloaded from Azure Blob Storage.</param>
	/// <param name="containerClient">Client for interacting with a specific blob container in Azure Blob Storage.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	Task<BudgetAggregateList> Import(string filename, BlobContainerClient containerClient, CsvConfiguration csvConfig);

	/// <summary>
	/// Creates a new budget based on the provided budget information.
	/// If a budget with the same name already exists in the database, a random number is appended to the name.
	/// </summary>
	/// <param name="budget">The budget information used to create the new budget.</param>
	/// <returns>Creates a new budget.</returns>
	public GetBudgetTransferInfo? Create(GetBudgetTransferInfo budget);

	/// <summary>
	/// Validates the properties of a budget object.
	/// </summary>
	/// <param name="budget">The budget object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget object is valid.</returns>
	public List<string> Validate(GetBudgetTransferInfo budget);

	/// <summary>
	/// Reads budgets from a CSV file, validates them, and returns a list of valid budgets.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="file">The CSV file containing the budgets to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budgets read from the CSV file.</returns>
	GetBudgetTransferList ReadAndValidateBudgetsMethod(IFormFile file, CsvConfiguration csvConfig, List<string> errors);
}