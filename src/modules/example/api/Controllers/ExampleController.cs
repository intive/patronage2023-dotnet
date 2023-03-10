namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

using System.Reflection.Metadata;
using FluentValidation;
using Intive.Patronage2023.Modules.Example.Application.Example;
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
	private CreateExampleValidator createExamplesValidator;
	private GetExamplesValidator getExamplesValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExampleController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createExamplesValidator">Command bus validator.</param>
	/// <param name="getExamplesValidator">Query bus validator.</param>
	public ExampleController(ICommandBus commandBus, IQueryBus queryBus, CreateExampleValidator createExamplesValidator, GetExamplesValidator getExamplesValidator)
	{
		this.createExamplesValidator = createExamplesValidator;
		this.getExamplesValidator = getExamplesValidator;
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
		var validator = this.getExamplesValidator.Validate(request);
		if (validator.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetExamples, PagedList<ExampleInfo>>(request);
			return this.Ok(pagedList);
		}

		string errorResponse = new ErrorResponse(" ", " ", " ", validator.Errors).ToJson();
		return this.ValidationProblem(detail: errorResponse, statusCode: 400);
	}

	/// <summary>
	/// Creates example.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	[HttpPost]
	public async Task<IActionResult> CreateExample(CreateExample request)
	{
		var validator = this.createExamplesValidator.Validate(request);
		if (validator.IsValid)
		{
			await this.commandBus.Send(request);
			return this.Created($"example/{request.Id}", request.Id);
		}

		string errorResponse = new ErrorResponse(" ", " ", " ", validator.Errors).ToJson();
		return this.ValidationProblem(detail: errorResponse, statusCode: 400);
	}
}
