using MailKit.Net.Smtp;
using MimeKit;

namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email service implementation used for sending emails.
/// </summary>
public class EmailService : IEmailService
{
	private readonly IEmailConfiguration emailConfiguration;

	/// <summary>
	/// Initializes a new instance of the <see cref="EmailService"/> class.
	/// </summary>
	/// <param name="emailConfiguration">Email configuration details.</param>
	public EmailService(IEmailConfiguration emailConfiguration)
	{
		this.emailConfiguration = emailConfiguration;
	}

	/// <inheritdoc/>
	public void SendEmail(EmailMessage emailMessage)
	{
		ArgumentNullException.ThrowIfNull(emailMessage);
		ArgumentNullException.ThrowIfNull(emailMessage.SendFromAddress);
		ArgumentNullException.ThrowIfNull(emailMessage.SendToAddresses);

		var message = new MimeMessage();
		message.From.Add(this.ToMailboxAddress(emailMessage.SendFromAddress));
		message.To.AddRange(emailMessage!.SendToAddresses!.Select(this.ToMailboxAddress));
		message.Subject = emailMessage!.Subject;
		message.Body = new TextPart("plain") { Text = emailMessage!.Body };

		using (var client = new SmtpClient())
		{
			client.Connect(this.emailConfiguration.SmtpServer, this.emailConfiguration.SmtpPort, this.emailConfiguration.UseSSL);
			client.Send(message);
			client.Disconnect(true);
		}
	}

	private MailboxAddress ToMailboxAddress(EmailAddress address) => new MailboxAddress(address.Name, address.Address);
}