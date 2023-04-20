namespace Intive.Patronage2023.Shared.Infrastructure.Domain.ValueObjects;

/// <summary>
/// Budget period value object.
/// </summary>
public record Period
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Period"/> class.
	/// </summary>
	/// <param name="startDate"> Budget start date. </param>
	/// <param name="endDate"> Budget end date. </param>
	public Period(DateTime startDate, DateTime endDate)
	{
		this.StartDate = startDate;
		this.EndDate = endDate;
	}

	/// <summary>
	/// Budget start date.
	/// </summary>
	public DateTime StartDate { get; init; }

	/// <summary>
	/// Budget end date.
	/// </summary>
	public DateTime EndDate { get; init; }
}