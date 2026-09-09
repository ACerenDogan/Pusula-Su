using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class KullaniciIdDuzeltildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "KullaniciId",
                value: null);

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 2,
                column: "KullaniciId",
                value: null);

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 3,
                column: "KullaniciId",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 1,
                column: "KullaniciId",
                value: "");

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 2,
                column: "KullaniciId",
                value: "");

            migrationBuilder.UpdateData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 3,
                column: "KullaniciId",
                value: "");
        }
    }
}
