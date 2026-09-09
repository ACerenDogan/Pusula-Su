using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class TarifeYayinTarihiVeTuketimDonemi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "YayinTarihi",
                table: "Tarifeler",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TuketimDonemleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AboneKaydiId = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicOkumasiId = table.Column<int>(type: "INTEGER", nullable: false),
                    BitisOkumasiId = table.Column<int>(type: "INTEGER", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BaslangicEndeksi = table.Column<decimal>(type: "TEXT", nullable: false),
                    BitisEndeksi = table.Column<decimal>(type: "TEXT", nullable: false),
                    ToplamTuketim = table.Column<decimal>(type: "TEXT", nullable: false),
                    SuBedeli = table.Column<decimal>(type: "TEXT", nullable: false),
                    AtikSuBedeli = table.Column<decimal>(type: "TEXT", nullable: false),
                    SuKdv = table.Column<decimal>(type: "TEXT", nullable: false),
                    AtikSuKdv = table.Column<decimal>(type: "TEXT", nullable: false),
                    Ctv = table.Column<decimal>(type: "TEXT", nullable: false),
                    ToplamBedel = table.Column<decimal>(type: "TEXT", nullable: false),
                    KapatilmaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IptalEdildi = table.Column<bool>(type: "INTEGER", nullable: false),
                    IptalTarihi = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuketimDonemleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TuketimDonemleri_AboneKayitlari_AboneKaydiId",
                        column: x => x.AboneKaydiId,
                        principalTable: "AboneKayitlari",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Tarifeler",
                keyColumn: "Id",
                keyValue: 1,
                column: "YayinTarihi",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tarifeler",
                keyColumn: "Id",
                keyValue: 2,
                column: "YayinTarihi",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tarifeler",
                keyColumn: "Id",
                keyValue: 3,
                column: "YayinTarihi",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tarifeler",
                keyColumn: "Id",
                keyValue: 4,
                column: "YayinTarihi",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_TuketimDonemleri_AboneKaydiId",
                table: "TuketimDonemleri",
                column: "AboneKaydiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TuketimDonemleri");

            migrationBuilder.DropColumn(
                name: "YayinTarihi",
                table: "Tarifeler");
        }
    }
}
