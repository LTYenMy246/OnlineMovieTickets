using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineMovieTicket.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ho",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ten",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DonDatVe",
                columns: table => new
                {
                    MaDonDatVe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNguoiDung = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DonDatVe", x => x.MaDonDatVe);
                    table.ForeignKey(
                        name: "FK_DonDatVe_AspNetUsers_MaNguoiDung",
                        column: x => x.MaNguoiDung,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ghe",
                columns: table => new
                {
                    MaGhe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenGhe = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ghe", x => x.MaGhe);
                });

            migrationBuilder.CreateTable(
                name: "NgayChieu",
                columns: table => new
                {
                    MaNgayChieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NgayChieu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NgayChieu", x => x.MaNgayChieu);
                });

            migrationBuilder.CreateTable(
                name: "Phim",
                columns: table => new
                {
                    MaPhim = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnhPhim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TrailerPhim = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TinhTrang = table.Column<bool>(type: "bit", nullable: false),
                    TenPhim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDungPhim = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TheLoai = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuocGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaoDien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DienVien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ThoiLuong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayKhoiChieu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phim", x => x.MaPhim);
                });

            migrationBuilder.CreateTable(
                name: "RapModel",
                columns: table => new
                {
                    MaRap = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenRap = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiaChiRap = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RapModel", x => x.MaRap);
                });

            migrationBuilder.CreateTable(
                name: "ThoiGianChieu",
                columns: table => new
                {
                    MaThoiGianChieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ThoiGianChieu = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThoiGianChieu", x => x.MaThoiGianChieu);
                });

            migrationBuilder.CreateTable(
                name: "BinhLuan",
                columns: table => new
                {
                    MaBinhLuan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungBinhLuan = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    NgayBinhLuan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TenNguoiBinhLuan = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaNguoiBinhLuan = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhim = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BinhLuan", x => x.MaBinhLuan);
                    table.ForeignKey(
                        name: "FK_BinhLuan_AspNetUsers_MaNguoiBinhLuan",
                        column: x => x.MaNguoiBinhLuan,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BinhLuan_Phim_MaPhim",
                        column: x => x.MaPhim,
                        principalTable: "Phim",
                        principalColumn: "MaPhim",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuKien",
                columns: table => new
                {
                    MaSuKien = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenSuKien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnhSuKien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoiDungSuKien = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NgayDangSuKien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaPhim = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuKien", x => x.MaSuKien);
                    table.ForeignKey(
                        name: "FK_SuKien_Phim_MaPhim",
                        column: x => x.MaPhim,
                        principalTable: "Phim",
                        principalColumn: "MaPhim",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhongModel",
                columns: table => new
                {
                    MaPhong = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaRap = table.Column<int>(type: "int", nullable: false),
                    SoLuongGhe = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhongModel", x => x.MaPhong);
                    table.ForeignKey(
                        name: "FK_PhongModel_RapModel_MaRap",
                        column: x => x.MaRap,
                        principalTable: "RapModel",
                        principalColumn: "MaRap",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuatChieu",
                columns: table => new
                {
                    MaSuatChieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaPhim = table.Column<int>(type: "int", nullable: false),
                    MaPhong = table.Column<int>(type: "int", nullable: false),
                    MaNgayChieu = table.Column<int>(type: "int", nullable: false),
                    MaThoiGianChieu = table.Column<int>(type: "int", nullable: false),
                    GiaVeSuatChieu = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuatChieu", x => x.MaSuatChieu);
                    table.ForeignKey(
                        name: "FK_SuatChieu_NgayChieu_MaNgayChieu",
                        column: x => x.MaNgayChieu,
                        principalTable: "NgayChieu",
                        principalColumn: "MaNgayChieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuatChieu_Phim_MaPhim",
                        column: x => x.MaPhim,
                        principalTable: "Phim",
                        principalColumn: "MaPhim",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuatChieu_PhongModel_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "PhongModel",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SuatChieu_ThoiGianChieu_MaThoiGianChieu",
                        column: x => x.MaThoiGianChieu,
                        principalTable: "ThoiGianChieu",
                        principalColumn: "MaThoiGianChieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GheSuatChieu",
                columns: table => new
                {
                    MaGheSuatChieu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSuatChieu = table.Column<int>(type: "int", nullable: false),
                    MaGhe = table.Column<int>(type: "int", nullable: false),
                    TrangThaiGhe = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GheSuatChieu", x => x.MaGheSuatChieu);
                    table.ForeignKey(
                        name: "FK_GheSuatChieu_Ghe_MaGhe",
                        column: x => x.MaGhe,
                        principalTable: "Ghe",
                        principalColumn: "MaGhe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GheSuatChieu_SuatChieu_MaSuatChieu",
                        column: x => x.MaSuatChieu,
                        principalTable: "SuatChieu",
                        principalColumn: "MaSuatChieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietDatVe",
                columns: table => new
                {
                    MaChiTietDatVe = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonDatVe = table.Column<int>(type: "int", nullable: false),
                    MaGheSuatChieu = table.Column<int>(type: "int", nullable: false),
                    GiaVe = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietDatVe", x => x.MaChiTietDatVe);
                    table.ForeignKey(
                        name: "FK_ChiTietDatVe_DonDatVe_MaDonDatVe",
                        column: x => x.MaDonDatVe,
                        principalTable: "DonDatVe",
                        principalColumn: "MaDonDatVe",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietDatVe_GheSuatChieu_MaGheSuatChieu",
                        column: x => x.MaGheSuatChieu,
                        principalTable: "GheSuatChieu",
                        principalColumn: "MaGheSuatChieu",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaNguoiBinhLuan",
                table: "BinhLuan",
                column: "MaNguoiBinhLuan");

            migrationBuilder.CreateIndex(
                name: "IX_BinhLuan_MaPhim",
                table: "BinhLuan",
                column: "MaPhim");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatVe_MaDonDatVe",
                table: "ChiTietDatVe",
                column: "MaDonDatVe");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietDatVe_MaGheSuatChieu",
                table: "ChiTietDatVe",
                column: "MaGheSuatChieu");

            migrationBuilder.CreateIndex(
                name: "IX_DonDatVe_MaNguoiDung",
                table: "DonDatVe",
                column: "MaNguoiDung");

            migrationBuilder.CreateIndex(
                name: "IX_GheSuatChieu_MaGhe",
                table: "GheSuatChieu",
                column: "MaGhe");

            migrationBuilder.CreateIndex(
                name: "IX_GheSuatChieu_MaSuatChieu",
                table: "GheSuatChieu",
                column: "MaSuatChieu");

            migrationBuilder.CreateIndex(
                name: "IX_PhongModel_MaRap",
                table: "PhongModel",
                column: "MaRap");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaNgayChieu",
                table: "SuatChieu",
                column: "MaNgayChieu");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaPhim",
                table: "SuatChieu",
                column: "MaPhim");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaPhong",
                table: "SuatChieu",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_SuatChieu_MaThoiGianChieu",
                table: "SuatChieu",
                column: "MaThoiGianChieu");

            migrationBuilder.CreateIndex(
                name: "IX_SuKien_MaPhim",
                table: "SuKien",
                column: "MaPhim");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BinhLuan");

            migrationBuilder.DropTable(
                name: "ChiTietDatVe");

            migrationBuilder.DropTable(
                name: "SuKien");

            migrationBuilder.DropTable(
                name: "DonDatVe");

            migrationBuilder.DropTable(
                name: "GheSuatChieu");

            migrationBuilder.DropTable(
                name: "Ghe");

            migrationBuilder.DropTable(
                name: "SuatChieu");

            migrationBuilder.DropTable(
                name: "NgayChieu");

            migrationBuilder.DropTable(
                name: "Phim");

            migrationBuilder.DropTable(
                name: "PhongModel");

            migrationBuilder.DropTable(
                name: "ThoiGianChieu");

            migrationBuilder.DropTable(
                name: "RapModel");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ho",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ten",
                table: "AspNetUsers");
        }
    }
}
