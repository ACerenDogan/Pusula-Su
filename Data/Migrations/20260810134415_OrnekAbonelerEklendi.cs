using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrnekAbonelerEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AboneKayitlari",
                columns: new[] { "Id", "AboneNo", "AdSoyad", "KullaniciId", "SayacNo" },
                values: new object[,]
                {
                    { 1, "123456", "Ceren Doğan", "", "789012" },
                    { 2, "654321", "Zeren Nas", "", "210987" },
                    { 3, "987654", "Ayşe Ceren", "", "345678" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AboneKayitlari",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
