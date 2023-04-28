namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Text search interface.
/// </summary>
public interface ITextSearchQuery
{
	/// <summary>
	/// Search text.
	/// </summary>
	string Search { get; set; }
}