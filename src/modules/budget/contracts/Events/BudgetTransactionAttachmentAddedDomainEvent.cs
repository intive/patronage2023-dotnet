using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget transaction attachment added domain event.
/// </summary>
public class BudgetTransactionAttachmentAddedEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionAttachmentAddedEvent"/> class.
	/// </summary>
	/// <param name="transactionId">Budget transaction Id.</param>
	/// <param name="attachmentUrl">Attachment Url.</param>
	public BudgetTransactionAttachmentAddedEvent(TransactionId transactionId, string attachmentUrl)
	{
		this.Id = transactionId;
		this.AttachmentUrl = attachmentUrl;
	}

	/// <summary>
	/// Budget transaction Id.
	/// </summary>
	public TransactionId Id { get; private set; }

	/// <summary>
	/// Attachment Url.
	/// </summary>
	public string AttachmentUrl { get; private set; }
}