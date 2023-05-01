using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Errors;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Budget.Api.Controllers;

/// <summary>
/// Budget controller.
/// </summary>
[ApiController]
[Route("budgets")]
public class BudgetController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;
	private readonly IValidator<CreateBudget> createBudgetValidator;
	private readonly IValidator<GetBudgets> getBudgetsValidator;
	private readonly IValidator<EditBudget> editBudgetValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createBudgetValidator">Create budget validator.</param>
	/// <param name="getBudgetsValidator">Get budgets validator.</param>
	/// <param name="editBudgetValidator">Edit budget validator. </param>
	public BudgetController(ICommandBus commandBus, IQueryBus queryBus, IValidator<CreateBudget> createBudgetValidator, IValidator<GetBudgets> getBudgetsValidator, IValidator<EditBudget> editBudgetValidator)
	{
		this.createBudgetValidator = createBudgetValidator;
		this.getBudgetsValidator = getBudgetsValidator;
		this.editBudgetValidator = editBudgetValidator;
		this.commandBus = commandBus;
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Get Budgets.
	/// </summary>
	/// <param name="request">Query parameters.</param>
	/// <returns>Paged and sorted list of budgets.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST
	///     {
	///         "pageSize": 10,
	///         "pageIndex": 1,
	///         "search": "in",
	///         "sortDescriptors": [
	///         {
	///             "columnName": "name",
	///             "sortAscending": true
	///         }
	///       ]
	///     }
	/// .</remarks>
	/// <response code="200">Returns the list of budgets corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	[HttpPost]
	[Route("list")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBudgets([FromBody] GetBudgets request)
	{
		var validationResult = await this.getBudgetsValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetBudgets, PagedList<BudgetInfo>>(request);
			return this.Ok(pagedList);
		}

		throw new AppException("One or more error occured when trying to get Budgets.", validationResult.Errors);
	}

	/// <summary>
	/// Creates Budget.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST
	///     {
	///       "id": "3e6ca5f0-5ef8-44bc-a8bc-175c826b39b4",
	///       "name": "budgetName",
	///       "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///       "limit": {
	///         "value": 15,
	///         "currency": 1
	///       },
	///       "period": {
	///         "startDate": "2023-04-20T19:14:20.152Z",
	///         "endDate": "2023-04-25T20:14:20.152Z"
	///       },
	///       "description": "some budget description",
	///       "iconName": "yellowIcon"
	///     }
	///
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> CreateBudget([FromBody] CreateBudget request)
	{
		var validationResult = await this.createBudgetValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(request);
			return this.Created($"Budget/{request.Id}", request.Id);
		}

		throw new AppException("One or more error occured when trying to create Budget.", validationResult.Errors);
	}

	/// <summary>
	/// Edits Budget.
	/// </summary>
	/// <param name="id">Budget id. </param>
	/// <param name="request">Request.</param>
	/// <returns>Edited Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     PUT
	///     {
	///       "id": "3e6ca5f0-5ef8-44bc-a8bc-175c826b39b4",
	///       "name": "budgetName",
	///       "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///       "limit": {
	///         "value": 15,
	///         "currency": 1
	///       },
	///       "period": {
	///         "startDate": "2023-04-20T19:14:20.152Z",
	///         "endDate": "2023-04-25T20:14:20.152Z"
	///       },
	///       "description": "some budget description",
	///       "iconName": "yellowIcon"
	///     }
	///
	/// .</remarks>
	/// <response code="201">Returns the edited item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPut("{id:Guid}/edit")]
	public async Task<IActionResult> EditBudget([FromRoute] Guid id, [FromBody] EditBudget request)
	{
		var editedBudget = new EditBudget(id, request.Name, request.UserId, request.Limit, request.Period, request.Description, request.IconName);
		var validationResult = await this.editBudgetValidator.ValidateAsync(editedBudget);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(editedBudget);
			return this.Created($"Budget/{id}", id);
		}

		throw new AppException("One or more error occured when trying to edit Budget.", validationResult.Errors);
	}
}