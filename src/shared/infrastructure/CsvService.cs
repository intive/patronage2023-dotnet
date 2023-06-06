using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Intive.Patronage2023.Shared.Abstractions;
using Microsoft.AspNetCore.Http;

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
	public string GenerateFileNameWithCsvExtension()
	{
		return Guid.NewGuid() + ".csv";
	}

	/// <summary>
	/// Reads a list of data records from a CSV file using the provided IFormFile and CsvConfiguration.
	/// If the CSV file does not start with the expected header, the header is added.
	/// </summary>
	/// <typeparam name="THeader">The type used for generating the expected CSV file header.
	/// This should have properties corresponding to headers in the CSV file.</typeparam>
	/// <param name="file">The IFormFile containing the CSV data.</param>
	/// <param name="csvConfig">The CsvConfiguration for parsing the CSV file.</param>
	/// <returns>A list of data records of type T.</returns>
	public async Task<List<T>> GetRecordsFromCsv<THeader>(IFormFile file, CsvConfiguration csvConfig)
	{
		using var reader = new StreamReader(file.OpenReadStream());
		string fileContent = await reader.ReadToEndAsync();
		string expectedHeader = this.GenerateExpectedHeader<THeader>();

		if (!fileContent.StartsWith(expectedHeader))
		{
			fileContent = expectedHeader + "\n" + fileContent;
		}

		fileContent = fileContent.Replace("\"", string.Empty);

		byte[] byteArray = Encoding.UTF8.GetBytes(fileContent);
		var stream = new MemoryStream(byteArray);

		using var streamReader = new StreamReader(stream);
		using var csv = new CsvReader(streamReader, csvConfig);
		await csv.ReadAsync();

		var records = csv.GetRecords<T>().ToList();
		return records;
	}

	/// <summary>
	/// Generates the expected CSV header based on the properties of a given type.
	/// </summary>
	/// <typeparam name="THeader">The type of data that will be used to generate the CSV header.</typeparam>
	/// <returns>A string representing the expected CSV header.</returns>
	private string GenerateExpectedHeader<THeader>()
	{
		string[] headerFields = typeof(T)
			.GetProperties()
			.Select(property => property.Name)
			.ToArray();

		return string.Join(",", headerFields);
	}
}