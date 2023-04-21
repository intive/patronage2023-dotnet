using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddBudgetTransactionTable : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.EnsureSchema(
			name: "Budgets");

		migrationBuilder.CreateTable(
			name: "Budget",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
				CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_Budget", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "DomainEventStore",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
				Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
				CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_DomainEventStore", x => x.Id);
			});

		migrationBuilder.CreateTable(
			name: "BudgetTransaction",
			schema: "Budgets",
			columns: table => new
			{
				Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "newsequentialid()"),
				BudgetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
				TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
				Value = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
				CategoryType = table.Column<string>(type: "nvarchar(max)", nullable: false),
				BudgetTransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
				CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
			},
			constraints: table =>
			{
				table.PrimaryKey("PK_BudgetTransaction", x => x.Id);
				table.ForeignKey(
					name: "FK_BudgetTransaction_Budget_BudgetId",
					column: x => x.BudgetId,
					principalSchema: "Budgets",
					principalTable: "Budget",
					principalColumn: "Id",
					onDelete: ReferentialAction.Cascade);
			});

		migrationBuilder.CreateIndex(
			name: "IX_BudgetTransaction_BudgetId",
			schema: "Budgets",
			table: "BudgetTransaction",
			column: "BudgetId");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "BudgetTransaction",
			schema: "Budgets");

		migrationBuilder.DropTable(
			name: "DomainEventStore",
			schema: "Budgets");

		migrationBuilder.DropTable(
			name: "Budget",
			schema: "Budgets");
	}
}