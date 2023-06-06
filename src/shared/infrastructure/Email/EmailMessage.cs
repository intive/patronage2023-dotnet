namespace Intive.Patronage2023.Shared.Infrastructure.Email;

/// <summary>
/// Email  message class.
/// </summary>
public class EmailMessage
{
	/// <summary>
	/// Email addresses of recipients.
	/// </summary>
	public List<EmailAddress>? SendToAddresses { get; set; }

	/// <summary>
	/// Email address of sender.
	/// </summary>
	public EmailAddress? SendFromAddress { get; set; }

	/// <summary>
	/// Email subject.
	/// </summary>
	public string? Subject { get; set; }

	/// <summary>
	/// Email body.
	/// </summary>
	public string? Body { get; set; }

	/// <summary>
	/// Email attachments.
	/// </summary>
	public List<EmailAttachment>? Attachments { get; set; }
}