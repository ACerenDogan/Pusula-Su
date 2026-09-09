using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class BildirimOkumaEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BildirimOkumalari",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BildirimId = table.Column<int>(type: "INTEGER", nullable: false),
                    KullaniciId = table.Column<string>(type: "TEXT", nullable: false),
                    OkunmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BildirimOkumalari", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BildirimOkumalari_Bildirimler_BildirimId",
                        column: x => x.BildirimId,
                        principalTable: "Bildirimler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BildirimOkumalari_BildirimId_KullaniciId",
                table: "BildirimOkumalari",
                columns: new[] { "BildirimId", "KullaniciId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BildirimOkumalari");
        }
    }
}
