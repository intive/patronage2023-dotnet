using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared.Services;
using Intive.Patronage2023.Modules.Budget.Application.Budget.Shared;
using Intive.Patronage2023.Shared.Abstractions;
using Intive.Patronage2023.Shared.Infrastructure;

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
		services.AddScoped<IBlobStorageService, BlobStorageService>();
		services.AddScoped<IBudgetDataService, BudgetDataService>();
		services.AddScoped<ICsvService<GetBudgetTransferInfo>, CsvService<GetBudgetTransferInfo>>();

		return services;
	}
}