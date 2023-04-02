using System.Net;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Intive.Patronage2023.Modules.Example.Api.Controllers;

/// <summary>
/// text.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	/// <summary>
	/// text.
	/// </summary>
	/// /<param name="username">User login.</param>
	/// <param name="password">User password.</param>
	/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
	/// <response code="200">Successfully signed in.</response>
	/// <response code="400">Username or password is not valid.</response>
	/// <response code="500">Internal server error.</response>
	[HttpPost("SignIn")]
	public async Task<IActionResult> SignInUserAsync([FromForm] string username, [FromForm] string password)
	{
		using var client = new HttpClient();

		var content = new FormUrlEncodedContent(new[]
		{
				new KeyValuePair<string, string>("username", username),
				new KeyValuePair<string, string>("password", password),
				new KeyValuePair<string, string>("client_id", "test-client"),
				new KeyValuePair<string, string>("client_secret", "4VR8ktQIszIZVWgc3ud8efGAzYbbr1uu"),
				new KeyValuePair<string, string>("grant_type", "password"),
		});

		var response = await client.PostAsync("http://localhost:8080/realms/Test/protocol/openid-connect/token", content);

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			return this.BadRequest();
		}

		string responseContent = await response.Content.ReadAsStringAsync();
		var token = JsonConvert.DeserializeObject<Token>(responseContent);

		string? accessToken = token?.AccessToken;

		return this.Ok(accessToken);
	}

	/// <summary>
	/// text.
	/// </summary>
	[DataContract]
	public class Token
	{
		/// <summary>
		/// AccessToken.
		/// </summary>
		[DataMember(Name = "access_token")]
		public string? AccessToken { get; set; }

		/// <summary>
		/// ExpiresIn.
		/// </summary>
		[DataMember(Name = "expires_in")]
		public int? ExpiresIn { get; set; }

		/// <summary>
		/// RefreshExpiresIn.
		/// </summary>
		[DataMember(Name = "refresh_expires_in")]
		public int? RefreshExpiresIn { get; set; }

		/// <summary>
		/// RefreshToken.
		/// </summary>
		[DataMember(Name = "refresh_token")]
		public string? RefreshToken { get; set; }

		/// <summary>
		/// TokenType.
		/// </summary>
		[DataMember(Name = "token_type")]
		public string? TokenType { get; set; }

		/// <summary>
		/// NotBeforePolicy.
		/// </summary>
		[DataMember(Name = "not-before-policy")]
		public int? NotBeforePolicy { get; set; }

		/// <summary>
		/// SessionState.
		/// </summary>
		[DataMember(Name = "session_state")]
		public string? SessionState { get; set; }

		/// <summary>
		/// Scope.
		/// </summary>
		[DataMember(Name = "scope")]
		public string? Scope { get; set; }
	}
}