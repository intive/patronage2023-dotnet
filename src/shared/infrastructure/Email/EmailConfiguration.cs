using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email configuration class.
/// </summary>
public class EmailConfiguration : IEmailConfiguration
{
	/// <inheritdoc/>
	[ConfigurationKeyName(nameof(SmtpServer))]
	public string SmtpServer { get; set; } = null!;

	/// <inheritdoc/>
	[ConfigurationKeyName(nameof(SmtpPort))]
	public int SmtpPort { get; set; }

	/// <inheritdoc/>
	[ConfigurationKeyName(nameof(UseSSL))]
	public bool UseSSL { get; set; }

	/// <inheritdoc/>
	[ConfigurationKeyName(nameof(UserName))]
	public string? UserName { get; set; }

	/// <inheritdoc/>
	[ConfigurationKeyName(nameof(Password))]
	public string? Password { get; set; }
}