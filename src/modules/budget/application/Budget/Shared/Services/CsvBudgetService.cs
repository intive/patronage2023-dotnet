using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;

/// <summary>
/// CsvService class implements the ICsvService interface and provides methods
/// for handling CSV data operations such as writing budget information to a CSV file
/// and generating a local file path for a new CSV file.
/// </summary>
public class CsvBudgetService : ICsvBudgetService
{
	/// <summary>
	/// Writes a list of budgets to a CSV file at the specified file path.
	/// </summary>
	/// <param name="budgets">A list of budgets to be written to the CSV file.</param>
	/// <param name="csv">CSV Writer.</param>
	public void WriteBudgetsToMemoryStream(GetBudgetTransferList budgets, CsvWriter csv)
	{
		csv.WriteHeader<GetBudgetTransferInfo>();
		csv.NextRecord();
		foreach (var budget in budgets.BudgetsList)
		{
			csv.WriteRecord(budget);
			csv.NextRecord();
		}

		csv.Flush();
	}

	/// <summary>
	/// Generates a local file path for a new CSV file.
	/// </summary>
	/// <returns>A string representing the local path to a newly generated CSV file.</returns>
	public string GenerateFileName()
	{
		return Guid.NewGuid() + ".csv";
	}
}