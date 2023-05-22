using System.Globalization;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// CsvService class implements the ICsvService interface and provides methods
/// for handling CSV data operations such as writing budget information to a CSV file
/// and generating a local file path for a new CSV file.
/// </summary>
public class CsvService : ICsvService
{
	/// <summary>
	/// Writes a list of budgets to a CSV file at the specified file path.
	/// </summary>
	/// <param name="budgets">A list of budgets to be written to the CSV file.</param>
	/// <param name="filePath">The local path of the CSV file.</param>
	/// <returns>The local path of the CSV file where the budgets were written.</returns>
	public string WriteBudgetsToCSV(GetBudgetTransferList budgets, string filePath)
	{
		// Write text to the file
		using (var writer = new StreamWriter(filePath))
		using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			csv.WriteHeader<GetBudgetTransferInfo>();
			csv.NextRecord();
			foreach (var budget in budgets.BudgetsList)
			{
				csv.WriteRecord(budget);
				csv.NextRecord();
			}
		}

		return filePath;
	}

	/// <summary>
	/// Generates a local file path for a new CSV file.
	/// </summary>
	/// <returns>A string representing the local path to a newly generated CSV file.</returns>
	public string GeneratePathToCsvFile()
	{
		string localPath = "data"; ////src\api\app\data
		Directory.CreateDirectory(localPath);
		string fileName = Guid.NewGuid().ToString() + ".csv";
		return Path.Combine(localPath, fileName);
	}
}