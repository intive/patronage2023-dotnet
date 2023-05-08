using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddFlagIsDeleted : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<bool>(
			name: "IsBudgetDeleted",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "bit",
			nullable: false,
			defaultValue: false);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "IsBudgetDeleted",
			schema: "Budgets",
			table: "BudgetTransaction");
	}
}