using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetDesk.Migrations
{
    public partial class RefactoredImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Critter");

            migrationBuilder.AddColumn<int>(
                name: "PhotoId",
                table: "Critter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FileName = table.Column<string>(maxLength: 128, nullable: false),
                    ContentType = table.Column<string>(maxLength: 50, nullable: false),
                    PhotoFile = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Critter");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Critter",
                type: "image",
                nullable: true);
        }
    }
}
