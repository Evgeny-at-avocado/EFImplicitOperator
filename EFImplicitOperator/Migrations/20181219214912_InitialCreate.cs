using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFImplicitOperator.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Childen",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Childen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    RequiredChildId = table.Column<Guid>(nullable: false),
                    OptionalChildId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_Childen_OptionalChildId",
                        column: x => x.OptionalChildId,
                        principalTable: "Childen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parents_Childen_RequiredChildId",
                        column: x => x.RequiredChildId,
                        principalTable: "Childen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Parents_OptionalChildId",
                table: "Parents",
                column: "OptionalChildId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_RequiredChildId",
                table: "Parents",
                column: "RequiredChildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "Childen");
        }
    }
}
