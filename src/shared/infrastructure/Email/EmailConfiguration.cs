using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email configuration class.
/// </summary>
public class EmailConfiguration : IEmailConfiguration
{
	/// <inheritdoc/>
	[ConfigurationKeyName("SmtpServer")]
	public string SmtpServer { get; set; } = null!;

	/// <inheritdoc/>
	[ConfigurationKeyName("SmtpPort")]
	public int SmtpPort { get; set; }

	/// <inheritdoc/>
	[ConfigurationKeyName("UseSSL")]
	public bool UseSSL { get; set; }
}