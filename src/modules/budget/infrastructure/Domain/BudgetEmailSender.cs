using System.Net.Mail;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Class  responsible for exporting budgets and sending them via email.
/// </summary>
public class BudgetEmailSender
{
	/// <summary>
	/// Exports budgets to a CSV file and sends them via email.
	/// </summary>
	/// <param name="firstName">The first name of the user.</param>
	/// <param name="lastName">The last name of the user.</param>
	/// <param name="emailAddress">The email address of the user.</param>
	public void ExportAndSendBudgets(string firstName, string lastName, string emailAddress)
	{
		// TO Do: Connect with export budgets to a CSV file!!
		// string exportedBudgetsFilePath = this.ExportBudgets();
		string subject = "Exported budgets";
		string content = $"Dear {firstName} {lastName},\n\nThe attached file contains budgets as on {DateTime.Now}.\n\nBest regards,\nInbudget Team";

		var mailMessage = new MailMessage
		{
			Subject = subject,
			Body = content,
			IsBodyHtml = false,
		};
		mailMessage.To.Add(emailAddress);

		// mailMessage.Attachments.Add(new Attachment(exportedBudgetsFilePath));
		var smtpClient = new SmtpClient("smtp.example.com")
		{
			Port = 587,
			EnableSsl = true,
			Credentials = new System.Net.NetworkCredential("email@example.com", "password"),
		};

		smtpClient.Send(mailMessage);
	}
}