using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddUserBudgetTable : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "UserBudget",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				UserRole = table.Column<int>(type: "int", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_UserBudget", x => x.Id);
			});
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "UserBudget",
			schema: "Budgets");
	}
}