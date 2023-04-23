using FluentValidation;

using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Domain;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Domain;
using Intive.Patronage2023.Shared.Abstractions.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.Budget.Api;

/// <summary>
/// Budget module.
/// </summary>
public static class BudgetModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <param name="configurationManager">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddBudgetModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddDbContext<BudgetDbContext>(options => options.UseSqlServer(configurationManager.GetConnectionString("AppDb")));

		services.AddScoped<IBudgetRepository, BudgetRepository>();
		services.AddScoped<IValidator<CreateBudget>, CreateBudgetValidator>();
		services.AddScoped<IValidator<GetBudgets>, GetBudgetsValidator>();
		services.AddScoped<IValidator<GetBudgetDetails>, GetBudgetDetailsValidator>();

		return services;
	}

	/// <summary>
	/// Customizes app building process.
	/// </summary>
	/// <param name="app">IApplicationBuilder.</param>
	/// <returns>Updated IApplicationBuilder.</returns>
	public static IApplicationBuilder UseBudgetModule(this IApplicationBuilder app)
	{
		app.InitDatabase<BudgetDbContext>();
		return app;
	}
}