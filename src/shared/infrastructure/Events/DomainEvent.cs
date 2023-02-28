namespace Intive.Patronage2023.Shared.Infrastructure.Events;

using Intive.Patronage2023.Shared.Abstractions.Events;

/// <summary>
/// Domain event base class.
/// </summary>
public abstract class DomainEvent : IEvent
{
	/// <inheritdoc />
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}