namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email service interface for sending and receiving emails.
/// </summary>
public interface IEmailService
{
	/// <summary>
	/// Method for sending emails.
	/// </summary>
	/// <param name="emailMessage">Email message details.</param>
	void SendEmail(EmailMessage emailMessage);
}