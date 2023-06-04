using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Shared.Infrastructure.Events;

namespace Intive.Patronage2023.Modules.Budget.Contracts.Events;

/// <summary>
/// Budget transaction attachment added domain event.
/// </summary>
public class BudgetTransactionAttachmentAddedDomainEvent : DomainEvent
{
	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetTransactionAttachmentAddedDomainEvent"/> class.
	/// </summary>
	/// <param name="transactionId">Budget transaction Id.</param>
	/// <param name="attachmentUrl">Attachment Url.</param>
	public BudgetTransactionAttachmentAddedDomainEvent(TransactionId transactionId, Uri attachmentUrl)
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
	public Uri AttachmentUrl { get; private set; }
}