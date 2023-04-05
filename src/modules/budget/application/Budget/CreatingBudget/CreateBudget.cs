using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
/// <summary>
/// Create Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
/// <param name="Name">Budget name.</param>
/// <param name="StartDate">Budget start date.</param>
/// <param name="EndDate">Budget end date.</param>
/// <param name="Limit">Budget limit.</param>
/// <param name="Currency">Budget currency.</param>
/// <param name="Description">Description.</param>
/// <param name="IconName">IconName.</param>

public record CreateBudget(Guid Id, string Name, DateTime StartDate, DateTime EndDate, Currency Currency, decimal Limit, string Description, string IconName);

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

	/// <summary>
	/// Handling of CreateBudget.
	/// </summary>
	/// <param name="command">CreateBudget command.</param>
	/// <param name="cancellationToken">Cancellation token.</param>
	/// <returns>Result.</returns>
	public Task Handle(CreateBudget command, CancellationToken cancellationToken)
	{
		// var budget = BudgetAggregate.Create(command.Id, command.Name, command.StartDate, command.EndDate, command.Currency, command.Limit);
		// this.budgetRepository.Persist(budget);
		return Task.CompletedTask;
	}
}