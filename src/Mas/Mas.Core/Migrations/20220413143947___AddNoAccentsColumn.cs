using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddNoAccentsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Prices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Manufactures",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "ManufactureGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Invoices",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "InvoiceDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "InventoryItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Customers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "CustomerGroups",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SearchParams",
                table: "Category",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Manufactures");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "ManufactureGroups");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "InvoiceDetails");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "CustomerGroups");

            migrationBuilder.DropColumn(
                name: "SearchParams",
                table: "Category");
        }
    }
}
