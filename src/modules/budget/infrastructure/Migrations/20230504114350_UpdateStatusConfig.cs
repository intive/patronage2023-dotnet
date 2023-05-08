using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class UpdateStatusConfig : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "varchar(10)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(string),
			oldType: "nvarchar(max)",
			oldDefaultValue: "Active");

		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "varchar(10)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(string),
			oldType: "nvarchar(max)",
			oldDefaultValue: "Active");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(string),
			oldType: "varchar(10)",
			oldDefaultValue: "Active");

		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(string),
			oldType: "varchar(10)",
			oldDefaultValue: "Active");
	}
}