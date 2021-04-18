using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LawFirmDatabaseImplement.Migrations
{
    public partial class laba3Extended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorageName = table.Column<string>(nullable: false),
                    StorageManager = table.Column<string>(nullable: false),
                    DateCreate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StorageBlanks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorageId = table.Column<int>(nullable: false),
                    BlankId = table.Column<int>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    DocumentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorageBlanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorageBlanks_Blanks_BlankId",
                        column: x => x.BlankId,
                        principalTable: "Blanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorageBlanks_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StorageBlanks_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StorageBlanks_BlankId",
                table: "StorageBlanks",
                column: "BlankId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageBlanks_DocumentId",
                table: "StorageBlanks",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_StorageBlanks_StorageId",
                table: "StorageBlanks",
                column: "StorageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StorageBlanks");

            migrationBuilder.DropTable(
                name: "Storages");
        }
    }
}
