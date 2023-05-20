using Intive.Patronage2023.Modules.User.Application.GettingUsers.Extensions;
using Intive.Patronage2023.Modules.User.Application.GettingUsers.Mappers;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.User.Application.GettingUsers;

/// <summary>
/// Get users query.
/// </summary>>
public record GetUsers() : IQuery<PagedList<UserInfoDto>>, IPageableQuery, ITextSearchQuery, ISortableQuery
{
	/// <inheritdoc/>
	public int PageSize { get; set; }

	/// <inheritdoc/>
	public int PageIndex { get; set; }

	/// <inheritdoc/>
	public string? Search { get; set; }

	/// <inheritdoc/>
	public List<SortDescriptor> SortDescriptors { get; set; } = null!;
}

/// <summary>
/// Get Users handler.
/// </summary>
public class GetUsersQueryHandler : IQueryHandler<GetUsers, PagedList<UserInfoDto>>
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
	public async Task<PagedList<UserInfoDto>> Handle(GetUsers query, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetUsers(query.Search ?? string.Empty, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException(response.ToString());
		}

		string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		var deserializedUsers = JsonConvert.DeserializeObject<List<UserInfo>>(responseContent);
		int totalCount = deserializedUsers!.Count();

		var orderedUsers = deserializedUsers!.Sort(query.SortDescriptors);
		deserializedUsers = orderedUsers!.Skip((query.PageIndex - 1) * query.PageSize).Take(query.PageSize).ToList();

		var dtoUsers = deserializedUsers.Select(UserInfoUserInfoDtoMapper.Map).ToList();

		return new PagedList<UserInfoDto>
		{
			Items = dtoUsers!,
			TotalCount = totalCount,
		};
	}
}