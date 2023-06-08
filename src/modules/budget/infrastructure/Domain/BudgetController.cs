using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.User.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;

/// <summary>
/// Controller responsible for managing budget-related operations.
/// </summary>
[ApiController]
[Route("api/budget")]
public class BudgetController : ControllerBase
{
	private readonly BudgetSenderEmail budgetSenderEmail;

	/// <summary>
	/// Initializes a new instance of the <see cref="BudgetController"/> class.
	/// </summary>
	/// <param name="budgetSenderEmail">The budget sender email service.</param>
	public BudgetController(BudgetSenderEmail budgetSenderEmail)
	{
		this.budgetSenderEmail = budgetSenderEmail;
	}

	/// <summary>
	/// Exports budgets and sends them via email.
	/// </summary>
	/// <param name="budgets">The list of budgets to export.</param>
	/// <param name="user">The user to send the email to.</param>
	/// <returns>The result of the operation.</returns>
	[HttpPost("export-and-send")]
	public IActionResult ExportAndSendBudgets(List<BudgetAggregate> budgets, AppUser user)
	{
		this.budgetSenderEmail.ExportAndSendBudgets(budgets, user);

		return this.Ok();
	}
}