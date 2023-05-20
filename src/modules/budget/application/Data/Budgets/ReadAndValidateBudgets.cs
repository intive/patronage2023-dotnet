using CsvHelper.Configuration;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class ReadAndValidateBudgets.
/// </summary>
public class ReadAndValidateBudgets
{
	private readonly BudgetValidator validator;
	private readonly CreateBudgetInfoAsync createBudgetInfo;

	/// <summary>
	/// Initializes a new instance of the <see cref="ReadAndValidateBudgets"/> class.
	/// DataService.
	/// </summary>
	/// <param name="validator">BudgetValidator.</param>
	/// <param name="createBudgetInfo">CreateBudgetInfoAsync.</param>
	public ReadAndValidateBudgets(BudgetValidator validator, CreateBudgetInfoAsync createBudgetInfo)
	{
		this.validator = validator;
		this.createBudgetInfo = createBudgetInfo;
	}

	/// <summary>
	/// Reads budgets from a CSV file, validates them, and returns a list of valid budgets.
	/// Any errors encountered during validation are added to the provided errors list.
	/// </summary>
	/// <param name="file">The CSV file containing the budgets to be read and validated.</param>
	/// <param name="csvConfig">Configuration for reading the CSV file.</param>
	/// <param name="errors">A list to which any validation errors will be added.</param>
	/// <returns>A list of valid budgets read from the CSV file.</returns>
	public GetBudgetTransferList ReadAndValidateBudgetsMethod(IFormFile file, CsvConfiguration csvConfig, List<string> errors)
	{
		var budgetInfos = new List<GetBudgetTransferInfo>();
		using var stream = file.OpenReadStream();
		using (var csv = new CsvReader(new StreamReader(stream), csvConfig))
		{
			csv.Read();
			var budgets = csv.GetRecords<GetBudgetTransferInfo>().ToList();
			int rowNumber = 0;

			foreach (var budget in budgets)
			{
				var results = this.validator.Validate(budget);
				rowNumber++;

				if (results.Any())
				{
					foreach (string result in results)
					{
						errors.Add($"row: {rowNumber}| error: {result}");
					}

					continue;
				}

				var updateBudget = this.createBudgetInfo.Create(budget);
				budgetInfos.Add(updateBudget!);
			}
		}

		return new GetBudgetTransferList { BudgetsList = budgetInfos };
	}
}