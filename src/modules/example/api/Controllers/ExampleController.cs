using FluentValidation;

using Hangfire;

using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Modules.Example.Application.Example.GettingExamples;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Errors;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;

using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// Example controller.
/// </summary>
[ApiController]
[Route("examples")]
public class ExampleController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;
	private readonly IValidator<CreateExample> createExampleValidator;
	private readonly IValidator<GetExamples> getExamplesValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createExampleValidator">Create example validator.</param>
	/// <param name="getExamplesValidator">Get examples validator.</param>
	public ExampleController(ICommandBus commandBus, IQueryBus queryBus, IValidator<CreateExample> createExampleValidator, IValidator<GetExamples> getExamplesValidator)
	{
		this.createExampleValidator = createExampleValidator;
		this.getExamplesValidator = getExamplesValidator;
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
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<ExampleInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetExamples([FromQuery] GetExamples request)
	{
		var validationResult = await this.getExamplesValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetExamples, PagedList<ExampleInfo>>(request);

			BackgroundJob.Enqueue<SendLogMessage>("example", sender => sender.LogInformation("Someone Gets Examples"));

			return this.Ok(pagedList);
		}

		throw new AppException("One or more error occured when trying to get examples.", validationResult.Errors);
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
	///        "Id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///        "Name": "Example"
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> CreateExample([FromBody] CreateExample request)
	{
		var validationResult = await this.createExampleValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(request);
			return this.Created($"example/{request.Id}", request.Id);
		}

		throw new AppException("One or more error occured when trying to create example.", validationResult.Errors);
	}
}