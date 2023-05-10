namespace Intive.Patronage2023.Shared.Abstractions.Domain;

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
	Task<T?> GetById(TKey id);

	/// <summary>
	/// Persists aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	Task Persist(T example);
}