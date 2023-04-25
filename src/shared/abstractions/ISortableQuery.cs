namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Sorting criteria.
/// </summary>
public interface ISortableQuery
{
	/// <summary>
	/// Sort criteria.
	/// </summary>
	List<SortDescriptor> SortDescriptors { get; set; }
}