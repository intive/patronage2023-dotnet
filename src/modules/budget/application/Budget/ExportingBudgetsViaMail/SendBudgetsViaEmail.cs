using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Abstractions.Queries;
using Intive.Patronage2023.Shared.Infrastructure.Email;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport;
using Intive.Patronage2023.Shared.Infrastructure.ImportExport.Export;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgetsViaMail;

/// <summary>
/// SendBudgetViaEmail command.
/// </summary>
public record SendBudgetsViaEmail : ICommand;

/// <summary>
/// Class responsible for exporting budget and sending them via email.
/// </summary>
public class HandleSendBudgetViaEmail : ICommandHandler<SendBudgetsViaEmail>
{
	private readonly IQueryBus queryBus;
	private readonly IEmailService emailService;
	private readonly IBudgetExportService budgetExportService;
	private readonly IExecutionContextAccessor executionContextAccessor;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleSendBudgetViaEmail"/> class.
	/// </summary>
	/// <param name="queryBus">Bus that sends the query.</param>
	/// <param name="emailService">The email service.</param>
	/// <param name="budgetExportService">budget export service which holds method to export data to file.</param>
	/// <param name="executionContextAccessor">execution context accessor.</param>
	public HandleSendBudgetViaEmail(
		IQueryBus queryBus,
		IEmailService emailService,
		IBudgetExportService budgetExportService,
		IExecutionContextAccessor executionContextAccessor)
	{
		this.queryBus = queryBus;
		this.emailService = emailService;
		this.budgetExportService = budgetExportService;
		this.executionContextAccessor = executionContextAccessor;
	}

	/// <inheritdoc/>
	public async Task Handle(SendBudgetsViaEmail command, CancellationToken cancellationToken)
	{
		var query = new GetBudgetsToExport();
		var budgets = await this.queryBus.Query<GetBudgetsToExport, GetTransferList<GetBudgetTransferInfo>?>(query);
		var attachment = await this.budgetExportService.Export(budgets);

		var userData = this.executionContextAccessor.GetUserDataFromToken();
		string email = userData?["email"] ?? string.Empty;
		string name = userData?["name"] ?? string.Empty;

		var emailMessage = new EmailMessage
		{
			Subject = "Exported budgets",
			Body = $"Dear {name},\r\nThe attached file contains budgets as on date {DateTime.Now}\n" +
				"Best regards,\r\nInbudget Team",
			SendFromAddress = new EmailAddress("testFrom", "testFrom@intive.pl"),
			SendToAddresses = new List<EmailAddress> { new(name, email) },
			EmailAttachments = new List<FileDescriptor> { attachment },
		};

		this.emailService.SendEmail(emailMessage);
	}
}