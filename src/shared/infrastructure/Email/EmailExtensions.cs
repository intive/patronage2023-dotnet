namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Class that contains email extensions methods.
/// </summary>
public static class EmailExtensions
{
	/// <summary>
	/// Checks whether configuration has user credentials.
	/// </summary>
	/// <param name="configuration">Email configuration.</param>
	/// <returns>True if there are provided username and password.</returns>
	public static bool HasUserCredentials(this IEmailConfiguration configuration) => !string.IsNullOrEmpty(configuration.UserName) && !string.IsNullOrEmpty(configuration.Password);
}