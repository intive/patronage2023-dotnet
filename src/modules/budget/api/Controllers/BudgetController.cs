using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Application.Budget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingTransaction;
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
[Route("[controller]")]
public class BudgetController : ControllerBase
{
	private readonly ICommandBus commandBus;
	private readonly IQueryBus queryBus;
	private readonly IValidator<CreateBudget> createBudgetValidator;
	private readonly IValidator<GetBudgets> getBudgetsValidator;
	private readonly IValidator<CreateTransaction> createTransactionValidator;
	private readonly IValidator<GetTransaction> getTransactionValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createBudgetValidator">Create Budget validator.</param>
	/// <param name="getBudgetsValidator">Get Budgets validator.</param>
	/// <param name="createTransactionValidator">Create Transaction validator.</param>
	/// <param name="getTransactionValidator">Get Transaction validator.</param>
	public BudgetController(ICommandBus commandBus, IQueryBus queryBus, IValidator<CreateBudget> createBudgetValidator, IValidator<GetBudgets> getBudgetsValidator, IValidator<CreateTransaction> createTransactionValidator, IValidator<GetTransaction> getTransactionValidator)
	{
		this.createBudgetValidator = createBudgetValidator;
		this.getBudgetsValidator = getBudgetsValidator;
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.createTransactionValidator = createTransactionValidator;
		this.getTransactionValidator = getTransactionValidator;
	}

	/// <summary>
	/// Get Budgets.
	/// </summary>
	/// <param name="request">Query parameters.</param>
	/// <returns>Paged list of Budgets.</returns>
	/// <response code="200">Returns the list of Budgets corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet]
	[ProducesResponseType(typeof(PagedList<BudgetInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBudgets([FromQuery] GetBudgets request)
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
	///        "Id" : "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///        "Name": "Budget"
	///     }
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
	/// Creates Income / Expanse Transaction.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     POST
	///     {
	///        "type": 1,
	///        "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///        "budgetId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///        "name": "string",
	///        "value": 1,
	///        "createdOn": "2023-04-16T11:51:17.820Z",
	///        "category": 1
	///     }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost("Create New Transaction")]
	public async Task<IActionResult> CreateNewTransaction([FromBody] CreateTransaction request)
	{
		var validationResult = await this.createTransactionValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(request);
			return this.Created($"Transaction/{request.TransactionId.Value}", request.TransactionId.Value);
		}

		throw new AppException("One or more error occured when trying to create Transaction.", validationResult.Errors);
	}

	/// <summary>
	/// Get Transactions.
	/// </summary>
	/// <param name="request">Query parameters.</param>
	/// <returns>Budget details, list of incomes and expanses.</returns>
	/// <response code="200">Returns the list of Budget details, list of incomes and expanses corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet("Get Budget With Details")]
	[ProducesResponseType(typeof(PagedList<BudgetInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetTransactionByBudgetId([FromQuery] GetTransaction request)
	{
		var validationResult = await this.getTransactionValidator.ValidateAsync(request);
		if (validationResult.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetTransaction, PagedList<TransactionInfo>>(request);
			return this.Ok(pagedList);
		}

		throw new AppException("One or more error occured when trying to get Transactions.", validationResult.Errors);
	}
}