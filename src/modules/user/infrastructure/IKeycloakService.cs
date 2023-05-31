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
	/// Extract access token.
	/// </summary>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>Access token.</returns>
	Task<string> ExtractAccessTokenFromClientToken(CancellationToken cancellationToken);

	/// <summary>
	/// Add new user to keycloak.
	/// </summary>
	/// <param name="appUser">User to add.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	Task<HttpResponseMessage> AddUser(AppUser appUser, CancellationToken cancellationToken);

	/// <summary>
	/// Get users from keycloak.
	/// </summary>
	/// <param name="searchText">Search text(string contained in username, first or last name, or email.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>List of users corresponding to query.</returns>
	Task<HttpResponseMessage> GetUsers(string searchText, CancellationToken cancellationToken);

	/// <summary>
	/// Get user from keycloak.
	/// </summary>
	/// <param name="id">User id.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>User corresponding to query.</returns>
	Task<HttpResponseMessage> GetUserById(string id, CancellationToken cancellationToken);

	/// <summary>
	/// Get appuser from keycloak.
	/// </summary>
	/// <param name="id">User id.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>User corresponding to query.</returns>
	Task<string> GetUsernameById(string id, CancellationToken cancellationToken);
}