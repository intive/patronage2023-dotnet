using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Example.Contracts.Events;

/// <summary>
/// Example created domain event.
/// </summary>
public class ExampleCreatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleCreatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Example identifier.</param>
	/// <param name="name">Example name.</param>
	public ExampleCreatedDomainEvent(ExampleId id, string name)
	{
		this.Id = id;
		this.Name = name;
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public ExampleId Id { get; }

	/// <summary>
	/// Example name.
	/// </summary>
	public string Name { get; }
}