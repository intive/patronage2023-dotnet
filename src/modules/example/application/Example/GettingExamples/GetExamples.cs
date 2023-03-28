using Intive.Patronage2023.Modules.Example.Application.Example.Mappers;
using Intive.Patronage2023.Modules.Example.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

/// <summary>
/// Get Examples query.
/// </summary>
public record GetExamples();

/// <summary>
/// Get Examples handler.
/// </summary>
public class GetExampleQueryHandler : IQueryHandler<GetExamples, PagedList<ExampleInfo>>
{
	private readonly ExampleDbContext exampleDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetExampleQueryHandler"/> class.
	/// </summary>
	/// <param name="exampleDbContext">Example dbContext.</param>
	public GetExampleQueryHandler(ExampleDbContext exampleDbContext)
	{
		this.exampleDbContext = exampleDbContext;
	}

	/// <summary>
	/// GetExamples query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <returns>Paged list of examples.</returns>
	public async Task<PagedList<ExampleInfo>> Handle(GetExamples query)
	{
		var examples = await this.exampleDbContext.Example.OrderBy(x => x.Id).ToListAsync();
		var mappedData = examples.Select(ExampleAggregateExampleInfoMapper.Map).ToList();
		var result = new PagedList<ExampleInfo> { Items = mappedData };
		return result;
	}
}