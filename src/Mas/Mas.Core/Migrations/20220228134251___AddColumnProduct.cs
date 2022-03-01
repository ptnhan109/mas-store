using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddColumnProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CloseToDate",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "Products",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloseToDate",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "Products");
        }
    }
}
