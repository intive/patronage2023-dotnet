namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email configuration interface.
/// </summary>
public interface IEmailConfiguration
{
	/// <summary>
	/// Smtp server name.
	/// </summary>
	string SmtpServer { get; }

	/// <summary>
	/// Smpt port number.
	/// </summary>
	int SmtpPort { get; }

	/// <summary>
	/// SSL setting.
	/// </summary>
	bool UseSSL { get; }
}