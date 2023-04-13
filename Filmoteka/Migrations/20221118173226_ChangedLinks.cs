using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmoteka.Migrations
{
    public partial class ChangedLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Link_Episodes_EpisodeId",
                table: "Link");

            migrationBuilder.DropForeignKey(
                name: "FK_Link_Movies_MovieId",
                table: "Link");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Link",
                table: "Link");

            migrationBuilder.RenameTable(
                name: "Link",
                newName: "Links");

            migrationBuilder.RenameIndex(
                name: "IX_Link_MovieId",
                table: "Links",
                newName: "IX_Links_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Link_EpisodeId",
                table: "Links",
                newName: "IX_Links_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Episodes_EpisodeId",
                table: "Links",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Movies_MovieId",
                table: "Links",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Episodes_EpisodeId",
                table: "Links");

            migrationBuilder.DropForeignKey(
                name: "FK_Links_Movies_MovieId",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.RenameTable(
                name: "Links",
                newName: "Link");

            migrationBuilder.RenameIndex(
                name: "IX_Links_MovieId",
                table: "Link",
                newName: "IX_Link_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Links_EpisodeId",
                table: "Link",
                newName: "IX_Link_EpisodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Link",
                table: "Link",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Episodes_EpisodeId",
                table: "Link",
                column: "EpisodeId",
                principalTable: "Episodes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Link_Movies_MovieId",
                table: "Link",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
