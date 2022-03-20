using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_CustomerGroup_CustomerGroupId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CustomerGroupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CustomerGroupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "District",
                table: "Customers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CustomerGroupId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "District",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CustomerGroupId",
                table: "Products",
                column: "CustomerGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_CustomerGroup_CustomerGroupId",
                table: "Products",
                column: "CustomerGroupId",
                principalTable: "CustomerGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
