using CsvHelper;
using Intive.Patronage2023.Shared.Abstractions;

namespace Intive.Patronage2023.Shared.Infrastructure;

/// <summary>
/// The CsvService class implements the ICsvService interface and provides methods
/// for handling CSV data operations. These operations include writing data records of any type to a CSV file
/// and generating a local file path for a new CSV file. This class is generic and can handle any data type that
/// needs to be written to a CSV file.
/// </summary>
/// <typeparam name="T">The type of data that will be written to a CSV file.</typeparam>
public class CsvService<T> : ICsvService<T>
{
	/// <summary>
	/// Writes a list of budgets to a CSV file at the specified file path.
	/// </summary>
	/// <param name="records">A list of budgets to be written to the CSV file.</param>
	/// <param name="csv">CSV Writer.</param>
	public void WriteRecordsToMemoryStream(IEnumerable<T> records, CsvWriter csv)
	{
		csv.WriteHeader<T>();
		csv.NextRecord();
		foreach (var record in records)
		{
			csv.WriteRecord(record);
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