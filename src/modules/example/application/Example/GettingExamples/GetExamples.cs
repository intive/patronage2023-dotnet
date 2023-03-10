namespace Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

using Intive.Patronage2023.Modules.Example.Application.Example.Mappers;
using Intive.Patronage2023.Modules.Example.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
/// <summary>
/// Get Examples query.
/// </summary>
public record GetExamples();

/// <summary>
/// Get Examples handler.
/// </summary>
public class HandleGetExamples : IQueryHandler<GetExamples, PagedList<ExampleInfo>>
{
	private readonly IExampleRepository exampleRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetExamples"/> class.
	/// </summary>
	/// <param name="exampleRepository">Example repository.</param>
	public HandleGetExamples(IExampleRepository exampleRepository)
	{
		this.exampleRepository = exampleRepository;
	}

	/// <summary>
	/// GetExamples query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <returns>Paged list of examples.</returns>
	public async Task<PagedList<ExampleInfo>> Handle(GetExamples query)
	{
		var examples = await this.exampleRepository.GetAll();
		var mappedData = examples.Select(s => ExampleAggregateExampleInfoMapper.Map(s)).ToList();

		return new PagedList<ExampleInfo> { Items = mappedData };
	}
}
