using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddIndexFilterForDeletedBudgets : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.CreateIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget",
			columns: new[] { "UserId", "Name" },
			unique: true,
			filter: "Status <> 2");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.CreateIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget",
			columns: new[] { "UserId", "Name" },
			unique: true);
	}
}