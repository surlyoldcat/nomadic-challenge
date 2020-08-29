using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace VetDesk.Migrations
{
    public partial class InitialSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CritterType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CritterType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(maxLength: 100, nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Critter",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    LastWeight = table.Column<decimal>(type: "decimal(4, 1)", nullable: false),
                    CritterTypeId = table.Column<int>(nullable: false),
                    Color = table.Column<string>(maxLength: 50, nullable: false),
                    Photo = table.Column<byte[]>(type: "image", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Critter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CritterType_Critter",
                        column: x => x.CritterTypeId,
                        principalTable: "CritterType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Customer_Critter",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CritterType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Cat" },
                    { 2, "Dog" },
                    { 3, "Llama" },
                    { 4, "Herp" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Critter_CritterTypeId",
                table: "Critter",
                column: "CritterTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Critter_CustomerId",
                table: "Critter",
                column: "CustomerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Critter");

            migrationBuilder.DropTable(
                name: "CritterType");

            migrationBuilder.DropTable(
                name: "Customer");
        }
    }
}
