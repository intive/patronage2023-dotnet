using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangeStatusToByte : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction");

		migrationBuilder.DropColumn(
			name: "Status",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.AddColumn<bool>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "tinyint",
			nullable: false,
			defaultValue: (byte)1);

		migrationBuilder.AddColumn<bool>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "tinyint",
			nullable: false,
			defaultValue: (byte)1);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction");

		migrationBuilder.DropColumn(
			name: "Status",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.AddColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: "Active");

		migrationBuilder.AddColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: "Active");
	}
}