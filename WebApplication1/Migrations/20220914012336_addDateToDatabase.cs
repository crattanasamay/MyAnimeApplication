using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class addDateToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeTableId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "UserAnime");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserAnime",
                newName: "Id");

            migrationBuilder.AddColumn<DateTime>(
                name: "AnimeAddDate",
                table: "UserAnime",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnimeAddDate",
                table: "UserAnime");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserAnime",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "AnimeTableId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "UserAnime",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
