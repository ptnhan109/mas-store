using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddGroupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerGroup_GroupId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerGroup",
                table: "CustomerGroup");

            migrationBuilder.RenameTable(
                name: "CustomerGroup",
                newName: "CustomerGroups");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerGroups",
                table: "CustomerGroups",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ManufactureGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufactureGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufactures",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Mail = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    GroupId = table.Column<Guid>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Province = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufactures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manufactures_ManufactureGroups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "ManufactureGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manufactures_GroupId",
                table: "Manufactures",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerGroups_GroupId",
                table: "Customers",
                column: "GroupId",
                principalTable: "CustomerGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CustomerGroups_GroupId",
                table: "Customers");

            migrationBuilder.DropTable(
                name: "Manufactures");

            migrationBuilder.DropTable(
                name: "ManufactureGroups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerGroups",
                table: "CustomerGroups");

            migrationBuilder.RenameTable(
                name: "CustomerGroups",
                newName: "CustomerGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerGroup",
                table: "CustomerGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CustomerGroup_GroupId",
                table: "Customers",
                column: "GroupId",
                principalTable: "CustomerGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
