using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Helpers;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="BudgetId">Budget identifier.</param>
/// <param name="Name">Budget name.</param>
public record CreateBudget(BudgetId BudgetId, string Name) : ICommand;

/// <summary>
/// Create Budget.
/// </summary>
public class HandleCreateBudget : ICommandHandler<CreateBudget>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleCreateBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleCreateBudget(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(CreateBudget command, CancellationToken cancellationToken)
	{
		var budget = BudgetAggregate.Create(command.BudgetId, command.Name);

		await this.budgetRepository.Persist(budget);
	}
}