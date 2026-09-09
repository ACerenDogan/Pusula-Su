using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class SayacForeignKeyTekrarDuzeltildi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SayacOkumalari_AboneKayitlari_AboneKaydiId1",
                table: "SayacOkumalari");

            migrationBuilder.DropIndex(
                name: "IX_SayacOkumalari_AboneKaydiId1",
                table: "SayacOkumalari");

            migrationBuilder.DropColumn(
                name: "AboneKaydiId1",
                table: "SayacOkumalari");

            migrationBuilder.AlterColumn<int>(
                name: "AboneKaydiId",
                table: "SayacOkumalari",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_SayacOkumalari_AboneKaydiId",
                table: "SayacOkumalari",
                column: "AboneKaydiId");

            migrationBuilder.AddForeignKey(
                name: "FK_SayacOkumalari_AboneKayitlari_AboneKaydiId",
                table: "SayacOkumalari",
                column: "AboneKaydiId",
                principalTable: "AboneKayitlari",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SayacOkumalari_AboneKayitlari_AboneKaydiId",
                table: "SayacOkumalari");

            migrationBuilder.DropIndex(
                name: "IX_SayacOkumalari_AboneKaydiId",
                table: "SayacOkumalari");

            migrationBuilder.AlterColumn<string>(
                name: "AboneKaydiId",
                table: "SayacOkumalari",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "AboneKaydiId1",
                table: "SayacOkumalari",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SayacOkumalari_AboneKaydiId1",
                table: "SayacOkumalari",
                column: "AboneKaydiId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SayacOkumalari_AboneKayitlari_AboneKaydiId1",
                table: "SayacOkumalari",
                column: "AboneKaydiId1",
                principalTable: "AboneKayitlari",
                principalColumn: "Id");
        }
    }
}
