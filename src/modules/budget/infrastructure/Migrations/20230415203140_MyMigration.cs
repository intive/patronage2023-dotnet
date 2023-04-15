using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class MyMigration : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "Transaction",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
				TransactionType = table.Column<int>(type: "int", nullable: false),
				CategoryType = table.Column<int>(type: "int", nullable: false),
				Value = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
				CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Transaction", x => x.Id);
			});
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "Transaction",
			schema: "Budgets");
	}
}