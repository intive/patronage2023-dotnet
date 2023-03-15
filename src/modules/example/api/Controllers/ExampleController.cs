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
	public async Task<IActionResult> GetExamples(GetExamples request)
	{
		var validationResult = await this.getExamplesValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetExamples, PagedList<ExampleInfo>>(request);
			return this.Ok(pagedList);
		}

		string errorResponse = new ErrorResponse(" ", " ", " ", validationResult.Errors).CreateResponse();
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
		var validator = await this.createExampleValidator.ValidateAsync(request);
		if (validator.IsValid)
		{
			await this.commandBus.Send(request);
			return this.Created($"example/{request.Id}", request.Id);
		}

		string errorResponse = new ErrorResponse(" ", " ", " ", validator.Errors).CreateResponse();
		return this.ValidationProblem(detail: errorResponse, statusCode: 400);
	}
}
