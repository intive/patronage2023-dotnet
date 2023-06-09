using Intive.Patronage2023.Shared.Abstractions.Attributes;
using Intive.Patronage2023.Shared.Infrastructure.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Represents a domain event that is raised when a UserBudget is deleted.
/// </summary>
[Lifetime(Lifetime = ServiceLifetime.Singleton)]
public class UserBudgetDeletedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserBudgetDeletedDomainEvent"/> class.
	/// </summary>
	/// <param name="id">The identifier of the user budget.</param>
	public UserBudgetDeletedDomainEvent(Guid id)
	{
		this.Id = id;
	}

	/// <summary>
	/// Gets the identifier of the UserBudget.
	/// </summary>
	public Guid Id { get; }
}