using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Budget.Infrastructure.Migrations;

/// <inheritdoc />
public partial class ModifyTransactionAttachmentColumn : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "AttachmentUrl",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "nvarchar(256)",
			maxLength: 256,
			nullable: true,
			oldClrType: typeof(string),
			oldType: "nvarchar(max)",
			oldNullable: true);
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "AttachmentUrl",
			schema: "Budgets",
			table: "BudgetTransaction",
			type: "nvarchar(max)",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "nvarchar(256)",
			oldMaxLength: 256,
			oldNullable: true);
	}
}