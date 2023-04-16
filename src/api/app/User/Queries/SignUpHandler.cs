using Intive.Patronage2023.Api.Keycloak;
using Intive.Patronage2023.Api.User.CreatingUser;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Api.User.Commands;

/// <summary>
/// Sign up request handler.
/// </summary>
public class SignUpHandler : IQueryHandler<CreateUser, HttpResponseMessage>
{
	private readonly KeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignUpHandler"/> class.
	/// </summary>
	/// <param name="keycloakService">KeycloakService.</param>
	public SignUpHandler(KeycloakService keycloakService) => this.keycloakService = keycloakService;

	/// <summary>
	/// Handle sign up operation in keycloak api.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>HttpResponseMessage.</returns>
	public async Task<HttpResponseMessage> Handle(CreateUser request, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetClientToken(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		string responseContent = await response.Content.ReadAsStringAsync();
		if (!string.IsNullOrEmpty(responseContent))
		{
			Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

			if (token == null || token?.AccessToken == null)
			{
				throw new AppException("One or more error occured while trying to create user.");
			}

			response = await this.keycloakService.AddUser(request, token.AccessToken, cancellationToken);

			if (!response.IsSuccessStatusCode)
			{
				throw new AppException("One or more error occured while trying to create user.");
			}

			return response;
		}

		throw new AppException("One or more error occured while trying to create user.");
	}
}