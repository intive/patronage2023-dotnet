namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// IPageableQuery.
/// </summary>
public interface IPageableQuery
{
	/// <summary>
	/// Page size.
	/// </summary>
	int PageSize { get; set; }

	/// <summary>
	/// Page index.
	/// </summary>
	int PageIndex { get; set; }

	/// <summary>
	/// Sort order.
	/// </summary>
	bool SortAscending { get; set; }
}