using System.Text;
using Intive.Patronage2023.Modules.Budget.Application.Budget.ExportingBudgets;
using Intive.Patronage2023.Shared.Abstractions.Queries;

namespace Intive.Patronage2023.Modules.Budget.Application.Data;

/// <summary>
/// Class DataService.
/// </summary>
public class DataService
{
	private readonly IQueryBus queryBus;

	/// <summary>
	/// Initializes a new instance of the <see cref="DataService"/> class.
	/// DataService.
	/// </summary>
	/// <param name="queryBus">QueryBus.</param>
	public DataService(IQueryBus queryBus)
	{
		this.queryBus = queryBus;
	}

	/// <summary>
	/// Method to export budgets to CSV file.
	/// </summary>
	/// <returns>CSV file.</returns>
	public async Task Export()
	{
		var query = new GetBudgetsToExport() { };
		var userBudgets = await this.queryBus.Query<GetBudgetsToExport, List<GetBudgetsToExportInfo>?>(query);

		using (var writer = new StreamWriter("MyBudgets.csv", false, Encoding.UTF8))
		{
			writer.WriteLine("Name, Limit, Currency, StartDate, EndDate, Icon, Description");
			foreach (var budget in userBudgets!)
			{
				writer.WriteLine($"{budget.Name}, {budget.Limit}, {budget.Currency}, {budget.StartDate}, {budget.EndDate}, {budget.Icon}, {budget.Description}");
			}
		}
	}
}