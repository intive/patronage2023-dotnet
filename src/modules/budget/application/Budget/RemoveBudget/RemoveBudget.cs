using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;

/// <summary>
/// Remove Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public record RemoveBudget(Guid Id) : ICommand;

/// <summary>
/// Remove Budget.
/// </summary>
public class HandlerRemoveBudget : ICommandHandler<RemoveBudget>
{
	private readonly IRepository<BudgetAggregate, BudgetId> budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandlerRemoveBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandlerRemoveBudget(IRepository<BudgetAggregate, BudgetId> budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(RemoveBudget command, CancellationToken cancellationToken)
	{
		var id = new BudgetId(command.Id);
		var budget = await this.budgetRepository.GetById(id);
		budget!.SoftRemove();
		await this.budgetRepository.Persist(budget);
	}
}