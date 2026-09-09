using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PusulaSu.Data.Migrations
{
    /// <inheritdoc />
    public partial class FaturaBedeliEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ToplamBedel",
                table: "SayacOkumalari",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToplamBedel",
                table: "SayacOkumalari");
        }
    }
}
