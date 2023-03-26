using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intive.Patronage2023.Modules.Example.Infrastructure.Migrations;

/// <inheritdoc />
public partial class AddDomainEventStore : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.CreateTable(
			name: "DomainEventStore",
			schema: "Examples",
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
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DropTable(
			name: "DomainEventStore",
			schema: "Examples");
	}
}