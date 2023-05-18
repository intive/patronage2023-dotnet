using System.Globalization;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class WriteBudgetsToCsvFile.
/// </summary>
public class WriteBudgetsToCsvFile
{
	/// <summary>
	/// Writes a list of budgets to a CSV file at the specified file path.
	/// </summary>
	/// <param name="budgets">A list of budgets to be written to the CSV file.</param>
	/// <param name="filePath">The local path of the CSV file.</param>
	/// <returns>The local path of the CSV file where the budgets were written.</returns>
	public string WriteBudgets(List<GetBudgetsToExportInfo> budgets, string filePath)
	{
		// Write text to the file
		using (var writer = new StreamWriter(filePath))
		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteHeader<GetBudgetsToExportInfo>();
			csv.NextRecord();
			foreach (var budget in budgets)
			{
				csv.WriteRecord(budget);
				csv.NextRecord();
			}
		}

		return filePath;
	}
}