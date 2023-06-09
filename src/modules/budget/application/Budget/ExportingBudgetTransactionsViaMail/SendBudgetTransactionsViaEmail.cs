using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetTransactionViaMail;

/// <summary>
/// SendBudgetTransactionsViaEmail command.
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
	private readonly IEmailService emailService;
	private readonly IBudgetTransactionExportService budgetTransactionExportService;
	private readonly IExecutionContextAccessor executionContextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSendBudgetTransactionsViaEmail"/> class.
	/// </summary>
	/// <param name="queryBus">Bus that sends the query.</param>
	/// <param name="emailService">The email service.</param>
	/// <param name="budgetTransactionExportService">budget transaction export service which holds method to export data to file.</param>
	/// <param name="executionContextAccessor">execution context accessor.</param>
	public HandleSendBudgetTransactionsViaEmail(
		IQueryBus queryBus,
		IEmailService emailService,
		IBudgetTransactionExportService budgetTransactionExportService,
		IExecutionContextAccessor executionContextAccessor)
	{
		this.queryBus = queryBus;
		this.emailService = emailService;
		this.budgetTransactionExportService = budgetTransactionExportService;
		this.executionContextAccessor = executionContextAccessor;
	}

	/// <inheritdoc/>
	public async Task Handle(SendBudgetTransactionsViaEmail command, CancellationToken cancellationToken)
	{
		var query = new GetBudgetTransactionsToExport { BudgetId = command.BudgetId };
		var getBudgetDetails = new GetBudgetDetails { Id = command.BudgetId.Value };

		var transactions = await this.queryBus.Query<GetBudgetTransactionsToExport, GetTransferList<GetBudgetTransactionTransferInfo>?>(query);
		var budgetDetails = await this.queryBus.Query<GetBudgetDetails, BudgetDetailsInfo?>(getBudgetDetails);
		var attachment = await this.budgetTransactionExportService.Export(transactions);

		var userData = this.executionContextAccessor.GetUserContext();
		string email = userData?.Email ?? string.Empty;
		string name = userData?.FirstName + " " + userData?.LastName ?? string.Empty;

		var emailMessage = new EmailMessage
		{
			Subject = "Exported budget transactions",
			Body = $"Dear {name},\r\nThe attached file contains transactions from budget {budgetDetails?.Name} as on date {DateTime.Now}\n" +
				"Best regards,\r\nInbudget Team",
			SendFromAddress = new EmailAddress("InBudget", "system@inbudget.com"),
			SendToAddresses = new List<EmailAddress> { new(name, email) },
			EmailAttachments = new List<FileDescriptor> { attachment },
		};

		this.emailService.SendEmail(emailMessage);
	}
}