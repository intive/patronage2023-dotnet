using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;

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

	/// <summary>
	/// Reads a list of data records from a CSV file using the provided IFormFile and CsvConfiguration.
	/// If the CSV file does not start with the expected header, the header is added.
	/// </summary>
	/// <typeparam name="THeader">The type used for generating the expected CSV file header.
	/// This should have properties corresponding to headers in the CSV file.</typeparam>
	/// <param name="file">The IFormFile containing the CSV data.</param>
	/// <param name="csvConfig">The CsvConfiguration for parsing the CSV file.</param>
	/// <returns>A list of data records of type T.</returns>
	public Task<List<T>> GetRecordsFromCsv<THeader>(IFormFile file, CsvConfiguration csvConfig);
}