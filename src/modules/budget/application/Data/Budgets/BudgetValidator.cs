using System.Globalization;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;

namespace Intive.Patronage2023.Modules.Budget.Application.Data.Budgets;

/// <summary>
/// Class BudgetValidator.
/// </summary>
public class BudgetValidator
{
	/// <summary>
	/// Validates the properties of a budget object.
	/// </summary>
	/// <param name="budget">The budget object to validate.</param>
	/// <returns>A list of error messages. If the list is empty, the budget object is valid.</returns>
	public List<string> Validate(GetBudgetTransferInfo budget)
	{
		var errors = new List<string>();

		if (string.IsNullOrEmpty(budget.Name))
		{
			errors.Add("Budget name is missing");
		}

		if (string.IsNullOrEmpty(budget.IconName))
		{
			errors.Add("Budget icon name is missing");
		}

		if (string.IsNullOrEmpty(budget.Currency))
		{
			errors.Add("Budget currency is missing");
		}

		if (string.IsNullOrEmpty(budget.Value))
		{
			errors.Add("Budget value is missing");
		}
		else if (!decimal.TryParse(budget.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
		{
			errors.Add("Budget value is not a valid decimal number");
		}

		if (string.IsNullOrEmpty(budget.StartDate))
		{
			errors.Add("Budget start date is missing");
		}
		else if (!DateTime.TryParse(budget.StartDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget start date is not a valid date");
		}

		if (string.IsNullOrEmpty(budget.EndDate))
		{
			errors.Add("Budget end date is missing");
		}
		else if (!DateTime.TryParse(budget.EndDate, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
		{
			errors.Add("Budget end date is not a valid date");
		}

		if (!string.IsNullOrEmpty(budget.StartDate) && !string.IsNullOrEmpty(budget.EndDate) &&
			DateTime.Parse(budget.StartDate) >= DateTime.Parse(budget.EndDate))
		{
			errors.Add("Budget start date cannot be later than or equal to end date");
		}

		return errors;
	}
}