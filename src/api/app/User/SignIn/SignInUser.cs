using Intive.Patronage2023.Api.Keycloak;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Api.User.SignIn;

/// <summary>
/// SignIn user query.
/// </summary>
/// <param name="Email">User email which user provides.</param>
/// <param name="Password">Password which user provides.</param>
public record SignInUser(string Email, string Password) : IQuery<HttpResponseMessage>;

/// <summary>
/// Handles a sign-in request from a user.
/// </summary>
/// <remarks>
/// This class implements the <see cref="IQueryHandler{TQuery,TResult}"/> interface to handle a sign-in request
/// from a user. It sends the sign-in request to the authentication service (Keycloak) and returns an HTTP response that
/// includes a JWT token if the sign-in was successful.
/// </remarks>
public class HandleSignInUser : IQueryHandler<SignInUser, HttpResponseMessage>
{
	private readonly KeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="keycloakService">KeycloakService.</param>
	public HandleSignInUser(KeycloakService keycloakService)
	{
		this.keycloakService = keycloakService;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignInUser"/> class.
	/// </summary>
	/// <param name="request">User email and password.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage with JSON Web Token.</returns>
	public async Task<HttpResponseMessage> Handle(SignInUser request, CancellationToken cancellationToken)
	{
		return await this.keycloakService.SignInGetToken(request.Email, request.Password, cancellationToken);
	}
}