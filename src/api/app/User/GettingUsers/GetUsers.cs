using Intive.Patronage2023.Api.Keycloak;
using Intive.Patronage2023.Modules.Budget.Application.Budget;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Api.User.GettingUsers;

/// <summary>
/// Get users query.
/// </summary>>
public record GetUsers() : IQuery<PagedList<UserInfo>>, IPageableQuery, ITextSearchQuery, ISortableQuery
{
	/// <summary>
	/// The amount of data to return.
	/// </summary>
	public int PageSize { get; set; }

	/// <summary>
	/// Requested page.
	/// </summary>
	public int PageIndex { get; set; }

	/// <summary>
	/// Field to search budget by name.
	/// </summary>
	public string Search { get; set; } = null!;

	/// <summary>
	/// List of criteria to sort budgets.
	/// </summary>
	public List<SortDescriptor> SortDescriptors { get; set; } = null!;
}

/// <summary>
/// Get Users handler.
/// </summary>
public class GetUsersQueryHandler : IQueryHandler<GetUsers, PagedList<UserInfo>>
{
	/// <summary>
	/// Keycloak service.
	/// </summary>
	private readonly KeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
	/// </summary>
	/// <param name = "keycloakService" > KeycloakService.</param>
	public GetUsersQueryHandler(KeycloakService keycloakService) => this.keycloakService = keycloakService;

	/// <summary>
	/// GetUsers query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of Budgets.</returns>
	public async Task<PagedList<UserInfo>> Handle(GetUsers query, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetClientToken(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		if (string.IsNullOrEmpty(responseContent))
		{
			throw new AppException(response.ToString());
		}

		Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

		if (token == null || token?.AccessToken == null)
		{
			throw new AppException(response.ToString());
		}

		response = await this.keycloakService.GetUsers(token.AccessToken, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		var deserializedUsers = JsonConvert.DeserializeObject<List<UserInfo>>(responseContent);

		return new PagedList<UserInfo>
		{
			Items = deserializedUsers!,
			TotalCount = 17,
		};
	}
}