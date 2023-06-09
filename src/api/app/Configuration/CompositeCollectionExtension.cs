namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Extension methods for composite collections in DI container.
/// </summary>
public static class CompositeCollectionExtension
{
	/// <summary>
	/// Creates an instance of an object based on the provided <paramref name="descriptor"/> using the given <paramref name="services"/>.
	/// </summary>
	/// <param name="services">The service provider used to resolve dependencies.</param>
	/// <param name="descriptor">The service descriptor containing information about the object to be created.</param>
	/// <returns>The created instance of the object.</returns>
	public static object CreateInstance(this IServiceProvider services, ServiceDescriptor descriptor)
	{
		if (descriptor.ImplementationInstance != null)
		{
			return descriptor.ImplementationInstance;
		}

		if (descriptor.ImplementationFactory != null)
		{
			return descriptor.ImplementationFactory(services);
		}

		return ActivatorUtilities.GetServiceOrCreateInstance(services, descriptor.ImplementationType!);
	}

	/// <summary>
	/// Registers a composite collection of a specified interface and concrete types in the DI container.
	/// </summary>
	/// <typeparam name="TInterface">The interface type representing the composite collection.</typeparam>
	/// <typeparam name="TConcrete">The concrete type implementing the composite collection interface.</typeparam>
	/// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
	public static void AddComposite<TInterface, TConcrete>(this IServiceCollection services)
		where TInterface : class
		where TConcrete : class, TInterface
	{
		var wrappedDescriptors = services.Where(s => s.ServiceType == typeof(TInterface)).ToList();
		foreach (var descriptor in wrappedDescriptors)
		{
			services.Remove(descriptor);
		}

		var objectFactory = ActivatorUtilities.CreateFactory(
			typeof(TConcrete),
			new[] { typeof(IEnumerable<TInterface>) });

		services.Add(ServiceDescriptor.Describe(
			typeof(TInterface),
			s => (TInterface)objectFactory(s, new object?[] { wrappedDescriptors.Select(s.CreateInstance).Cast<TInterface>() }),
			wrappedDescriptors.Select(d => d.Lifetime).Max()));
	}
}