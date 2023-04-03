using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Abstractions.Attributes;

/// <summary>
/// Attribute defines the lifetime of the registered class.
/// </summary>
public class LifetimeAttribute : Attribute
{
	/// <summary>
	/// Lifetime of the registered class.
	/// </summary>
	public ServiceLifetime Lifetime { get; set; }
}