using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Create Budget Transaction command.
/// </summary>
/// <param name="Type">Enum of Income or Expense.</param>
/// <param name="Id">Id of Income or Expense.</param>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="Name">Name of income or Expense.</param>
/// <param name="Value">Value of income or Expense.</param>
/// <param name="Category">Enum of income/Expense Categories.</param>
/// <param name="TransactionDate">Date of creation budget transaction.</param>
public record CreateBudgetTransaction(
	TransactionType Type,
	Guid Id,
	Guid BudgetId,
	string Name,
	decimal Value,
	CategoryType Category,
	DateTime TransactionDate) : ICommand;

/// <summary>
/// Create Budget Transaction.
/// </summary>
public class HandleCreateBudgetTransaction : ICommandHandler<CreateBudgetTransaction>
{
	private readonly IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateBudgetTransaction"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget Transaction aggregate root.</param>
	public HandleCreateBudgetTransaction(IRepository<BudgetTransactionAggregate, TransactionId> budgetTransactionRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(CreateBudgetTransaction command, CancellationToken cancellationToken)
	{
		var id = new TransactionId(command.Id);
		var budgetId = new BudgetId(command.BudgetId);
		var budgetTransaction = BudgetTransactionAggregate.Create(
			id,
			budgetId,
			command.Type,
			command.Name,
			command.Value,
			command.Category,
			command.TransactionDate);

		await this.budgetTransactionRepository.Persist(budgetTransaction);
	}
}