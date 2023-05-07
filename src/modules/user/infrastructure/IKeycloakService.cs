using Intive.Patronage2023.Modules.User.Domain;

namespace Intive.Patronage2023.Modules.User.Infrastructure;

/// <summary>
/// Interface for KeycloakService which provides server and users management.
/// </summary>
public interface IKeycloakService
{
	/// <summary>
	/// SignInGetToken method.
	/// </summary>
	/// <param name="email">User email.</param>
	/// <param name="password">User password.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage object.</returns>
	Task<HttpResponseMessage> SignInGetToken(string email, string password, CancellationToken cancellationToken);

	/// <summary>
	/// Get client token method.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage object.</returns>
	Task<HttpResponseMessage> GetClientToken(CancellationToken cancellationToken);

	/// <summary>
	/// Add new user to keycloak.
	/// </summary>
	/// <param name="appUser">User to add.</param>
	/// <param name="accessToken">Client token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	Task<HttpResponseMessage> AddUser(AppUser appUser, string accessToken, CancellationToken cancellationToken);

	/// <summary>
	/// Get users from keycloak.
	/// </summary>
	/// <param name="searchText">Search text(string contained in username, first or last name, or email.</param>
	/// <param name="accessToken">Client token.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>List of users corresponding to query.</returns>
	Task<HttpResponseMessage> GetUsers(string searchText, string accessToken, CancellationToken cancellationToken);
}