using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Application.Budget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingTransaction;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Errors;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;
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
	private readonly IValidator<CreateBudgetTransaction> createTransactionValidator;
	private readonly IValidator<GetBudgetTransaction> getBudgetTransactionValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="createBudgetValidator">Create Budget validator.</param>
	/// <param name="getBudgetsValidator">Get Budgets validator.</param>
	/// <param name="createTransactionValidator">Create Transaction validator.</param>
	/// <param name="getBudgetTransactionValidator">Get Budget Transaction validator.</param>
	public BudgetController(ICommandBus commandBus, IQueryBus queryBus, IValidator<CreateBudget> createBudgetValidator, IValidator<GetBudgets> getBudgetsValidator, IValidator<CreateBudgetTransaction> createTransactionValidator, IValidator<GetBudgetTransaction> getBudgetTransactionValidator)
	{
		this.createBudgetValidator = createBudgetValidator;
		this.getBudgetsValidator = getBudgetsValidator;
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.createTransactionValidator = createTransactionValidator;
		this.getBudgetTransactionValidator = getBudgetTransactionValidator;
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
	/// Creates Income / Expense Budget Transaction.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Created Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	/// Types: "Income" , "Expense"
	///
	/// Categories: "HomeSpendings" ,  "Subscriptions" , "Car" , "Grocery" ,
	///
	///    {
	///        "type": "Income",
	///        "transactionId": {
	///           "value": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
	///        },
	///        "budgetId": {
	///           "value": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
	///        },
	///        "name": "string",
	///        "value": 1,
	///        "category": "HomeSpendings",
	///        "transactionDate": "2023-06-20T14:15:47.392Z"
	///    }
	/// .</remarks>
	/// <response code="201">Returns the newly created item.</response>
	/// <response code="400">If the body is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPost("Transaction/Create")]
	public async Task<IActionResult> CreateNewTransaction([FromBody] CreateBudgetTransaction request)
	{
		var isTransactionIdGenerated = request.Id.Value == Guid.Empty ? Guid.NewGuid() : request.Id.Value;
		var transactionId = new TransactionId(isTransactionIdGenerated);

		var transactionDate = request.TransactionDate == DateTime.MinValue ? DateTime.UtcNow : request.TransactionDate;

		var newBudgetTransaction = new CreateBudgetTransaction(request.Type, transactionId, request.BudgetId, request.Name, request.Value, request.Category, transactionDate);
		var validationResult = await this.createTransactionValidator.ValidateAsync(newBudgetTransaction);
		if (validationResult.IsValid)
		{
			await this.commandBus.Send(newBudgetTransaction);
			return this.Created($"Transaction/{newBudgetTransaction.Id}", newBudgetTransaction.Id);
		}

		throw new AppException("One or more error occured when trying to create Budget Transaction.", validationResult.Errors);
	}

	/// <summary>
	/// Get Budget by id with transactions.
	/// </summary>
	/// <param name="id">Query parameters.</param>
	/// <returns>Budget details, list of incomes and Expenses.</returns>
	/// <response code="200">Returns the list of Budget details, list of incomes and Expenses corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpGet("Transactions")]
	[ProducesResponseType(typeof(PagedList<BudgetInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetTransactionByBudgetId([FromQuery] Guid id)
	{
		var budgetId = new BudgetId(id);
		var result = new GetBudgetTransaction(budgetId);

		var validationResult = await this.getBudgetTransactionValidator.ValidateAsync(result);
		if (validationResult.IsValid)
		{
			var pagedList = await this.queryBus.Query<GetBudgetTransaction, PagedList<BudgetTransactionInfo>>(result);
			return this.Ok(pagedList);
		}

		throw new AppException("One or more error occured when trying to get Transactions.", validationResult.Errors);
	}
}