using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Modules.Budget.Contracts.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;

/// <summary>
/// Remove Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
public record RemoveBudget(Guid Id) : ICommand;

/// <summary>
/// Remove Budget.
/// </summary>
public class RemoveBudgetCommandHandler : ICommandHandler<RemoveBudget>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="RemoveBudgetCommandHandler"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public RemoveBudgetCommandHandler(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(RemoveBudget command, CancellationToken cancellationToken)
	{
		var id = new BudgetId(command.Id);
		var budget = await this.budgetRepository.GetById(id);
		budget.SoftRemove();
		await this.budgetRepository.Persist(budget);
	}
}