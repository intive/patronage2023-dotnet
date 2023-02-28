namespace Intive.Patronage2023.Modules.Example.Domain;

using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Modules.Example.Domain.Rules;
using Intive.Patronage2023.Shared.Abstractions.Events;
using Intive.Patronage2023.Shared.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Infrastructure.Events;

/// <summary>
/// Example of aggregate root.
/// </summary>
public class ExampleAggregate : Aggregate
{
	private ExampleAggregate(Guid id, string name)
	{
		var exampleCreated = new ExampleCreatedDomainEvent(id, name);
		this.Apply(exampleCreated, this.Handle);
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public Guid Id { get; private set; }

	/// <summary>
	/// Example name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Create example.
	/// </summary>
	/// <param name="id">Unique identifier.</param>
	/// <param name="name">Example name.</param>
	/// <returns>New aggregate.</returns>
	public static ExampleAggregate Create(Guid id, string name)
	{
		return new ExampleAggregate(id, name);
	}

	/// <summary>
	/// Update Example name.
	/// </summary>
	/// <param name="name">New name.</param>
	public void UpdateName(string name)
	{
		this.CheckRule(new SuperImportantExampleBusinessRule(name));

		var evt = new ExampleNameUpdatedDomainEvent(this.Id, name);

		this.Apply(evt, this.Handle);
	}

	private void Handle(ExampleNameUpdatedDomainEvent @event)
	{
		this.Name = @event.NewName;
	}

	private void Handle(ExampleCreatedDomainEvent @event)
	{
		this.Id = @event.Id;
		this.Name = @event.Name;
	}
}