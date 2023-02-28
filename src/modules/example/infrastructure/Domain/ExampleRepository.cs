namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

using Intive.Patronage2023.Modules.Example.Domain;

/// <summary>
/// Example aggregate repository.
/// </summary>
public class ExampleRepository : IExampleRepository
{
	/// <summary>
	/// Retrieves example aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<ExampleAggregate> GetById(Guid id)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public Task Persist(ExampleAggregate example)
	{
		throw new NotImplementedException();
	}
}