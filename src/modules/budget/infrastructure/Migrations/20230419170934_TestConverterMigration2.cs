using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class TestConverterMigration2 : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateIndex(
			name: "IX_TransactionStore_BudgetId",
			schema: "Budgets",
			table: "TransactionStore",
			column: "BudgetId");

		migrationBuilder.AddForeignKey(
			name: "FK_TransactionStore_Budget_BudgetId",
			schema: "Budgets",
			table: "TransactionStore",
			column: "BudgetId",
			principalSchema: "Budgets",
			principalTable: "Budget",
			principalColumn: "Id",
			onDelete: ReferentialAction.Cascade);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropForeignKey(
			name: "FK_TransactionStore_Budget_BudgetId",
			schema: "Budgets",
			table: "TransactionStore");

		migrationBuilder.DropIndex(
			name: "IX_TransactionStore_BudgetId",
			schema: "Budgets",
			table: "TransactionStore");
	}
}