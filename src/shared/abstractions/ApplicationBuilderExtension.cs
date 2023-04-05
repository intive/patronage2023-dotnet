using Microsoft.AspNetCore.Builder;

using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;

namespace Intive.Patronage2023.Shared.Abstractions;

/// <summary>
/// Class contains methods that extend ApplicationBuilder.
/// </summary>
public static class ApplicationBuilderExtension
{
	/// <summary>
	/// Enables migrations on startup for provided dbcontext class.
	/// </summary>
	/// <typeparam name="T">DbContext class to make migrations for.</typeparam>
	/// <param name="app">IApplicationBuilder.</param>
	/// <returns>Updated IApplicationBuilder.</returns>
	public static IApplicationBuilder InitDatabase<T>(this IApplicationBuilder app)
		where T : DbContext
	{
		using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
		{
			var dbContext = scope.ServiceProvider
				.GetRequiredService<T>();
			dbContext.Database.Migrate();
		}

		return app;
	}
}