using MediatR;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// SignIn command.
/// </summary>
/// <param name="Username">Username.</param>
/// <param name="Password">Password.</param>
public record SignInCommand(string Username, string Password) : IRequest<Token>;

/// <summary>
/// SignIn.
/// </summary>
public class HandleSignIn : IRequestHandler<SignInCommand, Token>
{
	private readonly IHttpClientFactory httpClientFactory;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignIn"/> class.
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory.</param>
	public HandleSignIn(IHttpClientFactory httpClientFactory)
	{
		this.httpClientFactory = httpClientFactory;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignIn"/> class.
	/// </summary>
	/// <param name="request">request.</param>
	/// <param name="cancellationToken">cancellationToken.</param>
	/// <returns>Token.</returns>
	public async Task<Token> Handle(SignInCommand request, CancellationToken cancellationToken)
	{
		var httpClient = this.httpClientFactory.CreateClient();

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", request.Username),
				new KeyValuePair<string, string>("password", request.Username),
				new KeyValuePair<string, string>("client_id", "test-client"),
				new KeyValuePair<string, string>("client_secret", "4VR8ktQIszIZVWgc3ud8efGAzYbbr1uu"),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await httpClient.PostAsync("http://localhost:8080/realms/Test/protocol/openid-connect/token", content);
		string responseContent = await response.Content.ReadAsStringAsync();
		Token? token = JsonConvert.DeserializeObject<Token>(responseContent);

		if (token == null)
		{
			return new Token();
		}

		return token;
	}
}