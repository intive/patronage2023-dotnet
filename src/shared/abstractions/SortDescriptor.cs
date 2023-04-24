namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Sort criteria.
/// </summary>
public class SortDescriptor
{
	/// <summary>
	/// Column.
	/// </summary>
	public string ColumnName { get; set; }

	/// <summary>
	/// SortOrder.
	/// </summary>
	public bool SortAscending { get; set; }
}