using Intive.Patronage2023.Modules.User.Domain;
using Intive.Patronage2023.Shared.Abstractions.Extensions;

using Microsoft.EntityFrameworkCore;

namespace Intive.Patronage2023.Modules.User.Infrastructure.Data;

/// <summary>
/// Database context.
/// </summary>
public class UserDbContext : DbContext
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserDbContext"/> class.
	/// </summary>
	/// <param name="options">DbContext options.</param>
	public UserDbContext(DbContextOptions<UserDbContext> options)
		: base(options)
	{
	}

	/// <summary>
	/// Domain Event Store DbSet.
	/// </summary>
	public DbSet<DomainEventStore> DomainEventStore { get; set; }

	/// <inheritdoc />
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyAllConfigurationsFromAssemblies(typeof(UserDbContext).Assembly);
		base.OnModelCreating(modelBuilder);
	}
}