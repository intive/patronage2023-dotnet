using Intive.Patronage2023.Api.Keycloak;
using Intive.Patronage2023.Api.User.CreatingUser;
using Intive.Patronage2023.Modules.Example.Application.Example;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Api.User.Commands;

/// <summary>
/// Sign up request handler.
/// </summary>
public class SignUpHandler : ICommandHandler<CreateUser>
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
	/// <param name="command">Command.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the command.</param>
	/// <returns>HttpResponseMessage.</returns>
	public async Task Handle(CreateUser command, CancellationToken cancellationToken)
	{
		var response = await this.keycloakService.GetClientToken(cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		string responseContent = await response.Content.ReadAsStringAsync();

		if (string.IsNullOrEmpty(responseContent))
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

		if (token == null || token?.AccessToken == null)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		response = await this.keycloakService.AddUser(command, token.AccessToken, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}
	}
}