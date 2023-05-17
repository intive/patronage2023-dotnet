using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper.Configuration;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// BudgetImportService.
/// </summary>
public class BudgetImportService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly DataHelperService dateHelperService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetImportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="dateHelperService">??.</param>
	public BudgetImportService(IExecutionContextAccessor contextAccessor, DataHelperService dateHelperService)
	{
		this.contextAccessor = contextAccessor;
		this.dateHelperService = dateHelperService;
	}

	/// <summary>
	/// Imports budgets from a provided .csv file, validates them, and stores them in the system.
	/// </summary>
	/// <param name="file">The .csv file containing the budgets to be imported.</param>
	/// <returns>A tuple containing a list of any errors encountered during the import process and
	/// the URI of the saved .csv file in the Azure Blob Storage if any budgets were successfully imported.
	/// If no budgets were imported, the URI is replaced with a message stating "No budgets were saved.".</returns>
	public async Task<(List<string>? ErrorsList, string? Uri)> Import(IFormFile file)
	{
		var errors = new List<string>();

		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.dateHelperService.CreateBlobContainerIfNotExists(containerName);

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		var budgetInfos = this.dateHelperService.ReadAndValidateBudgets(file, csvConfig, errors);

		if (budgetInfos.Count() == 0)
		{
			return (errors, "No budgets were saved.");
		}

		string uri = await this.dateHelperService.UploadToBlobStorage(budgetInfos, containerClient);

		string fileName = new Uri(uri).LocalPath;
		await this.dateHelperService.ImportBudgetsFromBlobStorage(fileName, containerClient, csvConfig);

		return (errors, uri);
	}
}