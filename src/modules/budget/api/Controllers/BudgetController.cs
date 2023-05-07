using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Errors;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Microsoft.AspNetCore.Mvc;
using Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;
#pragma warning disable IDE0005
using Intive.Patronage2023.Modules.Budget.Application;
#pragma warning restore IDE0005
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;

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
	private readonly IValidator<CreateBudgetTransaction> createTransactionValidator;
	private readonly IValidator<GetBudgetTransactions> getBudgetTransactionValidator;
	private readonly IValidator<GetBudgetDetails> getBudgetDetailsValidator;
	private readonly IValidator<RemoveBudget> removeBudgetValidator;
	private readonly PermissionsService permissionsService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createBudgetValidator">Create Budget validator.</param>
	/// <param name="getBudgetsValidator">Get Budgets validator.</param>
	/// <param name="createTransactionValidator">Create Transaction validator.</param>
	/// <param name="getBudgetTransactionValidator">Get Budget Transaction validator.</param>
	/// <param name="getBudgetDetailsValidator">Get budget details validator.</param>
	/// <param name="removeBudgetValidator">Remove budget validator.</param>
	/// <param name="permissionsService">Permissions Service.</param>
	public BudgetController(
		ICommandBus commandBus,
		IQueryBus queryBus,
		IValidator<CreateBudget> createBudgetValidator,
		IValidator<GetBudgets> getBudgetsValidator,
		IValidator<CreateBudgetTransaction> createTransactionValidator,
		IValidator<GetBudgetTransactions> getBudgetTransactionValidator,
		IValidator<GetBudgetDetails> getBudgetDetailsValidator,
		IValidator<RemoveBudget> removeBudgetValidator,
		PermissionsService permissionsService)
	{
		this.createBudgetValidator = createBudgetValidator;
		this.getBudgetsValidator = getBudgetsValidator;
		this.getBudgetDetailsValidator = getBudgetDetailsValidator;
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.createTransactionValidator = createTransactionValidator;
		this.getBudgetTransactionValidator = getBudgetTransactionValidator;
		this.removeBudgetValidator = removeBudgetValidator;
		this.permissionsService = permissionsService;
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
	/// Get budget details.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Budget details.</returns>
	/// <response code="200">Returns details of budget with given id.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	/// <response code="404">If budget with given id does not exist.</response>
	[HttpGet("{Id:guid}")]
	[ProducesResponseType(typeof(BudgetDetailsInfo), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetBudgetDetails([FromRoute] GetBudgetDetails request)
	{
		var validationResult = await this.getBudgetDetailsValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			if (this.permissionsService.IsPermission(new BudgetId(request.Id)))
			{
				var result = await this.queryBus.Query<GetBudgetDetails, BudgetDetailsInfo?>(request);
				if (result is null)
				{
					return this.NotFound();
				}

				return this.Ok(result);
			}

			return this.Forbid();
		}

		throw new AppException("One or more error occured when trying to create Budget.", validationResult.Errors);
	}

	/// <summary>
	/// Creates Budget.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
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
			return this.Created(string.Empty, request.Id);
		}

		throw new AppException("One or more error occured when trying to create Budget.", validationResult.Errors);
	}

	/// <summary>
	/// Remove Budget and its related Transactions.
	/// </summary>
	/// <param name="budgetId">The Id of the budget to remove.</param>
	/// <returns>
	/// Returns an HTTP 200 OK status code with the ID of the removed budget if successful.
	/// Throws an AppException if there are validation errors.
	/// </returns>
	/// <response code="200">Returns Id of removed budget.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpDelete("{budgetId:guid}")]
	public async Task<IActionResult> RemoveBudget([FromRoute] Guid budgetId)
	{
		var removeBudget = new RemoveBudget(budgetId);

		var validationResult = await this.removeBudgetValidator.ValidateAsync(removeBudget);
		if (validationResult.IsValid)
		{
			if (this.permissionsService.IsPermission(new BudgetId(budgetId), UserRole.BudgetOwner))
			{
				await this.commandBus.Send(removeBudget);
				return this.Ok(removeBudget.Id);
			}

			return this.Forbid();
		}

		throw new AppException("One or more error occured when trying to delete Budget.", validationResult.Errors);
	}

	/// <summary>
	/// Creates Income / Expense Budget Transaction.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="command">Command that creates Transaction to specific budget.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	/// Types: "Income" , "Expense"
	///
	/// Value must be positive for income or negative for expense.
	///
	/// Categories: "HomeSpendings" ,  "Subscriptions" , "Car" , "Grocery" ,
	///
	///     POST
	///     {
	///         "type": "Income",
	///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///         "name": "string",
	///         "value": 1,
	///         "category": "HomeSpendings",
	///         "transactionDate": "2023-06-20T14:15:47.392Z"
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost("{budgetId:guid}/transaction")]
	public async Task<IActionResult> CreateNewTransaction([FromRoute] Guid budgetId, [FromBody] CreateTransaction command)
	{
		var transactionId = command.Id == default ? Guid.NewGuid() : command.Id;
		var transactionDate = command.TransactionDate == DateTime.MinValue ? DateTime.UtcNow : command.TransactionDate;
		var newBudgetTransaction = new CreateBudgetTransaction(command.Type, transactionId, budgetId, command.Name, command.Value, command.Category, transactionDate);

		var validationResult = await this.createTransactionValidator.ValidateAsync(newBudgetTransaction);
		if (validationResult.IsValid)
		{
			if (this.permissionsService.IsPermission(new BudgetId(budgetId)))
			{
				await this.commandBus.Send(newBudgetTransaction);
				return this.Created(string.Empty, newBudgetTransaction.Id);
			}

			return this.Forbid();
		}

		throw new AppException("One or more error occured when trying to create Budget Transaction.", validationResult.Errors);
	}

	/// <summary>
	/// Get Budget by id with transactions.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="request">Query parameters.</param>
	/// <returns>Budget details, list of incomes and Expenses.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     {
	///         "pageSize": 1,
	///         "pageIndex": 1
	///     }
	/// .</remarks>
	/// <response code="200">Returns the list of Budget details, list of incomes and Expenses corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpPost("{budgetId:guid}/transactions")]
	[ProducesResponseType(typeof(PagedList<BudgetInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetTransactionByBudgetId([FromRoute] Guid budgetId, [FromBody] GetBudgetTransactionsPaginationInfo request)
	{
		var getBudgetTransactions = new GetBudgetTransactions
		{
			BudgetId = new BudgetId(budgetId),
			PageSize = request.PageSize,
			PageIndex = request.PageIndex,
		};
		var validationResult = await this.getBudgetTransactionValidator.ValidateAsync(getBudgetTransactions);
		if (validationResult.IsValid)
		{
			if (this.permissionsService.IsPermission(new BudgetId(budgetId)))
			{
				var pagedList = await this.queryBus.Query<GetBudgetTransactions, PagedList<BudgetTransactionInfo>>(getBudgetTransactions);
				return this.Ok(pagedList);
			}

			return this.Forbid();
		}

		throw new AppException("One or more error occured when trying to get Transactions.", validationResult.Errors);
	}
}