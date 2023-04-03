using System.Text.Json;
using Intive.Patronage2023.Modules.Example.Application.Example.CreatingExample;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// Sign up Controller.
/// </summary>
[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{
	private readonly ICommandBus commandBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="SignUpController"/> class.
	/// </summary>
	/// <param name="commandBus">CommandBus.</param>
	public SignUpController(ICommandBus commandBus)
	{
		this.commandBus = commandBus;
	}

	/// <summary>
	/// Post Method to create user in keycloak.
	/// </summary>
	/// <param name="request">Request.</param>
	/// <returns>Returns created user.</returns>
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[HttpPost]
	public async Task<IActionResult> CreateUser([FromBody] CreateExample request)
	{
		var client = new HttpClient();
		var config = new ConfigurationBuilder()
		.AddJsonFile("appsettings.json")
		.Build();
		string? tokenEndpoint = config["KeycloakConfig:TokenEndpoint"];
		string? clientId = config["KeycloakConfig:ClientId"];
		string? clientSecret = config["KeycloakConfig:ClientSecret"];

		var tokenResponse = await client.PostAsync(tokenEndpoint, new StringContent($"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}"));

		string? token = JsonDocument.Parse(await tokenResponse.Content.ReadAsStringAsync()).RootElement.GetProperty("access_token").GetString();

		await this.commandBus.Send(request);
		return this.Created($"example/{request.Id}", request.Id);
	}
}