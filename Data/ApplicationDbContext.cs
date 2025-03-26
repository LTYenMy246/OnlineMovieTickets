using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using OnlineMovieTicket.Models;
namespace OnlineMovieTicket.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<PhimModel> Phim { get; set; }

        public DbSet<RapModel> Rap { get; set; }

        public DbSet<RapModel> Phong { get; set; }
        public DbSet<NgayChieuModel> NgayChieu { get; set; }

        public DbSet<ThoiGianChieuModel> ThoiGianChieu { get; set; }
        public DbSet<DonDatVeModel> DonDatVe { get; set; }
        public DbSet<ChiTietDatVeModel> ChiTietDatVe { get; set; }
        public DbSet<SuatChieuModel> SuatChieu { get; set; }
        public DbSet<GheModel> Ghe { get; set; }
        public DbSet<GheSuatChieuModel> GheSuatChieu { get; set; }
        public DbSet<BinhLuanModel> BinhLuan { get; set; }

        public DbSet<SuKienModel> SuKien { get; set; }

        public DbSet<AppNguoiDung> AppNguoiDung { get; set; }
        public DbSet<OnlineMovieTicket.Models.PhongModel> PhongModel { get; set; } = default!;

    }
}
