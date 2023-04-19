using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingTransaction;

/// <summary>
/// Create Transaction command.
/// </summary>
/// <param name="Type">Enum of Income or Expanse.</param>
/// <param name="TransactionId">Id of Income or Expanse.</param>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="Name">Name of income or expanse.</param>
/// <param name="Value">Value of income or expanse.</param>
/// <param name="CreatedOn">Creation of new income or expanse date.</param>
/// <param name="Category">Enum of income/expanse Categories.</param>
public record CreateTransaction(TransactionTypes Type, TransactionId TransactionId, BudgetId BudgetId, string Name, decimal Value, DateTime CreatedOn, Categories Category) : ICommand;

/// <summary>
/// Create Transaction.
/// </summary>
public class HandleCreateTransaction : ICommandHandler<CreateTransaction>
{
	private readonly ITransactionRepository transactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateTransaction"/> class.
	/// </summary>
	/// <param name="transactionRepository">Repository that manages Transaction aggregate root.</param>
	public HandleCreateTransaction(ITransactionRepository transactionRepository)
	{
		this.transactionRepository = transactionRepository;
	}

	/// <inheritdoc/>
	public Task Handle(CreateTransaction command, CancellationToken cancellationToken)
	{
		var transaction = TransactionAggregate.Create(command.TransactionId, command.BudgetId, command.Type, command.Name, command.Value, command.Category, command.CreatedOn);

		this.transactionRepository.Persist(transaction);
		return Task.CompletedTask;
	}
}