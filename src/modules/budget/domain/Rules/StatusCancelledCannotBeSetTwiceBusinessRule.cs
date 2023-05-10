using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// Budget Transaction Cancell Rule business rule.
/// </summary>
public class StatusCancelledCannotBeSetTwiceBusinessRule : IBusinessRule
{
	private readonly Status status;

	/// <summary>
	/// Initializes a new instance of the <see cref="StatusCancelledCannotBeSetTwiceBusinessRule"/> class.
	/// </summary>
	/// <param name="status">Budget Cancelled flag.</param>
	public StatusCancelledCannotBeSetTwiceBusinessRule(Status status)
	{
		this.status = status;
	}

	/// <inheritdoc />
	public bool IsBroken()
	{
		return this.status.Equals(Status.Deleted);
	}
}