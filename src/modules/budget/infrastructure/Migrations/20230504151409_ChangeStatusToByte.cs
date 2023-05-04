using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ChangeStatusToByte : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<byte>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "tinyint",
			nullable: false,
			defaultValue: (byte)1,
			oldClrType: typeof(string),
			oldType: "varchar(10)",
			oldDefaultValue: "Active");

		migrationBuilder.AlterColumn<byte>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "tinyint",
			nullable: false,
			defaultValue: (byte)1,
			oldClrType: typeof(string),
			oldType: "varchar(10)",
			oldDefaultValue: "Active");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "varchar(10)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(byte),
			oldType: "tinyint",
			oldDefaultValue: (byte)1);

		migrationBuilder.AlterColumn<string>(
			name: "Status",
			schema: "Budgets",
			table: "Budget",
			type: "varchar(10)",
			nullable: false,
			defaultValue: "Active",
			oldClrType: typeof(byte),
			oldType: "tinyint",
			oldDefaultValue: (byte)1);
	}
}