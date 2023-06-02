using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddBudgetTransactionCategoryTable : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "BudgetTransactionCategory",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
				BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				IconName = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Foreground = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Background = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_BudgetTransactionCategory", x => x.Id);
				table.ForeignKey(
					name: "FK_BudgetTransactionCategory_Budget_BudgetId",
					column: x => x.BudgetId,
					principalSchema: "Budgets",
					principalTable: "Budget",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_BudgetTransactionCategory_BudgetId",
			schema: "Budgets",
			table: "BudgetTransactionCategory",
			column: "BudgetId");

		migrationBuilder.CreateIndex(
			name: "IX_BudgetTransactionCategory_Id",
			schema: "Budgets",
			table: "BudgetTransactionCategory",
			column: "Id");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "BudgetTransactionCategory",
			schema: "Budgets");
	}
}