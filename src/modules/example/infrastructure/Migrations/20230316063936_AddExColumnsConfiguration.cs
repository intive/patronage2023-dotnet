using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class AddExColumnsConfiguration : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Name",
				schema: "Examples",
				table: "Example",
				type: "nvarchar(256)",
				maxLength: 256,
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<string>(
				name: "Name",
				schema: "Examples",
				table: "Example",
				type: "nvarchar(max)",
				nullable: false,
				oldClrType: typeof(string),
				oldType: "nvarchar(256)",
				oldMaxLength: 256);
		}
	}
}
