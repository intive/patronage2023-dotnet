using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Api.Configuration
{
	/// <summary>
	/// Static class CorsPolicyConfiguration.
	/// </summary>
	public static class CorsPolicyConfiguration
	{
		/// <summary>
		/// Eetension method named that extends the IServiceCollection interface, that add Cross-Origin Resource Sharing (CORS) middleware to an application.
		/// </summary>
		/// <param name="services">IServiceCollection object.</param>
		/// <param name="configuration">IConfiguration object.</param>
		/// <param name="corsPolicyName">Name of the CORS policy.</param>
		/// <returns>Returns <ref name="IServiceCollection"/>IServiceCollection.</returns>
		public static IServiceCollection AddCors(
			this IServiceCollection services, IConfiguration configuration, string corsPolicyName) => services.AddCors(options =>
			{
				string? allowedOrigins = configuration.GetValue<string>("CorsAllowedOrigins");
				if (!string.IsNullOrEmpty(allowedOrigins))
				{
					options.AddPolicy(corsPolicyName, builder =>
					{
						string[] origins = allowedOrigins.Split(";");
						builder.AllowAnyMethod()
						.AllowAnyHeader()
						.WithOrigins(origins);
					});
				}
			});
	}
}
