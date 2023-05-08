using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Example.Contracts.Events;

/// <summary>
/// Example name updated domain event.
/// </summary>
public class ExampleNameUpdatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleNameUpdatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">Example identifier.</param>
	/// <param name="newName">New name.</param>
	public ExampleNameUpdatedDomainEvent(ExampleId id, string newName)
	{
		this.Id = id;
		this.NewName = newName;
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public ExampleId Id { get; }

	/// <summary>
	/// New Example name.
	/// </summary>
	public string NewName { get; }
}