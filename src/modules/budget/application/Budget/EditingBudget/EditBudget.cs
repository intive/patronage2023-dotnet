using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Shared.Abstractions.Commands;
using Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

namespace Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;

/// <summary>
/// Edit Budget command.
/// </summary>
/// <param name="Id">Budget identifier.</param>
/// <param name="Name">Budget name.</param>
/// <param name="UserId">Budget owner user Id.</param>
/// <param name="Limit">Budget limit.</param>
/// <param name="Period">Budget time span.</param>
/// <param name="Description">Description.</param>
/// <param name="IconName">Budget icon identifier.</param>

public record EditBudget(Guid Id, string Name, Guid UserId, Money Limit, Period Period, string Description, string IconName) : ICommand;

/// <summary>
/// Edit Budget.
/// </summary>
public class HandleEditBudget : ICommandHandler<EditBudget>
{
	private readonly IBudgetRepository budgetRepository;

	/// <summary>
	/// Initializes a new instance of the <see cref="HandleEditBudget"/> class.
	/// </summary>
	/// <param name="budgetRepository">Repository that manages Budget aggregate root.</param>
	public HandleEditBudget(IBudgetRepository budgetRepository)
	{
		this.budgetRepository = budgetRepository;
	}

	/// <inheritdoc/>
	public async Task Handle(EditBudget command, CancellationToken cancellationToken)
	{
		var b = this.budgetRepository.GetById(command.Id);
		b.Result.EditBudget(command.Id, command.Name, command.UserId, command.Limit, command.Period, command.Description, command.IconName);
		await this.budgetRepository.Update(b.Result);
	}
}