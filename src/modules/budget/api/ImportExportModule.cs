using Intive.Patronage2023.Modules.Budget.Application.Budget.ImportingBudgetTransactions;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure;
using FluentValidation;

namespace Intive.Patronage2023.Modules.Budget.Api;

/// <summary>
/// ImportExport module.
/// </summary>
public static class ImportExportModule
{
	/// <summary>
	/// Add module services.
	/// </summary>
	/// <param name="services">IServiceCollection.</param>
	/// <param name="configurationManager">ConfigurationManager.</param>
	/// <returns>Updated IServiceCollection.</returns>
	public static IServiceCollection AddImportExportModule(this IServiceCollection services, ConfigurationManager configurationManager)
	{
		services.AddScoped<IBudgetExportService, BudgetExportService>();
		services.AddScoped<IBudgetImportService, BudgetImportService>();
		services.AddScoped<IBudgetTransactionExportService, BudgetTransactionExportService>();
		services.AddScoped<IBudgetTransactionImportService, BudgetTransactionImportService>();
		services.AddScoped<IBlobStorageService, BlobStorageService>();
		services.AddScoped<IBudgetDataService, BudgetDataService>();
		services.AddScoped<IBudgetTransactionDataService, BudgetTransactionDataService>();
		services.AddScoped<ICsvService<GetBudgetTransferInfo>, CsvService<GetBudgetTransferInfo>>();
		services.AddScoped<ICsvService<GetBudgetTransactionTransferInfo>, CsvService<GetBudgetTransactionTransferInfo>>();
		services.AddScoped<ICsvService<GetBudgetTransactionImportInfo>, CsvService<GetBudgetTransactionImportInfo>>();
		services.AddScoped<IValidator<GetBudgetTransactionImportInfo>, GetBudgetTransactionImportInfoValidator>();

		return services;
	}
}