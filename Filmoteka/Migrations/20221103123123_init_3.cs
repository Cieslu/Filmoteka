using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmoteka.Migrations
{
    public partial class init_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesSeson_Series_SeriesId",
                table: "SeriesSeson");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesSeson_Seson_SesonId",
                table: "SeriesSeson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seson",
                table: "Seson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesSeson",
                table: "SeriesSeson");

            migrationBuilder.RenameTable(
                name: "Seson",
                newName: "Sesons");

            migrationBuilder.RenameTable(
                name: "SeriesSeson",
                newName: "SeriesSesons");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesSeson_SesonId",
                table: "SeriesSesons",
                newName: "IX_SeriesSesons_SesonId");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesSeson_SeriesId",
                table: "SeriesSesons",
                newName: "IX_SeriesSesons_SeriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sesons",
                table: "Sesons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesSesons",
                table: "SeriesSesons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesSesons_Series_SeriesId",
                table: "SeriesSesons",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesSesons_Sesons_SesonId",
                table: "SeriesSesons",
                column: "SesonId",
                principalTable: "Sesons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SeriesSesons_Series_SeriesId",
                table: "SeriesSesons");

            migrationBuilder.DropForeignKey(
                name: "FK_SeriesSesons_Sesons_SesonId",
                table: "SeriesSesons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sesons",
                table: "Sesons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SeriesSesons",
                table: "SeriesSesons");

            migrationBuilder.RenameTable(
                name: "Sesons",
                newName: "Seson");

            migrationBuilder.RenameTable(
                name: "SeriesSesons",
                newName: "SeriesSeson");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesSesons_SesonId",
                table: "SeriesSeson",
                newName: "IX_SeriesSeson_SesonId");

            migrationBuilder.RenameIndex(
                name: "IX_SeriesSesons_SeriesId",
                table: "SeriesSeson",
                newName: "IX_SeriesSeson_SeriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seson",
                table: "Seson",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeriesSeson",
                table: "SeriesSeson",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesSeson_Series_SeriesId",
                table: "SeriesSeson",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SeriesSeson_Seson_SesonId",
                table: "SeriesSeson",
                column: "SesonId",
                principalTable: "Seson",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
