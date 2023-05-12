using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdateStatus : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "IsBudgetDeleted",
			schema: "Budgets",
			table: "BudgetTransaction");

		migrationBuilder.DropColumn(
			name: "IsDeleted",
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

		migrationBuilder.AddColumn<bool>(
			name: "IsBudgetDeleted",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "bit",
			nullable: false,
			defaultValue: false);

		migrationBuilder.AddColumn<bool>(
			name: "IsDeleted",
			schema: "Budgets",
			table: "Budget",
			type: "bit",
			nullable: false,
			defaultValue: false);
	}
}