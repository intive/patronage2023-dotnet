using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddTransactionUsername : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AddColumn<string>(
			name: "Username",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "nvarchar(max)",
			nullable: false,
			defaultValue: string.Empty);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropColumn(
			name: "Username",
			schema: "Budgets",
			table: "BudgetTransaction");
	}
}