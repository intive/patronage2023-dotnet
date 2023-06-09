using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactionAttachment;

/// <summary>
/// Adding attachment file to transaction validator class.
/// </summary>
public class GetBudgetTransactionAttachmentValidator : AbstractValidator<GetBudgetTransactionAttachment>
{
	private readonly BudgetDbContext budgetDbContext;

	/// <summary>
	/// Initializes a new instance of the <see cref="GetBudgetTransactionAttachmentValidator"/> class.
	/// Attachment file validator.
	/// </summary>
	/// <param name="budgetDbContext">Budget context.</param>
	public GetBudgetTransactionAttachmentValidator(BudgetDbContext budgetDbContext)
	{
		this.budgetDbContext = budgetDbContext;

		this.RuleFor(command => command.TransactionId)
			.NotEmpty()
			.WithMessage("TransactionId cannot be empty.")
			.WithErrorCode("2.12")
			.MustAsync(async (x, cancellation) => await this.TransactionExists(x.Value))
			.WithMessage("Transaction doesn't exist.")
			.WithErrorCode("2.11");

		this.RuleFor(command => command.BudgetId)
			.NotEmpty()
			.WithMessage("BudgetId cannot be empty.")
			.WithErrorCode("1.12")
			.MustAsync(async (x, cancellation) => await this.BudgetExists(x.Value))
			.WithMessage("Budget doesn't exist.")
			.WithErrorCode("1.11");

		this.RuleFor(command => command)
			.MustAsync(async (x, cancellation) => await this.BelongsToBudget(x.BudgetId.Value, x.TransactionId.Value))
			.WithMessage("This transaction does not belong to the specified budget.")
			.WithErrorCode("2.10");

		this.RuleFor(command => command.TransactionId)
			.MustAsync(async (x, cancellation) => await this.DoesTransactionHaveAttachment(x.Value))
			.WithMessage("Transaction already has an attachment.")
			.WithErrorCode("2.20");
	}

	private async Task<bool> TransactionExists(Guid transactionId)
	{
		var transaction = await this.budgetDbContext.Transaction.FirstOrDefaultAsync(x => x.Id.Value.Equals(transactionId));
		return transaction != null;
	}

	private async Task<bool> BudgetExists(Guid budgetId)
	{
		var budget = await this.budgetDbContext.Budget.FirstOrDefaultAsync(x => x.Id.Value.Equals(budgetId));
		return budget != null;
	}

	private async Task<bool> BelongsToBudget(Guid budgetId, Guid transactionId)
	{
		var transaction = await this.budgetDbContext.Transaction.FirstOrDefaultAsync(x => x.Id.Value.Equals(transactionId));
		return budgetId == transaction!.BudgetId.Value;
	}

	private async Task<bool> DoesTransactionHaveAttachment(Guid transactionId)
	{
		var transaction = await this.budgetDbContext.Transaction.FirstOrDefaultAsync(x => x.Id.Value.Equals(transactionId));
		return transaction!.AttachmentUrl == null;
	}
}