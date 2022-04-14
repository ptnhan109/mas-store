using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mas.Core.Migrations
{
    public partial class AddRelationDestruction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonDestruction",
                table: "DestructionDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "DestructionId",
                table: "DestructionDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_DestructionDetails_DestructionId",
                table: "DestructionDetails",
                column: "DestructionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DestructionDetails_Destructions_DestructionId",
                table: "DestructionDetails",
                column: "DestructionId",
                principalTable: "Destructions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DestructionDetails_Destructions_DestructionId",
                table: "DestructionDetails");

            migrationBuilder.DropIndex(
                name: "IX_DestructionDetails_DestructionId",
                table: "DestructionDetails");

            migrationBuilder.DropColumn(
                name: "DestructionId",
                table: "DestructionDetails");

            migrationBuilder.AddColumn<string>(
                name: "ReasonDestruction",
                table: "DestructionDetails",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
