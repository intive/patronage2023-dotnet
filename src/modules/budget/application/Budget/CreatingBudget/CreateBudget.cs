using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;

/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
/// <param name="Name">Budget name.</param>
/// <param name="UserId">Budget owner user Id.</param>
/// <param name="Limit">Budget limit.</param>
/// <param name="Period">Budget time span.</param>
/// <param name="Description">Description.</param>
/// <param name="IconName">Budget icon identifier.</param>

public record CreateBudget(Guid Id, string Name, Guid UserId, Money Limit, Period Period, string Description, string IconName) : ICommand;

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
		var budget = BudgetAggregate.Create(command.Id, command.Name, command.UserId, command.Limit, command.Period, command.Description, command.IconName);
		await this.budgetRepository.Persist(budget);
	}
}