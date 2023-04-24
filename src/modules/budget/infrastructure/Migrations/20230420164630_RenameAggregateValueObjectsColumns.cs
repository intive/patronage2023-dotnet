using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class RenameAggregateValueObjectsColumns : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "Period_StartDate",
			schema: "Budgets",
			table: "Budget",
			newName: "StartDate");

		migrationBuilder.RenameColumn(
			name: "Period_EndDate",
			schema: "Budgets",
			table: "Budget",
			newName: "EndDate");

		migrationBuilder.RenameColumn(
			name: "Limit_Value",
			schema: "Budgets",
			table: "Budget",
			newName: "Value");

		migrationBuilder.RenameColumn(
			name: "Limit_Currency",
			schema: "Budgets",
			table: "Budget",
			newName: "Currency");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.RenameColumn(
			name: "Value",
			schema: "Budgets",
			table: "Budget",
			newName: "Limit_Value");

		migrationBuilder.RenameColumn(
			name: "StartDate",
			schema: "Budgets",
			table: "Budget",
			newName: "Period_StartDate");

		migrationBuilder.RenameColumn(
			name: "EndDate",
			schema: "Budgets",
			table: "Budget",
			newName: "Period_EndDate");

		migrationBuilder.RenameColumn(
			name: "Currency",
			schema: "Budgets",
			table: "Budget",
			newName: "Limit_Currency");
	}
}