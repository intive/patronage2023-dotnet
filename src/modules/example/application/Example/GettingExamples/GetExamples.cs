namespace Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;

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
	/// <summary>
	/// Initializes a new instance of the <see cref="HandleGetExamples"/> class.
	/// </summary>
	public HandleGetExamples()
	{
	}

	/// <summary>
	/// GetExamples query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <returns>Paged list of examples.</returns>
	public Task<PagedList<ExampleInfo>> Handle(GetExamples query)
	{
		var examples = new PagedList<ExampleInfo>();
		return Task.FromResult(examples);
		throw new NotImplementedException();
	}
}
