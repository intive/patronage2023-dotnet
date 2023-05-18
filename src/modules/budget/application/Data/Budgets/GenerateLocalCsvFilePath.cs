namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class GenerateLocalCsvFilePath.
/// </summary>
public class GenerateLocalCsvFilePath
{
	/// <summary>
	/// Generates a local file path for a new CSV file.
	/// </summary>
	/// <returns>A string representing the local path to a newly generated CSV file.</returns>
	public string Generate()
	{
		string localPath = "data"; ////src\api\app\data
		Directory.CreateDirectory(localPath);
		string fileName = Guid.NewGuid().ToString() + ".csv";
		return Path.Combine(localPath, fileName);
	}
}