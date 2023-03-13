namespace Intive.Patronage2023.Modules.Example.Contracts.Events;

using Intive.Patronage2023.Shared.Infrastructure.Events;

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
	public ExampleCreatedDomainEvent(Guid id, string name)
	{
		this.Id = id;
		this.Name = name;
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public Guid Id { get; }

	/// <summary>
	/// Example name.
	/// </summary>
	public string Name { get; }
}