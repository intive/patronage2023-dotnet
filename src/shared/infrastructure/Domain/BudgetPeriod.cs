namespace Intive.Patronage2023.Shared.Infrastructure.Domain;

/// <summary>
/// Budget period value object.
/// </summary>
/// <param name="StartDate">Start date.</param>
/// <param name="EndDate">End date.</param>
public record class BudgetPeriod(DateOnly StartDate, DateOnly EndDate);