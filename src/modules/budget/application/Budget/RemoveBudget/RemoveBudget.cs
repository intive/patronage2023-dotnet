using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;

/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public record RemoveBudget(Guid Id) : ICommand;

/// <summary>
/// Create Budget.
/// </summary>
public class HandleRemoveBudget : ICommandHandler<RemoveBudget>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleRemoveBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleRemoveBudget(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(RemoveBudget command, CancellationToken cancellationToken)
	{
		var id = new BudgetId(command.Id);
		bool isDeleted = true;
		var budget = await this.budgetRepository.GetById(id);

		var aggregate = BudgetAggregate.Create(id, budget.Name, budget.UserId, budget.Limit, budget.Period, budget.Icon, budget.Description ?? " ", isDeleted);

		aggregate.UpdateIsRemoved(isDeleted);

		await this.budgetRepository.Persist(budget);
	}
}