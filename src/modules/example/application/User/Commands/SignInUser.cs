using System.Linq.Expressions;
using Intive.Patronage2023.Api.Configuration;
using MediatR;
using Microsoft.Extensions.Options;

namespace Intive.Patronage2023.Modules.Example.Application.User.Commands;

/// <summary>
/// SignIn command.
/// </summary>
/// <param name="Username">Username.</param>
/// <param name="Password">Password.</param>
public record SignInUser(string Username, string Password) : IRequest<HttpResponseMessage>;

/// <summary>
/// SignIn.
/// </summary>
public class HandleSignIn : IRequestHandler<SignInUser, HttpResponseMessage>
{
	private readonly IHttpClientFactory httpClientFactory;
	private readonly ApiKeycloakSettings apiKeycloakSettings;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignIn"/> class.
	/// </summary>
	/// <param name="httpClientFactory">IHttpClientFactory.</param>
	/// <param name="apiKeycloakSettings">ApiKeycloakSettings.</param>
	public HandleSignIn(IHttpClientFactory httpClientFactory, ApiKeycloakSettings apiKeycloakSettings)
	{
		this.httpClientFactory = httpClientFactory;
		this.apiKeycloakSettings = apiKeycloakSettings.Value;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSignIn"/> class.
	/// </summary>
	/// <param name="request">request.</param>
	/// <param name="cancellationToken">cancellationToken.</param>
	/// <returns>Token.</returns>
	public async Task<HttpResponseMessage> Handle(SignInUser request, CancellationToken cancellationToken)
	{
		var httpClient = this.httpClientFactory.CreateClient();
		string? resource = this.apiKeycloakSettings.Resource;
		string? secret = this.apiKeycloakSettings.Credentials?.Secret;
		string? realm = this.apiKeycloakSettings.Realm;
		string? url = $"http://localhost:8080/realms/{realm}/protocol/openid-connect/token";

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", request.Username),
				new KeyValuePair<string, string>("password", request.Password),
				new KeyValuePair<string, string>("client_id", resource ?? string.Empty),
				new KeyValuePair<string, string>("client_secret", secret ?? string.Empty),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await httpClient.PostAsync(url, content);

		return response;
	}
}