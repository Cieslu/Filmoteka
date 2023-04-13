using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmoteka.Migrations
{
    public partial class added_episode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Episodes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SesonId",
                table: "Episodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Episodes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_SesonId",
                table: "Episodes",
                column: "SesonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Sesons_SesonId",
                table: "Episodes",
                column: "SesonId",
                principalTable: "Sesons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Sesons_SesonId",
                table: "Episodes");

            migrationBuilder.DropIndex(
                name: "IX_Episodes_SesonId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "SesonId",
                table: "Episodes");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Episodes");
        }
    }
}
