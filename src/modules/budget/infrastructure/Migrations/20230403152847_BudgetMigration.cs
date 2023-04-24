using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class BudgetMigration : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema(
			name: "Budgets");

		migrationBuilder.CreateTable(
			name: "Budget",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
				CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Budget", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "DomainEventStore",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_DomainEventStore", x => x.Id);
			});
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Budget",
			schema: "Budgets");

		migrationBuilder.DropTable(
			name: "DomainEventStore",
			schema: "Budgets");
	}
}