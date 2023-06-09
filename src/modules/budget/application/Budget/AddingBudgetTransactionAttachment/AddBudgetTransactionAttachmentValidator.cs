using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;
using Microsoft.AspNetCore.Http;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;

/// <summary>
/// Adding attachment file to transaction validator class.
/// </summary>
public class AddBudgetTransactionAttachmentValidator : AbstractValidator<AddBudgetTransactionAttachment>
{
	private readonly long maxFileSize = 5000000;
	private readonly string[] allowedFileExtensions = { ".pdf", ".jpg", ".bmp", ".png" };
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="AddBudgetTransactionAttachmentValidator"/> class.
	/// Attachment file validator.
	/// </summary>
	/// <param name="budgetTransactionRepository">Budget Transaction Repository.</param>
	/// <param name="budgetRepository">Budget Repository.</param>
	public AddBudgetTransactionAttachmentValidator(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository, IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
		this.budgetRepository = budgetRepository;

		this.RuleFor(command => command.TransactionId)
			.NotEmpty()
			.WithMessage("TransactionId cannot be empty.")
			.WithErrorCode("2.12")
			.MustAsync(async (x, cancellation) => await this.TransactionExists(x.Value))
			.WithMessage("Transaction doesn't exist.")
			.WithErrorCode("2.11")
			.MustAsync(async (x, cancellation) => await this.IsTransactionWithoutAttachment(x.Value))
			.WithMessage("Transaction already has an attachment.")
			.WithErrorCode("2.18");

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

		this.RuleFor(command => command.File)
			.NotNull()
			.WithMessage("File is required.")
			.WithErrorCode("11.1")
			.Must(this.HaveValidSize)
			.WithMessage($"File size must be less than {this.maxFileSize} bytes.")
			.WithErrorCode("11.2")
			.Must(this.HaveValidExtension)
			.WithMessage($"File extension must be one of the following: {string.Join(", ", this.allowedFileExtensions)}.")
			.WithErrorCode("11.3");
	}

	private async Task<bool> TransactionExists(Guid transactionId)
	{
		BudgetTransactionAggregate? transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return transaction != null;
	}

	private async Task<bool> BudgetExists(Guid budgetId)
	{
		BudgetAggregate? budget = await this.budgetRepository.GetById(new BudgetId(budgetId));
		return budget != null;
	}

	private async Task<bool> BelongsToBudget(Guid budgetId, Guid transactionId)
	{
		var transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return transaction == null || transaction!.BudgetId.Value == budgetId;
	}

	private async Task<bool> IsTransactionWithoutAttachment(Guid transactionId)
	{
		BudgetTransactionAggregate? transaction = await this.budgetTransactionRepository.GetById(new TransactionId(transactionId));
		return transaction == null || transaction!.AttachmentUrl == null;
	}

	private bool HaveValidExtension(IFormFile file)
	{
		string extension = Path.GetExtension(file.FileName);
		return this.allowedFileExtensions.Contains(extension.ToLower());
	}

	private bool HaveValidSize(IFormFile file)
	{
		return file.Length <= this.maxFileSize;
	}
}