using Intive.Patronage2023.Modules.User.Application.User;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers;

/// <summary>
/// Get users query.
/// </summary>>
public record GetUsers() : IQuery<PagedList<UserInfo>>, IPageableQuery, ITextSearchQuery
{
	/// <inheritdoc/>
	public int PageSize { get; set; }

	/// <inheritdoc/>
	public int PageIndex { get; set; }

	/// <inheritdoc/>
	public string? Search { get; set; }
}

/// <summary>
/// Get Users handler.
/// </summary>
public class GetUsersQueryHandler : IQueryHandler<GetUsers, PagedList<UserInfo>>
{
	/// <summary>
	/// Keycloak service.
	/// </summary>
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
	/// </summary>
	/// <param name = "keycloakService" > KeycloakService.</param>
	public GetUsersQueryHandler(IKeycloakService keycloakService) => this.keycloakService = keycloakService;

	/// <summary>
	/// GetUsers query handler.
	/// </summary>
	/// <param name="query">Query.</param>
	/// <param name="cancellationToken">cancellation token.</param>
	/// <returns>Paged list of users.</returns>
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

		response = await this.keycloakService.GetUsers(query.PageSize, query.PageIndex, query.Search, token.AccessToken, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		var deserializedUsers = JsonConvert.DeserializeObject<List<UserInfo>>(responseContent);

		return new PagedList<UserInfo>
		{
			Items = deserializedUsers!,
			TotalCount = deserializedUsers!.Count,
		};
	}
}