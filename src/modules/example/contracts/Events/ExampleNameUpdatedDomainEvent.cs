namespace Intive.Patronage2023.Modules.Example.Contracts.Events;

using Intive.Patronage2023.Shared.Infrastructure.Events;

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
	public ExampleNameUpdatedDomainEvent(Guid id, string newName)
	{
		this.Id = id;
		this.NewName = newName;
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public Guid Id { get; }

	/// <summary>
	/// New Example name.
	/// </summary>
	public string NewName { get; }
}