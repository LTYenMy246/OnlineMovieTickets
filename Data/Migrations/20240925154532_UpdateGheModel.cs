using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMovieTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGheModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaPhong",
                table: "Ghe",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ghe_MaPhong",
                table: "Ghe",
                column: "MaPhong");

            migrationBuilder.AddForeignKey(
                name: "FK_Ghe_PhongModel_MaPhong",
                table: "Ghe",
                column: "MaPhong",
                principalTable: "PhongModel",
                principalColumn: "MaPhong",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ghe_PhongModel_MaPhong",
                table: "Ghe");

            migrationBuilder.DropIndex(
                name: "IX_Ghe_MaPhong",
                table: "Ghe");

            migrationBuilder.DropColumn(
                name: "MaPhong",
                table: "Ghe");
        }
    }
}
