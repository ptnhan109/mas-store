using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class __AddImportTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Imports",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SearchParams = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ManufactureId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imports_Manufactures_ManufactureId",
                        column: x => x.ManufactureId,
                        principalTable: "Manufactures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SearchParams = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    ImportId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 55, nullable: true),
                    CurrentPrice = table.Column<double>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportDetails_Imports_ImportId",
                        column: x => x.ImportId,
                        principalTable: "Imports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ImportId",
                table: "ImportDetails",
                column: "ImportId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ProductId",
                table: "ImportDetails",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Imports_ManufactureId",
                table: "Imports",
                column: "ManufactureId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportDetails");

            migrationBuilder.DropTable(
                name: "Imports");
        }
    }
}
