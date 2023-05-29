namespace Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;

/// <summary>
/// Command to retrieve a list of budget transfer information.
/// </summary>
public record GetBudgetTransferList()
{
	/// <summary>
	/// Contains a list of budget transfer details.
	/// </summary>
	public List<GetBudgetTransferInfo> BudgetsList { get; init; } = default!;
}