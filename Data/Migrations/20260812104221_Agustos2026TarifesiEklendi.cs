using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class Agustos2026TarifesiEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AboneTuru",
                table: "AboneKayitlari",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Tarifeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Yil = table.Column<int>(type: "INTEGER", nullable: false),
                    Ay = table.Column<int>(type: "INTEGER", nullable: false),
                    AboneTuru = table.Column<string>(type: "TEXT", nullable: false),
                    UstSinir = table.Column<decimal>(type: "TEXT", nullable: true),
                    AltSinir = table.Column<decimal>(type: "TEXT", nullable: false),
                    SuBirimFiyati = table.Column<decimal>(type: "TEXT", nullable: false),
                    AtikSuBirimFiyati = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarifeler", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "AboneTuru",
                value: "Mesken");

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 2,
                column: "AboneTuru",
                value: "Mesken");

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 3,
                column: "AboneTuru",
                value: "Mesken");

            migrationBuilder.InsertData(
                table: "Tarifeler",
                columns: new[] { "Id", "AboneTuru", "AltSinir", "AtikSuBirimFiyati", "Ay", "SuBirimFiyati", "UstSinir", "Yil" },
                values: new object[,]
                {
                    { 1, "Mesken", 0m, 17.64m, 8, 39.19m, 15m, 2026 },
                    { 2, "Mesken", 15m, 26.61m, 8, 59.08m, 30m, 2026 },
                    { 3, "Mesken", 30m, 39.85m, 8, 88.57m, 75m, 2026 },
                    { 4, "Mesken", 75m, 59.81m, 8, 132.97m, null, 2026 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tarifeler");

            migrationBuilder.DropColumn(
                name: "AboneTuru",
                table: "AboneKayitlari");
        }
    }
}
