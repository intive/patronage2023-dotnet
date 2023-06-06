using Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Budget.Api.Controllers;

/// <summary>
/// Controller that contains endpoints managing Budget Categories.
/// </summary>
[ApiController]
[Route("budgets")]
public class BudgetCategoryController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;
	private readonly IAuthorizationService authorizationService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetCategoryController"/> class.
	/// </summary>
	/// <param name="commandBus">Bus that managed persisting changes in database.</param>
	/// <param name="queryBus">Bus that get data from database.</param>
	/// <param name="authorizationService">An instance of the AuthorizationService class that provides authorization functionality.</param>
	public BudgetCategoryController(
		ICommandBus commandBus,
		IQueryBus queryBus,
		IAuthorizationService authorizationService)
	{
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.authorizationService = authorizationService;
	}

	/// <summary>
	/// Retrieves the budget transaction categories list.
	/// </summary>
	/// <param name="budgetId">The ID of the budget for which to retrieve the transaction categories.</param>
	/// <response code="200">Returns List of the Budget Transaction Categories.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	/// <response code="403">If the user is forbidden to do this action.</response>
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(TransactionCategoriesInfo), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
	[HttpGet]
	[Route("{budgetId:guid}/categories")]
	public async Task<IActionResult> GetBudgetCategories([FromRoute]Guid budgetId)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Read)).Succeeded)
		{
			return this.Forbid();
		}

		var query = new GetTransactionCategories(new BudgetId(budgetId));
		var categories = this.queryBus.Query<GetTransactionCategories, TransactionCategoriesInfo>(query);
		return this.Ok(categories.Result);
	}

	/// <summary>
	/// Adds a transaction category to a budget.
	/// </summary>
	/// <param name="budgetId">The ID of the budget to which the category will be added.</param>
	/// <param name="request">The transaction category name.</param>
	/// <remarks>
	///     {
	///         "icon": {
	///             "iconName": "string",
	///             "foreground": "#643400",
	///             "background": "#643400",
	///         },
	///         "name": "string"
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	/// <response code="403">If the user is forbidden to do this action.</response>
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
	[HttpPost]
	[Route("{budgetId:guid}/categories")]
	public async Task<IActionResult> AddCategoryToBudget([FromRoute]Guid budgetId, [FromBody]AddCategory request)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		var categoryId = new TransactionCategoryId(Guid.NewGuid());
		var command = new AddTransactionCategory(categoryId, new BudgetId(budgetId), request.Icon, new CategoryType(request.Name));
		await this.commandBus.Send(command);
		return this.Created(string.Empty, categoryId.Value);
	}

	/// <summary>
	/// Deletes a transaction category from a budget.
	/// </summary>
	/// <param name="budgetId">The ID of the budget from which to delete the transaction category.</param>
	/// <param name="budgetCategoryId">The id of the category to delete.</param>
	/// <response code="204">Returns no content if category is deleted correctly.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	/// <response code="403">If the user is forbidden to do this action.</response>
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
	[HttpDelete]
	[Route("{budgetId:guid}/categories/{budgetCategoryId:guid}")]
	public async Task<IActionResult> DeleteTransactionCategoryFromBudget([FromRoute]Guid budgetId, [FromRoute]Guid budgetCategoryId)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		var command = new DeleteTransactionCategory(new TransactionCategoryId(budgetCategoryId), new BudgetId(budgetId));
		await this.commandBus.Send(command);
		return this.NoContent();
	}
}