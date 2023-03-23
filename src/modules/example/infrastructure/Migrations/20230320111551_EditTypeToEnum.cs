using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class EditTypeToEnum : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<int>(
				name: "Type",
				schema: "Examples",
				table: "DomainEventStore",
				type: "int",
				nullable: false,
				defaultValue: 0,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Type",
				schema: "Examples",
				table: "DomainEventStore",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(int),
				oldType: "int");
		}
	}
}
