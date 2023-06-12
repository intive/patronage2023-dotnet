namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email configuration interface.
/// </summary>
public interface IEmailConfiguration
{
	/// <summary>
	/// Name of the user to authenticate to smtp server.
	/// </summary>
	string? UserName { get; }

	/// <summary>
	/// Password of the user to authenitcate to smtp server.
	/// </summary>
	string? Password { get; }

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