namespace Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

/// <summary>
/// Base interface of repository.
/// </summary>
/// <typeparam name="T">Type of aggregate.</typeparam>
/// <typeparam name="TKey">Type of aggregate key.</typeparam>
public interface IRepository<T, TKey>
{
	/// <summary>
	/// Gets the aggregate by guid identifier.
	/// </summary>
	/// <param name="id">Identifier of aggregate.</param>
	/// <returns>Task that gets Example aggregate.</returns>
#nullable enable

	Task<T?> GetById(TKey id);

#nullable disable

	/// <summary>
	/// Persists aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	Task Persist(T example);
}