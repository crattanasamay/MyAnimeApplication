using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class RemoveUserAnimeToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeImg",
                table: "UserAnime");

            migrationBuilder.DropColumn(
                name: "AnimeSeason",
                table: "UserAnime");

            migrationBuilder.DropColumn(
                name: "AnimeSummary",
                table: "UserAnime");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnimeImg",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnimeSeason",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AnimeSummary",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
