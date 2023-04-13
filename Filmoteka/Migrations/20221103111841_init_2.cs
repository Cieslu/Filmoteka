using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Filmoteka.Migrations
{
    public partial class init_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MovieSeriesGenreId",
                table: "Series");

            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Series",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Photo",
                table: "Series",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieSeriesGenreId",
                table: "Series",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
