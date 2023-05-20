using System.Globalization;
using Azure.Storage.Blobs;
using CsvHelper.Configuration;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Class BudgetImportService.
/// </summary>
public class BudgetImportService : IBudgetImportService
{
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IBlobStorageService blobStorageService;
	private readonly IDataService dataHelper;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetImportService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="contextAccessor">The ExecutionContextAccessor used for accessing context information.</param>
	/// <param name="blobStorageService">BlobStorageService.</param>
	/// <param name="dataHelper">IDataHelper.</param>
	public BudgetImportService(IExecutionContextAccessor contextAccessor, IBlobStorageService blobStorageService, IDataService dataHelper)
	{
		this.contextAccessor = contextAccessor;
		this.blobStorageService = blobStorageService;
		this.dataHelper = dataHelper;
	}

	/// <summary>
	/// Imports budgets from a provided .csv file, validates them, and stores them in the system.
	/// </summary>
	/// <param name="file">The .csv file containing the budgets to be imported.</param>
	/// <returns>A tuple containing a list of any errors encountered during the import process and
	/// the URI of the saved .csv file in the Azure Blob Storage if any budgets were successfully imported.
	/// If no budgets were imported, the URI is replaced with a message stating "No budgets were saved.".</returns>
	public async Task<GetImportResult> Import(IFormFile file)
	{
		var errors = new List<string>();

		string containerName = this.contextAccessor.GetUserId().ToString()!;
		BlobContainerClient containerClient = await this.blobStorageService.CreateBlobContainerIfNotExists(containerName);

		var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
		{
			HasHeaderRecord = false,
			Delimiter = ",",
		};

		var budgetInfos = this.dataHelper.ReadAndValidateBudgetsMethod(file, csvConfig, errors);

		if (budgetInfos.BudgetsList.Count == 0)
		{
			return new GetImportResult(
				new BudgetAggregateList(new List<BudgetAggregate>()),
				new ImportResult { ErrorsList = errors, Uri = "No budgets were saved." });
		}

		string uri = await this.blobStorageService.UploadToBlobStorage(budgetInfos, containerClient);

		string fileName = new Uri(uri).LocalPath;

		var budgetsAggregateList = await this.dataHelper.Import(fileName, containerClient, csvConfig);

		return new GetImportResult(budgetsAggregateList, new ImportResult { ErrorsList = errors, Uri = uri });
	}
}