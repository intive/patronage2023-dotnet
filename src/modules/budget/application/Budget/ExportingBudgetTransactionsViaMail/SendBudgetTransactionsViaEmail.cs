using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

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
	private readonly IQueryBus queryBus;
	private readonly ICsvService<BudgetTransactionAggregate> csvService;
	private readonly IEmailService emailService;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSendBudgetTransactionsViaEmail"/> class.
	/// </summary>
	/// <param name="queryBus">Bus that sends the query.</param>
	/// <param name="csvService">The CSV service.</param>
	/// <param name="emailService">The email service.</param>
	public HandleSendBudgetTransactionsViaEmail(IQueryBus queryBus, ICsvService<BudgetTransactionAggregate> csvService, IEmailService emailService)
	{
		this.queryBus = queryBus;
		this.csvService = csvService;
		this.emailService = emailService;
	}

	/// <inheritdoc/>
	public async Task Handle(SendBudgetTransactionsViaEmail command, CancellationToken cancellationToken)
	{
		string fileName = this.csvService.GenerateFileNameWithCsvExtension();
		byte[] emailAttachmentContent = Array.Empty<byte>();
		var query = new GetBudgetTransactionsToExport { BudgetId = command.BudgetId };
		var transactions = await this.queryBus.Query<GetBudgetTransactionsToExport, GetTransferList<GetBudgetTransactionTransferInfo>?>(query);

		var emailMessage = new EmailMessage
		{
			Subject = "Test subject",
			Body = "Test body",
			SendFromAddress = new EmailAddress("testFrom", "testFrom@intive.pl"),
			SendToAddresses = new List<EmailAddress> { new("testTo", "testTo@invite.pl") },
			EmailAttachments = new List<FileDescriptor> { new FileDescriptor(fileName, emailAttachmentContent) },
		};
	}
}