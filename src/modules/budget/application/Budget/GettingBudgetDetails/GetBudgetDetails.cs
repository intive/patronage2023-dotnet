using System.Net.Http.Json;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Mappers;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.User.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Intive.Patronage2023.Shared.Abstractions.UserContext;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;

/// <summary>
/// Get budget details query.
/// </summary>
public record GetBudgetDetails() : IQuery<BudgetDetailsInfo?>
{
	/// <summary>
	/// Budget id to retrive details for.
	/// </summary>
	public Guid Id { get; set; }
}

/// <summary>
/// Get Budget details handler.
/// </summary>
public class GetBudgetDetailsQueryHandler : IQueryHandler<GetBudgetDetails, BudgetDetailsInfo?>
{
	private readonly BudgetDbContext budgetDbContext;
	private readonly IKeycloakService keycloakService;
	private readonly IExecutionContextAccessor contextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetDetailsQueryHandler"/> class.
	/// </summary>
	/// <param name="budgetDbContext">Budget dbContext.</param>
	/// <param name="keycloakService">Keycloak service.</param>
	/// <param name="contextAccessor">IExecutionContextAccessor.</param>
	public GetBudgetDetailsQueryHandler(BudgetDbContext budgetDbContext, IKeycloakService keycloakService, IExecutionContextAccessor contextAccessor)
	{
		this.budgetDbContext = budgetDbContext;
		this.keycloakService = keycloakService;
		this.contextAccessor = contextAccessor;
	}

	/// <summary>
	/// GetBudgetDetails query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>BudgetDetailsInfo or null.</returns>
	public async Task<BudgetDetailsInfo?> Handle(GetBudgetDetails query, CancellationToken cancellationToken)
	{
		var budgetId = new BudgetId(query.Id);
		var budget = await this.budgetDbContext.Budget.FindAsync(new object?[] { budgetId }, cancellationToken: cancellationToken);

		var budgetUsers = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId == budgetId)
			.Select(x => x.UserId.Value)
			.ToListAsync(cancellationToken: cancellationToken);

		var tasks = await Task.WhenAll(budgetUsers.Select(x => this.keycloakService.GetUserById(x.ToString(), cancellationToken)
			.ContinueWith(x => this.DeserializeResponse(x.Result, cancellationToken))).ToArray());

		var usersBudget = tasks.Select(x => x.Result).ToArray();

		var userId = new UserId(this.contextAccessor.GetUserId()!.Value);

		bool isFavourite = await this.budgetDbContext.UserBudget
			.Where(x => x.BudgetId == budgetId && x.UserId == userId)
			.Select(x => x.IsFavourite)
			.FirstOrDefaultAsync();

		return budget?.MapToDetailsInfo(usersBudget!, isFavourite);
	}

	/// <summary>
	/// Deserialize json with all user info to budget details needed info.
	/// </summary>
	/// <param name="response">Http response meessage.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Budget user information.</returns>
	/// <exception cref="AppException">App Exception.</exception>
	private async Task<BudgetUser> DeserializeResponse(HttpResponseMessage response, CancellationToken cancellationToken)
	{
		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		var userInfo = await response.Content.ReadFromJsonAsync<UserInfo>();

		if (userInfo == null)
		{
			throw new AppException("One or more error occured when trying to get user info.");
		}

		var budgetUser = new BudgetUser(userInfo.Id, userInfo.Attributes?.Avatar[0] ?? string.Empty, userInfo.FirstName, userInfo.LastName, userInfo.Email);

		return budgetUser;
	}
}