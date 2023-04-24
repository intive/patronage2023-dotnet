using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class NewBudgetAggregate : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "Description",
			schema: "Budgets",
			table: "Budget",
			type: "nvarchar(max)",
			nullable: true);

		migrationBuilder.AddColumn<string>(
			name: "Icon",
			schema: "Budgets",
			table: "Budget",
			type: "nvarchar(max)",
			nullable: true);

		migrationBuilder.AddColumn<int>(
			name: "Limit_Currency",
			schema: "Budgets",
			table: "Budget",
			type: "int",
			nullable: false,
			defaultValue: 0);

		migrationBuilder.AddColumn<decimal>(
			name: "Limit_Value",
			schema: "Budgets",
			table: "Budget",
			type: "decimal(18,2)",
			nullable: false,
			defaultValue: 0m);

		migrationBuilder.AddColumn<DateTime>(
			name: "Period_EndDate",
			schema: "Budgets",
			table: "Budget",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

		migrationBuilder.AddColumn<DateTime>(
			name: "Period_StartDate",
			schema: "Budgets",
			table: "Budget",
			type: "datetime2",
			nullable: false,
			defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

		migrationBuilder.AddColumn<Guid>(
			name: "UserId",
			schema: "Budgets",
			table: "Budget",
			type: "uniqueidentifier",
			nullable: false,
			defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

		migrationBuilder.CreateIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget",
			columns: new[] { "UserId", "Name" },
			unique: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropIndex(
			name: "IX_Budget_UserId_Name",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Description",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Icon",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Limit_Currency",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Limit_Value",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Period_EndDate",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "Period_StartDate",
			schema: "Budgets",
			table: "Budget");

		migrationBuilder.DropColumn(
			name: "UserId",
			schema: "Budgets",
			table: "Budget");
	}
}