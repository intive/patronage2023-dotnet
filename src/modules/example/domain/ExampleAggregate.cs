using Intive.Patronage2023.Modules.Example.Contracts.Events;
using Intive.Patronage2023.Modules.Example.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Example.Domain.Rules;
using Intive.Patronage2023.Shared.Infrastructure;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

namespace Intive.Patronage2023.Modules.Example.Domain;

/// <summary>
/// Example of aggregate root.
/// </summary>
public class ExampleAggregate : Aggregate, IEntity<ExampleId>
{
	private ExampleAggregate()
	{
	}

	private ExampleAggregate(ExampleId id, string name)
	{
		if (id.Value == Guid.Empty)
		{
			throw new InvalidOperationException("Id value cannot be empty!");
		}

		var exampleCreated = new ExampleCreatedDomainEvent(id, name);
		this.Apply(exampleCreated, this.Handle);
	}

	/// <summary>
	/// Example identifier.
	/// </summary>
	public ExampleId Id { get; private set; }

	/// <summary>
	/// Example name.
	/// </summary>
	public string Name { get; private set; } = default!;

	/// <summary>
	/// Example creation date.
	/// </summary>
	public DateTime CreatedOn { get; private set; }

	/// <summary>
	/// Create example.
	/// </summary>
	/// <param name="id">Unique identifier.</param>
	/// <param name="name">Example name.</param>
	/// <returns>New aggregate.</returns>
	public static ExampleAggregate Create(Guid id, string name)
	{
		var exampleId = new ExampleId(id);
		return new ExampleAggregate(exampleId, name);
	}

	/// <summary>
	/// Update Example name.
	/// </summary>
	/// <param name="name">New name.</param>
	public void UpdateName(string name)
	{
		this.CheckRule(new ExampleCannotHaveNameStartedWithBrokenRuleBusinessRule(name));

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
		this.CreatedOn = @event.Timestamp;
	}
}