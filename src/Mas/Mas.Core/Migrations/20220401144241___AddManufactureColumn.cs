using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddManufactureColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Manufactures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Manufactures",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Manufactures");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Manufactures");
        }
    }
}
