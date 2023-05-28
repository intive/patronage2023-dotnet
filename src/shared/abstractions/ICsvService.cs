using CsvHelper;

namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Interface for CsvService that handle operations related to the CSV format.
/// This includes tasks such as generate path co CSV file and write data to csv file.
/// </summary>
/// <typeparam name="T">The type of data that will be written to a CSV file.</typeparam>
public interface ICsvService<T>
{
	/// <summary>
	/// Writes a list of budgets to a CSV file at the specified file path.
	/// </summary>
	/// <param name="records">A list of budgets to be written to the CSV file.</param>
	/// <param name="csv">Stream Writer.</param>
	public void WriteRecordsToMemoryStream(IEnumerable<T> records, CsvWriter csv);

	/// <summary>
	/// Generates a local file path for a new CSV file.
	/// </summary>
	/// <returns>A string representing the local path to a newly generated CSV file.</returns>
	public string GenerateFileNameWithCsvExtension();
}