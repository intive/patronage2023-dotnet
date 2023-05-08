using Intive.Patronage2023.Modules.Budget.Contracts.TransactionEnums;
using Intive.Patronage2023.Shared.Abstractions.Domain;

namespace Intive.Patronage2023.Modules.Budget.Domain.Rules;

/// <summary>
/// Budget and Budget Transaction Soft Delete business rule.
/// </summary>
public class StatusDeletedCannotBeSetTwiceBusinessRule : IBusinessRule
{
	private readonly Status status;

	/// <summary>
	/// Initializes a new instance of the <see cref="StatusDeletedCannotBeSetTwiceBusinessRule"/> class.
	/// </summary>
	/// <param name="status">Budget Soft Delete flag.</param>
	public StatusDeletedCannotBeSetTwiceBusinessRule(Status status)
	{
		this.status = status;
	}

	/// <inheritdoc />
	public bool IsBroken() => this.status.Equals(Status.Deleted);
}