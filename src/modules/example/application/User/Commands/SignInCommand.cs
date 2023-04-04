using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using MediatR;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// SignIn command.
/// </summary>
/// <param name="Username">User name.</param>
/// <param name="Password">User password.</param>
public record SignInCommand(string Username, string Password) : ICommand;

/// <summary>
/// SignIn.
/// </summary>
public class HandleSignIn : ICommandHandler<SignInCommand>
{
	private readonly IHttpClientFactory httpClientFactory;
	public SignInCommandHandler(IHttpClientFactory httpClientFactory)
	{
		this.httpClientFactory = httpClientFactory;
	}

	public async Task<Token> Handle(SignInCommand request, CancellationToken cancellationToken)
	{
		var httpClient = _httpClientFactory.CreateClient();

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", request.Username),
				new KeyValuePair<string, string>("password", request.Username),
				new KeyValuePair<string, string>("client_id", "test-client"),
				new KeyValuePair<string, string>("client_secret", "4VR8ktQIszIZVWgc3ud8efGAzYbbr1uu"),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await client.PostAsync("http://localhost:8080/realms/Test/protocol/openid-connect/token", content);
		string responseContent = await response.Content.ReadAsStringAsync();
		Token token = JsonConvert.DeserializeObject<Token>(responseContent);

		return token;
	}
}