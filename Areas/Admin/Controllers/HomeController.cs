using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;
using System.Diagnostics;
using System.Text;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppNguoiDung> _userManager;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppNguoiDung> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult DonDatVeByDate(string type = "view", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (type == "json")
            {
                // Kiểm tra nếu không có ngày bắt đầu hoặc ngày kết thúc
                if (startDate == null || endDate == null)
                {
                    return Json(new List<object>());
                }
                // Lọc dữ liệu theo ngày
                var data = _context.DonDatVe
                    .Where(d => d.NgayDat.Date >= startDate.Value && d.NgayDat.Date <= endDate.Value)
                    .GroupBy(d => d.NgayDat.Date)
                    .Select(g => new
                    {
                        NgayDat = g.Key,
                        SoLuong = g.Count()
                    })
                    .OrderBy(x => x.NgayDat)
                    .ToList();

                if (!data.Any())
                {
                    return Json(new List<object>());
                }
                return Json(data);
            }
            return View();
        }
        public IActionResult ThongKeVeTheoPhim(string type = "view")
        {
            if (type == "json")
            {
                var data = _context.ChiTietDatVe
                    .Include(ct => ct.GheSuatChieu) 
                    .ThenInclude(gs => gs.SuatChieu) 
                    .ThenInclude(s => s.Phim)
                    .GroupBy(ct => ct.GheSuatChieu.SuatChieu.Phim.TenPhim) 
                    .Select(g => new
                    {
                        TenPhim = g.Key,
                        SoLuongVe = g.Count(ct => ct.MaChiTietDatVe != 0)
                    })
                    .ToList();

                return Json(data);
            }
            return View();
        }

        public IActionResult ThongKeDoanhThuTheoPhim(string type = "view")
        {
            if (type == "json")
            {
                var data = _context.ChiTietDatVe
                    .Include(ct => ct.GheSuatChieu)
                    .ThenInclude(gs => gs.SuatChieu)
                    .ThenInclude(s => s.Phim)
                    .GroupBy(ct => ct.GheSuatChieu.SuatChieu.Phim.TenPhim)
                    .Select(g => new
                    {
                        TenPhim = g.Key,
                        DoanhThu = g.Sum(ct => ct.GiaVe)
                    })
                    .ToList();

                return Json(data);
            }
            return View();
        }

        public IActionResult ThongKeVeTheoRap()
        {
            var data = _context.ChiTietDatVe
                .Include(c => c.GheSuatChieu)
                .ThenInclude(g => g.SuatChieu)
                .ThenInclude(s => s.Phong)
                .ThenInclude(p => p.Rap)
                .GroupBy(c => c.GheSuatChieu.SuatChieu.Phong.Rap.TenRap)
                .Select(g => new
                {
                    RapChieuPhim = g.Key,
                    SoVeBan = g.Count()
                }).ToList();

            ViewBag.RapChiens = data.Select(d => d.RapChieuPhim).ToArray();
            ViewBag.SoVeBans = data.Select(d => d.SoVeBan).ToArray();
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
