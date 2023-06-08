using System.Globalization;
using CsvHelper;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure.Email;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Class responsible for exporting budgets and sending them via email.
/// </summary>
public class BudgetSenderEmail
{
	private readonly IBlobStorageService blobStorageService;
	private readonly ICsvService<BudgetAggregate> csvService;
	private readonly IEmailService emailService;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetSenderEmail"/> class.
	/// </summary>
	/// <param name="blobStorageService">The blob storage service.</param>
	/// <param name="csvService">The CSV service.</param>
	/// <param name="emailService">The email service.</param>
	public BudgetSenderEmail(IBlobStorageService blobStorageService, ICsvService<BudgetAggregate> csvService, IEmailService emailService)
	{
		this.blobStorageService = blobStorageService;
		this.csvService = csvService;
		this.emailService = emailService;
	}

	/// <summary>
	/// Exports budgets to a CSV file, uploads it to blob storage, and sends it via email.
	/// </summary>
	/// <param name="budgets">The list of budgets to export.</param>
	/// <param name="user">The user to send the email to.</param>
	/// <returns>A task representing the asynchronous operation.</returns>
	public Task ExportAndSendBudgets(List<BudgetAggregate> budgets, AppUser user)
	{
		string fileName = this.csvService.GenerateFileNameWithCsvExtension();
		var stream = new MemoryStream();

		using (var writer = new StreamWriter(stream, leaveOpen: true))
		using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
		{
			this.csvService.WriteRecordsToMemoryStream(budgets, csvWriter);
			csvWriter.Flush();
			writer.Flush();
			stream.Position = 0;

			// Generate the email message
			var emailMessage = new EmailMessage
			{
				Subject = "Exported budgets",
				Body = $"Dear {user.FirstName} {user.LastName},\n\nThe attached file contains budgets as on {DateTime.UtcNow.ToShortDateString()}.\n\nBest regards,\nInbudget Team",
				SendToAddresses = new List<EmailAddress> { new EmailAddress(user.FirstName, user.Email) },
				Attachments = new List<EmailAttachment> { new EmailAttachment(fileName, Convert.ToBase64String(stream.ToArray())) },
			};
			this.emailService.SendEmail(emailMessage);
		}

		return Task.CompletedTask;
	}
}