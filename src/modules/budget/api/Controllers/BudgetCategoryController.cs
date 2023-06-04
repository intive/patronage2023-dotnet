using Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Errors;
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
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(TransactionCategoriesInfo), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
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
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost]
	[Route("{budgetId:guid}/categories")]
	public async Task<IActionResult> AddCategoryToBudget([FromRoute]Guid budgetId, [FromBody]GetTransactionCategory request)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		var command = new AddCategory(new BudgetId(budgetId), request.Icon, request.Name);
		await this.commandBus.Send(command);
		return this.Ok(request.Name);
	}

	/// <summary>
	/// Deletes a transaction category from a budget.
	/// </summary>
	/// <param name="budgetId">The ID of the budget from which to delete the transaction category.</param>
	/// <param name="budgetCategoryId">The id of the category to delete.</param>
	/// <returns>A Task representing the asynchronous operation that returns an IActionResult.</returns>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
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
		return this.Ok(budgetCategoryId);
	}
}