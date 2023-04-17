using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;

/// <summary>
/// Create Budget Transaction command.
/// </summary>
/// <param name="Type">Enum of Income or Expanse.</param>
/// <param name="Id">Id of Income or Expanse.</param>
/// <param name="BudgetId">Budget Id.</param>
/// <param name="Name">Name of income or expanse.</param>
/// <param name="Value">Value of income or expanse.</param>
/// <param name="Category">Enum of income/expanse Categories.</param>
/// <param name="TransactionDate">Date of creation budget transaction.</param>
public record CreateBudgetTransaction(TransactionTypes Type, Guid Id, Guid BudgetId, string Name, decimal Value, CategoriesType Category, DateTime TransactionDate) : ICommand;

/// <summary>
/// Create Budget Transaction.
/// </summary>
public class HandleCreateBudgetTransaction : ICommandHandler<CreateBudgetTransaction>
{
	private readonly IBudgetTransactionRepository budgetTransactionRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateBudgetTransaction"/> class.
	/// </summary>
	/// <param name="budgetTransactionRepository">Repository that manages Budget Transaction aggregate root.</param>
	public HandleCreateBudgetTransaction(IBudgetTransactionRepository budgetTransactionRepository)
	{
		this.budgetTransactionRepository = budgetTransactionRepository;
	}

	/// <inheritdoc/>
	public Task Handle(CreateBudgetTransaction command, CancellationToken cancellationToken)
	{
		var budgetTransaction = BudgetTransactionAggregate.Create(command.Id, command.BudgetId, command.Type, command.Name, command.Value, command.Category, command.TransactionDate);

		this.budgetTransactionRepository.Persist(budgetTransaction);
		return Task.CompletedTask;
	}
}