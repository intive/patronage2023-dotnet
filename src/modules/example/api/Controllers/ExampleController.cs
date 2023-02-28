namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Example controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	public ExampleController(ICommandBus commandBus, IQueryBus queryBus)
	{
		this.commandBus = commandBus;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Get examples.
	/// </summary>
	/// <param name="request">Query parameters.</param>
	/// <returns>Paged list of examples.</returns>
	public async Task<IActionResult> GetExamples(GetExamples request)
	{
		var pagedList = await this.queryBus.Query<GetExamples, PagedList<ExampleInfo>>(new GetExamples());
		return this.Ok(pagedList);
	}

	/// <summary>
	/// Creates example.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	[HttpPost]
	public async Task<IActionResult> CreateExample(CreateExample request)
	{
		await this.commandBus.Send(request);

		return this.Created($"example/{request.Id}", request.Id);
	}
}