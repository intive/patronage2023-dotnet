using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;

namespace Intive.Patronage2023.Api.Configuration
{
	/// <summary>
	/// text.
	/// </summary>
	public static class CorsPolicyConfiguration
	{
		/// <summary>
		/// text.
		/// </summary>
		/// <param name="services">text1.</param>
		/// <param name="configuration">text2.</param>
		/// <param name="corsPolicyName">text3.</param>
		/// <returns>text4.</returns>
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
