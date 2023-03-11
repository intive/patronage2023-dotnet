namespace Intive.Patronage2023.Modules.Example.Infrastructure.Domain;

using System.Collections.Generic;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Example aggregate repository.
/// </summary>
public class ExampleRepository : IExampleRepository
{
	private readonly ExampleDbContext exampleDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="exampleDbContext">Database context.</param>
	public ExampleRepository(ExampleDbContext exampleDbContext)
	{
		this.exampleDbContext = exampleDbContext;
	}

	/// <summary>
	/// Retrieves example aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<ExampleAggregate> GetById(Guid id)
		=> this.exampleDbContext.ExampleAggregates.FirstOrDefaultAsync(x => x.Id == id);

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public Task Persist(ExampleAggregate example)
	{
		this.exampleDbContext.ExampleAggregates.Add(example);
		return this.exampleDbContext.SaveChangesAsync();
	}
}