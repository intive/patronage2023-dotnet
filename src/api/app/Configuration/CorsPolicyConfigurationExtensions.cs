namespace Intive.Patronage2023.Api.Configuration;

/// <summary>
/// Static class CorsPolicyConfiguration.
/// </summary>
public static class CorsPolicyConfigurationExtensions
{
	/// <summary>
	/// Extension method named that extends the IServiceCollection interface, that add Cross-Origin Resource Sharing (CORS) middleware to an application.
	/// </summary>
	/// <param name="services">IServiceCollection object.</param>
	/// <param name="configuration">IConfiguration object.</param>
	/// <param name="corsPolicyName">Name of the CORS policy.</param>
	public static void AddCors(
		this IServiceCollection services, IConfiguration configuration, string corsPolicyName) => services.AddCors(options =>
		{
			var apiSecuritySettings = configuration.GetSection("ApiSecuritySettings").Get<ApiSecuritySettings>();
			if (apiSecuritySettings == null)
			{
				return;
			}

			string[]? allowedOrigins = apiSecuritySettings?.CorsAllowedOrigins;
			if (allowedOrigins is null || allowedOrigins.Length == 0)
			{
				return;
			}

			options.AddPolicy(corsPolicyName, builder =>
			{
				builder.AllowAnyMethod()
				.AllowAnyHeader()
				.WithOrigins(allowedOrigins);
			});
		});
}