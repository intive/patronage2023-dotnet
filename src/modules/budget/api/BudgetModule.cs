using FluentValidation;
using Intive.Patronage2023.Modules.Budget.Api.ResourcePermissions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.AddingBudgetTransactionAttachment;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CancelBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.CreatingBudgetTransaction;
using Intive.Patronage2023.Modules.Budget.Application.Budget.EditingBudget;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetDetails;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgets;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetsReport;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetStatistics;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactionAttachment;
using Intive.Patronage2023.Modules.Budget.Application.Budget.GettingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.RemoveBudget;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.AddingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.CategoryProviders;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.DeletingTransactionCategory;
using Intive.Patronage2023.Modules.Budget.Application.TransactionCategories.GettingTransactionCategories;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.AddingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.GettingUserBudget;
using Intive.Patronage2023.Modules.Budget.Application.UserBudgets.UpdateUserBudgetFavourite;
using Intive.Patronage2023.Modules.Budget.Contracts.Provider;
using Intive.Patronage2023.Modules.Budget.Infrastructure.Data;
using Intive.Patronage2023.Shared.Abstractions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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
		services.AddScoped<IAuthorizationHandler, BudgetAuthorizationHandler>();
		services.AddScoped<IValidator<CreateBudget>, CreateBudgetValidator>();
		services.AddScoped<IValidator<GetBudgets>, GetBudgetsValidator>();
		services.AddScoped<IValidator<EditBudget>, EditBudgetValidator>();
		services.AddScoped<IValidator<CreateBudgetTransaction>, CreateBudgetTransactionValidator>();
		services.AddScoped<IValidator<GetBudgetTransactions>, GetBudgetTransactionValidator>();
		services.AddScoped<IValidator<GetBudgetDetails>, GetBudgetDetailsValidator>();
		services.AddScoped<IValidator<RemoveBudget>, RemoveBudgetValidator>();
		services.AddScoped<IValidator<GetBudgetStatistics>, GetBudgetStatisticsValidator>();
		services.AddScoped<IValidator<CancelBudgetTransaction>, CancelBudgetTransactionValidator>();
		services.AddScoped<IValidator<AddUserBudgetList>, AddUserBudgetListValidator>();
		services.AddScoped<IValidator<GetUserBudgetList>, GetUserBudgetListValidator>();
		services.AddScoped<IValidator<UpdateUserBudgetFavourite>, UpdateUserBudgetFavouriteValidator>();
		services.AddScoped<IValidator<AddTransactionCategory>, AddCategoryValidator>();
		services.AddScoped<IValidator<GetTransactionCategories>, GetTransactionCategoryValidator>();
		services.AddScoped<IValidator<DeleteTransactionCategory>, DeleteTransactionCategoryValidator>();
		services.AddScoped<IValidator<GetBudgetsReport>, GetBudgetsReportValidator>();
		services.AddScoped<IValidator<AddBudgetTransactionAttachment>, AddBudgetTransactionAttachmentValidator>();
		services.AddScoped<IValidator<GetBudgetTransactionAttachment>, GetBudgetTransactionAttachmentValidator>();
		services.AddScoped<ICategoryProvider, StaticCategoryProvider>();
		services.AddScoped<ICategoryProvider, DatabaseCategoryProvider>();

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