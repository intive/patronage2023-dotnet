using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.User.Contracts.Events;

/// <summary>
/// User created domain event.
/// </summary>
public class UserCreatedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserCreatedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">User identifier.</param>
	/// <param name="name">User name.</param>
	public UserCreatedDomainEvent(Guid id, string name)
	{
		this.Id = id;
		this.Name = name;
	}

	/// <summary>
	/// User identifier.
	/// </summary>
	public Guid Id { get; }

	/// <summary>
	/// User name.
	/// </summary>
	public string Name { get; }
}