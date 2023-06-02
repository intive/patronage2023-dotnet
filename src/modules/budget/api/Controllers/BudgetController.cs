using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CancelBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistic;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistics;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.UpdateUserBudgetFavourite;
using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Errors;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

using Microsoft.AspNetCore.Mvc;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgets;
using Intive.Patronage2023.Shared.Infrastructure.Domain;

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
	private readonly IAuthorizationService authorizationService;
	private readonly IExecutionContextAccessor contextAccessor;
	private readonly IBudgetExportService budgetExportService;
	private readonly IBudgetImportService budgetImportService;
	private readonly IValidator<AddUsersToBudget> addUsersToBudgetValidator;
	private readonly IValidator<UpdateUserBudgetFavourite> updateUserBudgetFavouriteValidator;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="commandBus">Command bus.</param>
	/// <param name="queryBus">Query bus.</param>
	/// <param name="authorizationService">IAuthorizationService.</param>
	/// <param name="usersIdsValidator">User ids validator.</param>
	/// <param name="updateUserBudgetFavouriteValidator">Update UserBudget favuorite flag validator.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	/// <param name="budgetExportService">BudgetExportService.</param>
	/// <param name="budgetImportService">BudgetImportService.</param>
	public BudgetController(
		ICommandBus commandBus,
		IQueryBus queryBus,
		IAuthorizationService authorizationService,
		IValidator<AddUsersToBudget> usersIdsValidator,
		IValidator<UpdateUserBudgetFavourite> updateUserBudgetFavouriteValidator,
		IExecutionContextAccessor contextAccessor,
		IBudgetExportService budgetExportService,
		IBudgetImportService budgetImportService)
	{
		this.commandBus = commandBus;
		this.queryBus = queryBus;
		this.authorizationService = authorizationService;
		this.updateUserBudgetFavouriteValidator = updateUserBudgetFavouriteValidator;
		this.contextAccessor = contextAccessor;
		this.addUsersToBudgetValidator = usersIdsValidator;
		this.budgetExportService = budgetExportService;
		this.budgetImportService = budgetImportService;
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
		var pagedList = await this.queryBus.Query<GetBudgets, PagedList<BudgetInfo>>(request);
		return this.Ok(pagedList);
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
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(request.Id), Operations.Read)).Succeeded)
		{
			return this.Forbid();
		}

		var result = await this.queryBus.Query<GetBudgetDetails, BudgetDetailsInfo?>(request);
		if (result is null)
		{
			return this.NotFound();
		}

		return this.Ok(result);
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
		var budgetId = request.Id == default ? Guid.NewGuid() : request.Id;
		var userId = request.UserId == default ? this.contextAccessor.GetUserId()!.Value : request.UserId;
		var newBudget = new CreateBudget(budgetId, request.Name, userId, request.Limit, request.Period, request.Description, request.IconName);

		await this.commandBus.Send(newBudget);
		return this.Created(string.Empty, budgetId);
	}

	/// <summary>
	/// Edits Budget.
	/// </summary>
	/// <param name="id">Budget id.</param>
	/// <param name="request">Request.</param>
	/// <returns>Edited Result.</returns>
	/// <remarks>
	/// Sample request:
	///
	///     {
	///       "name": "budgetName",
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
	public async Task<IActionResult> EditBudget([FromRoute] Guid id, [FromBody] EditBudgetDetails request)
	{
		var editedBudget = new EditBudget(new BudgetId(id), request.Name, request.Period, request.Description, request.IconName);

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(id), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		await this.commandBus.Send(editedBudget);
		return this.Created($"Budget/{id}/edit", id);
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
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpDelete("{budgetId:guid}")]
	public async Task<IActionResult> RemoveBudget([FromRoute] Guid budgetId)
	{
		var removeBudget = new RemoveBudget(budgetId);

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		await this.commandBus.Send(removeBudget);
		return this.Ok(removeBudget.Id);
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
	/// Categories: "HomeSpendings" ,  "Subscriptions" , "Car" , "Grocery" , "Salary" , "Refund"
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

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Create)).Succeeded)
		{
			return this.Forbid();
		}

		await this.commandBus.Send(newBudgetTransaction);
		return this.Created(string.Empty, newBudgetTransaction.Id);
	}

	/// <summary>
	/// Cancel transaction by Id.
	/// </summary>
	/// <param name="budgetId">Id of the budget for which the given transaction will be canceled.</param>
	/// <param name="transactionId">The Id of the transaction to cancel.</param>
	/// <returns>
	/// Returns an HTTP 200 OK status code with the ID of the cancelled transaction if successful.
	/// Throws an AppException if there are validation errors.
	/// </returns>
	/// <response code="200">Returns Id of removed budget.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[HttpPut("{budgetId:guid}/transactions/{transactionId:guid}/cancel")]
	public async Task<IActionResult> CancelBudgetTransaction([FromRoute] Guid budgetId, [FromRoute] Guid transactionId)
	{
		var cancelBudgetTransaction = new CancelBudgetTransaction(transactionId, budgetId);

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		await this.commandBus.Send(cancelBudgetTransaction);
		return this.Ok(cancelBudgetTransaction.TransactionId);
	}

	/// <summary>
	/// Get Budget by id with transactions.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="request">Query parameters.</param>
	/// <returns>Budget details, list of incomes and Expenses.</returns>
	/// <remarks>
	/// Sample request:
	/// Types: "Income", "Expense"
	/// Categories: "HomeSpendings" ,  "Subscriptions" , "Car" , "Grocery" ,
	/// Set transactionType to null or don't include at all to get both types. Same with categoryTypes.
	///
	///     {
	///         "pageSize": 10,
	///         "pageIndex": 1,
	///         "transactionType": null,
	///         "categoryTypes": [
	///           "HomeSpendings",
	///           "Car"
	///         ]
	///         "search": "text"
	///     }
	/// .</remarks>
	/// <response code="200">Returns the list of Budget details, list of incomes and Expenses corresponding to the query.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpPost("{budgetId:guid}/transactions")]
	[ProducesResponseType(typeof(PagedList<BudgetInfo>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetTransactionByBudgetId([FromRoute] Guid budgetId, [FromBody] GetBudgetTransactionsQueryInfo request)
	{
		var getBudgetTransactions = new GetBudgetTransactions
		{
			BudgetId = new BudgetId(budgetId),
			PageSize = request.PageSize,
			PageIndex = request.PageIndex,
			TransactionType = request.TransactionType,
			CategoryTypes = request.CategoryTypes,
			Search = request.Search,
		};

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Read)).Succeeded)
		{
			return this.Forbid();
		}

		var pagedList = await this.queryBus.Query<GetBudgetTransactions, PagedList<BudgetTransactionInfo>>(getBudgetTransactions);
		return this.Ok(pagedList);
	}

	/// <summary>
	/// Get calculated values for budget between two dates.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="startDate">Start Date in which we want to get statistics.</param>
	/// <param name="endDate">End date in which we want to get statistics.</param>
	/// <remarks>
	/// Sample Id and Date Points:
	///
	///         "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
	///         "startDate": "2023-04-20T19:14:20.152Z",
	///         "endDate": "2023-04-25T20:14:20.152Z"
	/// .</remarks>
	/// <returns>Returns the BudgetStatistics which has List of calculated budget balance, between two dates with day on which calculation was made.
	/// It also contains TrendValue, PeriodValue and TotalBudgetValue. </returns>
	[HttpGet("{budgetId:guid}/statistics")]
	[ProducesResponseType(typeof(BudgetStatistics<BudgetAmount>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBudgetStatisticsBetweenDates([FromRoute] Guid budgetId, DateTime startDate, DateTime endDate)
	{
		var getBudgetStatistics = new GetBudgetStatistics
		{
			Id = budgetId,
			StartDate = startDate,
			EndDate = endDate,
		};

		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Read)).Succeeded)
		{
			return this.Forbid();
		}

		var pagedList = await this.queryBus.Query<GetBudgetStatistics, BudgetStatistics<BudgetAmount>>(getBudgetStatistics);
		return this.Ok(pagedList);
	}

	/// <summary>
	/// Get calculated values for  all budgets between two dates.
	/// </summary>
	/// <param name="startDate">Start Date in which we want to get report.</param>
	/// <param name="endDate">End Date in which we want to get report.</param>
	/// <param name="currency">Currency which we use to fillter budgets.</param>
	/// <remarks>
	/// Sample Date Points:
	///
	///         "startDate": "2023-04-20T19:14:20.152Z",
	///         "endDate": "2023-04-25T20:14:20.152Z"
	/// .</remarks>
	/// <returns>Returns the BudgetReport which has List of sumed Incomes, List of sumed Expenses, between two dates with day on which calculation was made.
	/// It also contains TrendValue, PeriodValue and TotalBudgetValue. </returns>
	[HttpGet("statistics")]
	[ProducesResponseType(typeof(BudgetsReport<BudgetAmount>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> GetBudgetsReport(DateTime startDate, DateTime endDate, Currency currency)
	{
		var getBudgetReport = new GetBudgetsReport
		{
			StartDate = startDate,
			EndDate = endDate,
			Currency = currency,
		};

		var budgetReport = await this.queryBus.Query<GetBudgetsReport, BudgetsReport<BudgetAmount>>(getBudgetReport);
		return this.Ok(budgetReport);
	}

	/// <summary>
	/// Update value of favourite flag.
	/// </summary>
	/// <param name="budgetId">Budget Id.</param>
	/// <param name="isFavourite">Is favourite flag value.</param>
	/// <returns>Task.</returns>
	[HttpPut("{budgetId:guid}/favourite")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> UpdateBudgetFavouriteFlag([FromRoute] Guid budgetId, [Required] bool isFavourite)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Read)).Succeeded)
		{
			return this.Forbid();
		}

		var updateFavourite = new UpdateUserBudgetFavourite(budgetId, isFavourite);

		var validationResult = await this.updateUserBudgetFavouriteValidator.ValidateAsync(updateFavourite);
		if (!validationResult.IsValid)
		{
			throw new AppException("One or more error occured when trying to update favourite flag.", validationResult.Errors);
		}

		await this.commandBus.Send(updateFavourite);

		return this.Ok();
	}

	/// <summary>
	/// Add users to budget by budget owner or admin.
	/// </summary>
	/// <param name="budgetId">Budget id.</param>
	/// <param name="usersIds">List of users ids to add to budget.</param>
	/// <returns>User of the budget.</returns>
	/// <response code="200">If users are added.</response>
	/// <response code="400">If the query is not valid.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[HttpPost("{budgetId:guid}/users")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status403Forbidden)]
	public async Task<IActionResult> AddUsersToBudget([FromRoute] Guid budgetId, [FromBody] Guid[] usersIds)
	{
		if (!(await this.authorizationService.AuthorizeAsync(this.User, new BudgetId(budgetId), Operations.Update)).Succeeded)
		{
			return this.Forbid();
		}

		var addUsersToBudget = new AddUsersToBudget(usersIds, budgetId);

		await this.addUsersToBudgetValidator.ValidateAndThrowAsync(addUsersToBudget);

		var addUserToBudget = usersIds.Select(userId => new AddUserBudget(
		Guid.NewGuid(), new UserId(userId), new BudgetId(budgetId), UserRole.BudgetUser)).ToList();

		var userBudgetList = new AddUserBudgetList(addUserToBudget);

		await this.commandBus.Send(userBudgetList);

		return this.Ok();
	}

	/// <summary>
	/// Exports all user budgets to Azure Blob Storage.
	/// </summary>
	/// <returns>A string containing the URI to Azure Blob Storage of the exported file.</returns>
	/// <response code="200">If the export operation was successful and budgets have been stored in Azure Blob Storage.</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status401Unauthorized)]
	[HttpGet("export")]
	public async Task<IActionResult> ExportBudgets()
	{
		var query = new GetBudgetsToExport();
		var budgets = await this.queryBus.Query<GetBudgetsToExport, GetBudgetTransferList?>(query);
		string? result = await this.budgetExportService.Export(budgets);

		return this.Ok(result);
	}

	/// <summary>
	/// Imports budgets from a provided .csv file.
	/// </summary>
	/// <param name="file">The .csv file containing the budgets to be imported.</param>
	/// <returns>An object containing a list of any errors encountered during the import process,
	/// and a string that contains either the URI of the saved budgets if the operation was successful, or an appropriate error message.</returns>
	/// <response code="200">If at least one budget from the imported file passed the validation and was successfully saved.
	/// Also contains a list of budgets that failed the validation.</response>
	/// <response code="400">If no budgets from the imported file passed the validation.
	/// The response will include a list of errors for each budget that failed validation, and information in uri "No budgets were saved.".</response>
	/// <response code="401">If the user is unauthorized.</response>
	[ProducesResponseType(typeof(ImportResult), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ImportResult), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ErrorExample), StatusCodes.Status401Unauthorized)]
	[HttpPost("import")]
	public async Task<IActionResult> ImportBudgets(IFormFile file)
	{
		var getImportResult = await this.budgetImportService.Import(file);
		await this.commandBus.Send(getImportResult.BudgetAggregateList);

		if (getImportResult.ImportResult.Uri != "No budgets were saved.")
		{
			return this.Ok(new { Errors = getImportResult.ImportResult.ErrorsList, getImportResult.ImportResult.Uri });
		}

		return this.BadRequest(new { Errors = getImportResult.ImportResult.ErrorsList, getImportResult.ImportResult.Uri });
	}
}