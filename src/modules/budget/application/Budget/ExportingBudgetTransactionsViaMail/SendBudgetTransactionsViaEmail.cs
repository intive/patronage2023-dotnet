using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Email;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactionViaMail;

/// <summary>
/// .
/// </summary>
public record SendBudgetTransactionsViaEmail() : ICommand
{
	/// <summary>
	/// Budget id to export transactions to send email.
	/// </summary>
	public BudgetId BudgetId { get; set; }
}

/// <summary>
/// Class responsible for exporting budget's transactions and sending them via email.
/// </summary>
public class HandleSendBudgetTransactionsViaEmail : ICommandHandler<SendBudgetTransactionsViaEmail>
{
	private readonly ICsvService<BudgetTransactionAggregate> csvService;
	private readonly IEmailService emailService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSendBudgetTransactionsViaEmail"/> class.
	/// </summary>
	/// <param name="csvService">The CSV service.</param>
	/// <param name="emailService">The email service.</param>
	public HandleSendBudgetTransactionsViaEmail(ICsvService<BudgetTransactionAggregate> csvService, IEmailService emailService)
	{
		this.csvService = csvService;
		this.emailService = emailService;
	}

	/// <inheritdoc/>
	public async Task Handle(SendBudgetTransactionsViaEmail command, CancellationToken cancellationToken)
	{
		string fileName = this.csvService.GenerateFileNameWithCsvExtension();
		string emailAttachmentContent = " ";

		var emailMessage = new EmailMessage
		{
			Subject = "Test subject",
			Body = "Test body",
			SendFromAddress = new EmailAddress("testFrom", "testFrom@intive.pl"),
			SendToAddresses = new List<EmailAddress> { new("testTo", "testTo@invite.pl") },
			EmailAttachments = new EmailAttachment(fileName, emailAttachmentContent),
		};
	}
}