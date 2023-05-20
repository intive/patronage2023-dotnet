using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Service;

/// <summary>
/// Interface for CsvService that handle operations related to the CSV format.
/// This includes tasks such as generate path co CSV file and write data to csv file.
/// </summary>
public interface ICsvService
{
	/// <summary>
	 /// Writes a list of budgets to a CSV file at the specified file path.
	 /// </summary>
	 /// <param name="budgets">A list of budgets to be written to the CSV file.</param>
	 /// <param name="filePath">The local path of the CSV file.</param>
	 /// <returns>The local path of the CSV file where the budgets were written.</returns>
	public string WriteBudgetsToCSV(GetBudgetTransferList budgets, string filePath);

	/// <summary>
	/// Generates a local file path for a new CSV file.
	/// </summary>
	/// <returns>A string representing the local path to a newly generated CSV file.</returns>
	public string GeneratePathToCsvFile();
}