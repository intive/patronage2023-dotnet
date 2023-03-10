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
	private readonly AppDbContext appDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleRepository"/> class.
	/// </summary>
	/// <param name="appDbContext">Database context.</param>
	public ExampleRepository(AppDbContext appDbContext)
	{
		this.appDbContext = appDbContext;
	}

	/// <summary>
	/// Retrieves example aggregate.
	/// </summary>
	/// <param name="id">Aggregate identifier.</param>
	/// <returns>Aggregate.</returns>
	public Task<ExampleAggregate> GetById(Guid id)
	{
		return this.appDbContext.ExampleAggregates.FirstOrDefaultAsync(x => x.Id == id);
	}

	/// <summary>
	/// Persist aggregate state.
	/// </summary>
	/// <param name="example">Aggregate.</param>
	/// <returns>Task.</returns>
	public Task Persist(ExampleAggregate example)
	{
		this.appDbContext.ExampleAggregates.Add(example);
		return this.appDbContext.SaveChangesAsync();
	}

	/// <summary>
	/// Retrieves all example aggregates.
	/// </summary>
	/// <returns>All aggregates.</returns>
	public Task<List<ExampleAggregate>> GetAll()
	{
		return this.appDbContext.ExampleAggregates.OrderBy(x => x.Id).ToListAsync();
	}
}