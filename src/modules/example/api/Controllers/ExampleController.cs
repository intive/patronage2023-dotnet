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
	/// <response code="200">Returns the list of examples corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetExamples([FromQuery]GetExamples request)
	{
		var pagedList = await this.queryBus.Query<GetExamples, PagedList<ExampleInfo>>(new GetExamples());
		return this.Ok(pagedList);
	}

	/// <summary>
	/// Creates example.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST
	///     {
	///        "Name": "Example",
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> CreateExample([FromBody]CreateExample request)
	{
		await this.commandBus.Send(request);

		return this.Created($"example/{request.Id}", request.Id);
	}
}