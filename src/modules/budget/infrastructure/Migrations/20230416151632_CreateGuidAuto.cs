using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class CreateGuidAuto : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "Id",
			schema: "Budgets",
			table: "TransactionStore",
			type: "uniqueidentifier",
			nullable: false,
			defaultValueSql: "newsequentialid()",
			oldClrType: typeof(Guid),
			oldType: "uniqueidentifier");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<Guid>(
			name: "Id",
			schema: "Budgets",
			table: "TransactionStore",
			type: "uniqueidentifier",
			nullable: false,
			oldClrType: typeof(Guid),
			oldType: "uniqueidentifier",
			oldDefaultValueSql: "newsequentialid()");
	}
}