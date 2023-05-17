using Intive.Patronage2023.Shared.Infrastructure.Exceptions;
using Intive.Patronage2023.Modules.User.Contracts;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Modules.User.Infrastructure;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.User.Application.CreatingUser;

/// <summary>
/// Create user command.
/// </summary>
/// <param name="Avatar">Avatar which user chooses.</param>
/// <param name="FirstName">First name which user provides.</param>
/// <param name="LastName">Last name which user provides.</param>
/// <param name="Password">Password which user provides.</param>
/// <param name="Email">Email which user provides.</param>
public record CreateUser(string Avatar, string FirstName, string LastName, string Password, string Email) : ICommand;

/// <summary>
/// Sign up request handler.
/// </summary>
public class CreateUserCommandHandler : ICommandHandler<CreateUser>
{
	private readonly IKeycloakService keycloakService;

	/// <summary>
	/// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
	/// </summary>
	/// <param name="keycloakService">KeycloakService.</param>
	public CreateUserCommandHandler(IKeycloakService keycloakService) => this.keycloakService = keycloakService;

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

		string responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

		if (string.IsNullOrEmpty(responseContent))
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

		if (token == null || token?.AccessToken == null)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}

		UserCredentials[] credentials =
		{
			new UserCredentials
			{
				Type = "password",
				Value = command.Password,
				Temporary = false,
			},
		};

		var attributes = new UserAttributes
		{
			Avatar = new string[] { command.Avatar },
		};

		var appUser = new AppUser
		{
			Email = command.Email,
			FirstName = command.FirstName,
			LastName = command.LastName,
			Enabled = true,
			Attributes = attributes,
			Credentials = credentials,
		};

		response = await this.keycloakService.AddUser(appUser, token.AccessToken, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			throw new AppException("One or more error occured while trying to create user.");
		}
	}
}