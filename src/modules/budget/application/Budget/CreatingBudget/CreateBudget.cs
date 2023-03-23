namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;

/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
/// <param name="Name">Budget name.</param>
public record CreateBudget(Guid Id, string Name);

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
	public Task Handle(CreateBudget command)
	{
		var budget = BudgetAggregate.Create(command.Id, command.Name);

		this.budgetRepository.Persist(budget);
		return Task.CompletedTask;
	}
}