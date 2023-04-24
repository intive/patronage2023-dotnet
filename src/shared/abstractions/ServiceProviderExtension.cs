using System.Reflection;

using Intive.Patronage2023.Shared.Abstractions.Attributes;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Define extension methods for Service Provider.
/// </summary>
public static class ServiceProviderExtension
{
	/// <summary>
	/// Registration from assemblies.
	/// </summary>
	/// <param name="services">Service collection.</param>
	/// <param name="type">Type of interface implemented by the class.</param>
	/// <returns>Service collection with registered class.</returns>
	public static IServiceCollection AddFromAssemblies(this IServiceCollection services, Type type)
	{
		var concreteTypes = AppDomain.CurrentDomain
			.GetAssemblies()
			.SelectMany(x => x.GetTypes())
			.Where(x => x.GetInterfaces().Any(y => type.IsGenericType ? (y.IsGenericType && y.GetGenericTypeDefinition() == type) : y == type))
			.ToList();

		foreach (var concreteType in concreteTypes)
		{
			var concreteTypeInterface = concreteType.GetInterfaces()
				.First(i => type.IsGenericType ? (i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(type)) : i.IsAssignableFrom(type));

			var concreteTypeLifetime = concreteType.GetCustomAttribute<LifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Scoped;

			var service = ServiceDescriptor.Describe(concreteTypeInterface, concreteType, concreteTypeLifetime);

			services.Add(service);
		}

		return services;
	}

	/// <summary>
	/// Register from assemblies using extra param, assemblies.
	/// </summary>
	/// <param name="services">Service collection.</param>
	/// <param name="type">Type of interface implemented by the class.</param>
	/// <param name="assemblies">Assemblies to search for concrete types.</param>
	/// <returns>Service collection with registered class.</returns>
	public static IServiceCollection AddFromAssemblies(this IServiceCollection services, Type type, params Assembly[] assemblies)
	{
		var concreteTypes = assemblies
			.SelectMany(x => x.GetTypes())
			.Where(x => x.GetInterfaces().Any(y => type.IsGenericType ? (y.IsGenericType && y.GetGenericTypeDefinition() == type) : y == type))
			.ToList();

		foreach (var concreteType in concreteTypes)
		{
			var concreteTypeInterface = concreteType.GetInterfaces()
				.First(i => type.IsGenericType ? (i.IsGenericType && i.GetGenericTypeDefinition().IsAssignableFrom(type)) : i.IsAssignableFrom(type));

			var concreteTypeLifetime = concreteType.GetCustomAttribute<LifetimeAttribute>()?.Lifetime ?? ServiceLifetime.Scoped;

			var service = ServiceDescriptor.Describe(concreteTypeInterface, concreteType, concreteTypeLifetime);

			services.Add(service);
		}

		return services;
	}
}