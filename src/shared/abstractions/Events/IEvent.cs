namespace Intive.Patronage2023.Shared.Abstractions.Events;

/// <summary>
/// Base event interface.
/// </summary>
public interface IEvent
{
	/// <summary>
	/// Event timestamp.
	/// </summary>
	public DateTime Timestamp { get; set; }
}