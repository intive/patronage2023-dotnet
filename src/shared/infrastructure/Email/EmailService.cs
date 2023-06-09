using Intive.Patronage2023.Shared.Infrastructure.Exceptions;

using MailKit.Net.Smtp;
using MailKit.Security;

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
		MimeMessage message = this.CreateMessage(emailMessage);
		this.Send(message);
	}

	private void Send(MimeMessage message)
	{
		using (var client = new SmtpClient())
		{
			try
			{
				client.Connect(this.emailConfiguration.SmtpServer, this.emailConfiguration.SmtpPort, this.emailConfiguration.UseSSL);
				if (this.emailConfiguration.HasUserCredentials())
				{
					client.Authenticate(new SaslMechanismLogin(this.emailConfiguration.UserName, this.emailConfiguration.Password));
				}

				client.Send(message);
				client.Disconnect(true);
			}
			catch (AuthenticationException)
			{
				throw new AppException("Username or password are incorrect.");
			}
			catch
			{
				throw new AppException("Email not sent properly.");
			}
		}
	}

	private MimeMessage CreateMessage(EmailMessage emailMessage)
	{
		var message = new MimeMessage();
		message.From.Add(this.ToMailboxAddress(emailMessage.SendFromAddress!));
		message.To.AddRange(emailMessage!.SendToAddresses!.Select(this.ToMailboxAddress));
		message.Subject = emailMessage!.Subject;
		var builder = new BodyBuilder
		{
			TextBody = emailMessage!.Body ?? string.Empty,
		};
		if (emailMessage.EmailAttachments is not null)
		{
			foreach (var item in emailMessage!.EmailAttachments)
			{
				builder.Attachments.Add(item.Name, item.Content);
			}
		}

		message.Body = builder.ToMessageBody();
		return message;
	}

	private MailboxAddress ToMailboxAddress(EmailAddress address) => new MailboxAddress(address.Name, address.Address);
}