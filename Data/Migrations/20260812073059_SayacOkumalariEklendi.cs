using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class SayacOkumalariEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SayacOkumalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AboneKaydiId = table.Column<string>(type: "TEXT", nullable: false),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Endeks = table.Column<decimal>(type: "TEXT", nullable: false),
                    AboneKaydiId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SayacOkumalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SayacOkumalari_AboneKayitlari_AboneKaydiId1",
                        column: x => x.AboneKaydiId1,
                        principalTable: "AboneKayitlari",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SayacOkumalari_AboneKaydiId1",
                table: "SayacOkumalari",
                column: "AboneKaydiId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SayacOkumalari");
        }
    }
}
