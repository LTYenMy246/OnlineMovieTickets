using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Math;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using OnlineMovieTicket.Utilities;
using OnlineMovieTicket.Services;
using System.Net;
using static OnlineMovieTicket.Controllers.HomeController;
using static Org.BouncyCastle.Math.EC.ECCurve;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OnlineMovieTicket.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppNguoiDung> _userManager;
        private readonly IVnPayService _vnPayservice;
        private readonly IConfiguration _config;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<AppNguoiDung> userManager, IVnPayService vnPayservice, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _vnPayservice = vnPayservice;
            _config = config;
        }

        public IActionResult Index(string searchString, int page = 1)
        {
            //TinhTrang == true
            var phim = _context.Phim.Where(m => m.TinhTrang == true).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                phim = phim.Where(m => m.TenPhim.Contains(searchString));
            }

            int pageSize = 9;
            var paginatedMovies = phim.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(phim.Count() / (double)pageSize);
            ViewBag.SearchString = searchString;

            return View(paginatedMovies);
        }

        public async Task<IActionResult> DSPhimDangChieu()
        {
            var movies = await _context.Phim.ToListAsync();
            var releasedMovies = movies.Where(m => m.TinhTrang).ToList();

            ViewBag.ReleasedMovies = releasedMovies;
            return View();
        }

        public async Task<IActionResult> DSPhimSapChieu()
        {
            var movies = await _context.Phim.ToListAsync();
            var upcomingMovies = movies.Where(m => !m.TinhTrang).ToList();

            ViewBag.UpcomingMovies = upcomingMovies;
            return View();
        }

        //Chi tiet phim
        public IActionResult ChiTietPhim(int id)
        {
            var phim = _context.Phim.FirstOrDefault(m => m.MaPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            var binhLuans = _context.BinhLuan
                .Where(b => b.MaPhim == id)
                .ToList();

            if (phim != null)
            {
                var luotXem = HttpContext.Session.GetInt32($"LuotXem_{id}") ?? 0;
                HttpContext.Session.SetInt32($"LuotXem_{id}", (int)luotXem + 1);
                ViewBag.LuotXem = luotXem + 1;
            }
            ViewBag.BinhLuans = binhLuans;
            return View(phim);
        }

        public IActionResult SuKien()
        {
            var suKienList = _context.SuKien.ToList();
            ViewBag.SuKienList = suKienList;
            return View(); 
        }

        public IActionResult ChiTietSuKien(int id)
        {
            var suKien = _context.SuKien.Include(s => s.Phim).FirstOrDefault(s => s.MaSuKien == id);
            if (suKien == null)
            {
                return NotFound();
            }
            ViewBag.SuKien = suKien;
            return View();
        }

        [HttpPost]
        public IActionResult AddComment(int maPhim, string noiDungBinhLuan)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ErrorMessage"] = "Ban can dang nhap de binh luan";
                return RedirectToAction("ChiTietPhim", new { id = maPhim });
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Lay ID nguoi dung
            var binhLuan = new BinhLuanModel
            {
                NoiDungBinhLuan = noiDungBinhLuan,
                NgayBinhLuan = DateTime.Now,
                TenNguoiBinhLuan = User.Identity.Name,
                MaNguoiBinhLuan = userId,
                MaPhim = maPhim
            };
            _context.BinhLuan.Add(binhLuan);
            _context.SaveChanges();

            return RedirectToAction("ChiTietPhim", new { id = maPhim });
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ThongTin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult DatVe()
        {
            // Lay ngay hien tai
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            ViewBag.DanhSachPhim = _context.Phim
            .Where(p => p.TinhTrang == true)
            .ToList();
            var danhSachNgayChieu = _context.NgayChieu
                .Where(n => n.NgayChieu >= today) 
                .OrderBy(n => n.NgayChieu) 
                .Take(5) // Chi lay 5 ngay tuong lai theo ngay hien tai
                .ToList();

            ViewBag.DanhSachNgayChieu = danhSachNgayChieu;
            return View();
        }

        public IActionResult LayRapTheoPhimVaNgay(int maPhim, DateOnly ngayChieu)
        {
            var danhSachRap = _context.SuatChieu
                .Include(s => s.Phong)
                .ThenInclude(p => p.Rap)
                .Where(s => s.MaPhim == maPhim && s.NgayChieu.NgayChieu == ngayChieu)
                .Select(s => new
                {
                    s.Phong.MaPhong,
                    TenRap = s.Phong.Rap.TenRap,
                    s.MaSuatChieu,
                    s.GiaVeSuatChieu
                })
                .Distinct()
                .ToList();
            return Json(danhSachRap);
        }

        [HttpPost]
        public IActionResult LayRapVaSuatChieu(int maPhim, int maNgayChieu)
        {
            var rapVaSuatChieu = _context.SuatChieu
                .Include(s => s.Phong)
                .ThenInclude(p => p.Rap)
                .Where(s => s.MaPhim == maPhim && s.MaNgayChieu == maNgayChieu)
                .Select(s => new
                {
                    tenRap = s.Phong.Rap.TenRap,
                    thoiGianChieu = s.ThoiGianChieu.ThoiGianChieu,
                    suatChieuId = s.MaSuatChieu
                })
                .Distinct()
                .ToList();
            return Json(rapVaSuatChieu);
        }

        [Authorize]
        public IActionResult DonDatVe(int suatChieuId)
        {
            var suatChieu = _context.SuatChieu
                .Include(s => s.Phim)
                .Include(s => s.Phong)
                .Include(s => s.Phong.Rap)
                .Include(s => s.NgayChieu)
                .Include(s => s.ThoiGianChieu)
                .FirstOrDefault(s => s.MaSuatChieu == suatChieuId);

            if (suatChieu == null)
            {
                return NotFound("Không.");
            }
            HttpContext.Session.SetInt32("MaSuatChieu", suatChieuId);
            var gheSuatChieu = _context.GheSuatChieu
                .Include(g => g.Ghe)
                .Where(g => g.MaSuatChieu == suatChieuId)
                .ToList();

            ViewBag.Phim = suatChieu.Phim;
            ViewBag.Phong = suatChieu.Phong;
            ViewBag.Rap = suatChieu.Phong.Rap;
            ViewBag.NgayChieu = suatChieu.NgayChieu;
            ViewBag.ThoiGianChieu = suatChieu.ThoiGianChieu;
            ViewBag.GiaVe = suatChieu.GiaVeSuatChieu;
            ViewBag.GheSuatChieu = gheSuatChieu;
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult DonDatVe(string selectedGheIds)
        {
            var maSuatChieu = HttpContext.Session.GetInt32("MaSuatChieu");
            decimal tongTien = 0;
            var gheIds = selectedGheIds?.Split(',').Select(int.Parse).ToList();

            if (gheIds == null || gheIds.Count == 0)
            {
                TempData["ErrorMessage"] = "Ban chua chon ghe. Vui lòng chon ghe truoc khi thanh toan.";
                return RedirectToAction("DonDatVe", new { suatChieuId = maSuatChieu.Value });

            }
            if (gheIds != null && gheIds.Count > 0)
            {
                var suatChieu = _context.SuatChieu
                    .Include(s => s.Phim)
                    .Include(s => s.Phong)
                    .Include(s => s.NgayChieu)
                    .FirstOrDefault(s => s.MaSuatChieu == maSuatChieu.Value);

                if (suatChieu != null)
                {
                    List<string> gheDaChon = new List<string>();
                    foreach (var gheId in gheIds)
                    {
                        var gheSuatChieu = _context.GheSuatChieu
                            .Include(g => g.Ghe)
                            .FirstOrDefault(g => g.MaGhe == gheId && g.MaSuatChieu == maSuatChieu);

                        if (gheSuatChieu != null)
                        {
                            tongTien += suatChieu.GiaVeSuatChieu;
                            gheDaChon.Add(gheSuatChieu.Ghe.MaGhe.ToString());
                        }
                    }
                    HttpContext.Session.SetString("GheDaChon", string.Join(",", gheDaChon));
                    HttpContext.Session.SetDecimal("TongTien", tongTien);
                    return RedirectToAction("ThanhToan");
                }
            }
            return RedirectToAction("ThanhToan");
        }


        private async Task<string> GetTenGheAsync(int maGhe)
        {
            var ghe = await _context.Ghe.FirstOrDefaultAsync(g => g.MaGhe == maGhe);
            return ghe?.TenGhe ?? "Không";
        }

        [Authorize]
        public async Task<IActionResult> ThanhToan()
        {
            var maSuatChieu = HttpContext.Session.GetInt32("MaSuatChieu");
            var tongTien = HttpContext.Session.GetDecimal("TongTien");
            var gheDaChon = HttpContext.Session.GetString("GheDaChon");

            if (!maSuatChieu.HasValue || tongTien <= 0 || string.IsNullOrEmpty(gheDaChon))
            {
                TempData["ErrorMessage"] = "Ban can chon ghe truoc khi thanh toan.";
                return RedirectToAction("DonDatVe", new { suatChieuId = maSuatChieu.Value });
            }

            var nguoiDung = await _userManager.GetUserAsync(User);
            if (nguoiDung == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var suatChieu = await _context.SuatChieu
                .Include(sc => sc.Phim)
                .FirstOrDefaultAsync(sc => sc.MaSuatChieu == maSuatChieu.Value);

            var maGheList = gheDaChon.Split(',').Select(int.Parse).ToList();
            var tenGheList = new List<string>();

            foreach (var maGhe in maGheList)
            {
                var tenGhe = await GetTenGheAsync(maGhe);
                tenGheList.Add(tenGhe);
            }

            var viewModel = new ThanhToanViewModel
            {
                NgayDat = DateTime.Now,
                NguoiDat = nguoiDung.UserName,
                TongTien = tongTien.Value,
                GheDaChon = string.Join(", ", tenGheList),
                TenPhim = suatChieu.Phim.TenPhim,
            };

            ViewBag.ThanhToanMessage = TempData["ThanhToanMessage"];
            return View(viewModel);
        }

        //Thanh toan truc tiep
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> XacNhan()
        {
            var maSuatChieu = HttpContext.Session.GetInt32("MaSuatChieu");
            var tongTien = HttpContext.Session.GetDecimal("TongTien");
            var gheDaChon = HttpContext.Session.GetString("GheDaChon");

            if (!maSuatChieu.HasValue || tongTien <= 0 || string.IsNullOrEmpty(gheDaChon))
            {
                return RedirectToAction("Index", "Home");
            }

            var nguoiDung = await _userManager.GetUserAsync(User);
            if (nguoiDung == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var danhSachGhe = gheDaChon.Split(',').Select(int.Parse).ToList();

            foreach (var gheId in danhSachGhe)
            {
                var gheSuatChieu = await _context.GheSuatChieu
                    .FirstOrDefaultAsync(g => g.MaGhe == gheId && g.MaSuatChieu == maSuatChieu);

                if (gheSuatChieu == null)
                {
                    TempData["ErrorMessage"] = "Ghe nay da duoc dat.";
                    return RedirectToAction("ThanhToan");
                }

                var daDatVe = await _context.ChiTietDatVe
                    .AnyAsync(c => c.MaGheSuatChieu == gheSuatChieu.MaGheSuatChieu);

                if (daDatVe)
                {
                    TempData["ErrorMessage"] = "Don nay da duoc thanh toan.";
                    return RedirectToAction("ThanhToan");
                }
            }

            var suatChieu = await _context.SuatChieu
                .Include(sc => sc.Phim)
                .FirstOrDefaultAsync(sc => sc.MaSuatChieu == maSuatChieu.Value);

            var donDatVe = new DonDatVeModel
            {
                MaNguoiDung = nguoiDung.Id,
                NgayDat = DateTime.Now,
            };
            _context.DonDatVe.Add(donDatVe);
            await _context.SaveChangesAsync();

            foreach (var gheId in danhSachGhe)
            {
                var gheSuatChieu = await _context.GheSuatChieu
                    .FirstOrDefaultAsync(g => g.MaGhe == gheId && g.MaSuatChieu == maSuatChieu);

                var chiTietDatVe = new ChiTietDatVeModel
                {
                    MaDonDatVe = donDatVe.MaDonDatVe,
                    MaGheSuatChieu = gheSuatChieu.MaGheSuatChieu,
                    GiaVe = tongTien.Value / danhSachGhe.Count
                };

                _context.ChiTietDatVe.Add(chiTietDatVe);

                gheSuatChieu.TrangThaiGhe = true;
            }

            await _context.SaveChangesAsync();

            TempData["ThanhToanMessage"] = "Thanh toán thành công!";
            return RedirectToAction("ThanhToan");
        }


        //Thanh toan VNPay
        [Authorize]
        public async Task<IActionResult> XacNhanThanhToan()
        {
            var maSuatChieu = HttpContext.Session.GetInt32("MaSuatChieu");
            var tongTien = HttpContext.Session.GetDecimal("TongTien");
            var gheDaChon = HttpContext.Session.GetString("GheDaChon");

            if (!maSuatChieu.HasValue || tongTien <= 0 || string.IsNullOrEmpty(gheDaChon))
            {
                return RedirectToAction("Index", "Home");
            }

            var nguoiDung = await _userManager.GetUserAsync(User);
            if (nguoiDung == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var danhSachGhe = gheDaChon.Split(',').Select(int.Parse).ToList();

            var suatChieu = await _context.SuatChieu
                .Include(sc => sc.Phim)
                .FirstOrDefaultAsync(sc => sc.MaSuatChieu == maSuatChieu.Value);

            if (suatChieu == null)
            {
                TempData["ErrorMessage"] = "Suat chieu khong ton tai.";
                return RedirectToAction("ThanhToan");
            }         

            var vnPayModel = new VnPaymentRequestModel
            {
                OrderId = new Random().Next(1000,10000).ToString(),
                FullName = nguoiDung.UserName,
                Description = $"Thanh toán vé cho phim: {suatChieu.Phim.TenPhim}",
                Amount = tongTien.Value,
                CreatedDate = DateTime.Now
            };
            return Redirect(_vnPayservice.CreatePaymentUrl(HttpContext, vnPayModel));
        }


        public IActionResult PaymentFail()
        {
            return View("PaymentFail");
        }
        public IActionResult PaymentSuccess()
        {
            return View("PaymentSuccess");
        }

        
        public async Task<IActionResult> PaymentCallBack()
        {
            var response = _vnPayservice.PaymentExecute(Request.Query);
            if (response == null || response.VnPayResponseCode != "00")
            {
                TempData["Message"] = $"Loi thanh toan VNPay: {response?.VnPayResponseCode}";
                return RedirectToAction("PaymentFail");
            }

            var maSuatChieu = HttpContext.Session.GetInt32("MaSuatChieu");
            var tongTien = HttpContext.Session.GetDecimal("TongTien");
            var gheDaChon = HttpContext.Session.GetString("GheDaChon");
            if (!maSuatChieu.HasValue || tongTien <= 0 || string.IsNullOrEmpty(gheDaChon))
            {
                TempData["Message"] = "Thông tin tin vé không h?p l?.";
                return RedirectToAction("PaymentFail");
            }

            var danhSachGhe = gheDaChon.Split(',').Select(int.Parse).ToList();
            var nguoiDung = await _userManager.GetUserAsync(User);
            if (nguoiDung == null)
            {
                TempData["Message"] = "Nguoi dung khong hop le.";
                return RedirectToAction("PaymentFail");
            }

            var donDatVe = new DonDatVeModel
            {
                MaNguoiDung = nguoiDung.Id,
                NgayDat = DateTime.Now,
            };
            _context.DonDatVe.Add(donDatVe); //Tao va luu don dat ve 
            await _context.SaveChangesAsync();

            foreach (var gheId in danhSachGhe)
            {
                var gheSuatChieu = _context.GheSuatChieu
                    .FirstOrDefault(g => g.MaGhe == gheId && g.MaSuatChieu == maSuatChieu);

                if (gheSuatChieu != null)
                {
                    var chiTietDatVe = new ChiTietDatVeModel
                    {
                        MaDonDatVe = donDatVe.MaDonDatVe,
                        MaGheSuatChieu = gheSuatChieu.MaGheSuatChieu,
                        GiaVe = tongTien.Value / danhSachGhe.Count 
                    };
                    _context.ChiTietDatVe.Add(chiTietDatVe); //Tao va luu chi tiet dat ve
                    gheSuatChieu.TrangThaiGhe = true; //Cap nhat trang thai ghe sau khi thanh cong
                }
            }
            await _context.SaveChangesAsync();

            TempData["Message"] = "Thanh toán VNPay thành công!";
            return RedirectToAction("PaymentSuccess");
        }

        
        //Danh sach don dat ve
        public IActionResult DonCuaToi()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var donCuaTois = _context.DonDatVe
                .Include(o => o.NguoiDung)
                .Where(o => o.MaNguoiDung == userId)
                .ToList();

            return View(donCuaTois);
        }

        //Chi tiet cua Don dat ve
        [Authorize]
        public async Task<IActionResult> ChiTietDonCuaToi(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chiTietDonCuaToi = _context.DonDatVe
                .Include(o => o.NguoiDung)
                .FirstOrDefault(o => o.MaDonDatVe == id && o.MaNguoiDung == User.FindFirstValue(ClaimTypes.NameIdentifier));

            if (chiTietDonCuaToi == null)
            {
                return NotFound();
            }

            var chiTietDatVe = _context.ChiTietDatVe
                .Where(od => od.MaDonDatVe == chiTietDonCuaToi.MaDonDatVe)
                .Include(od => od.GheSuatChieu)
                    .ThenInclude(gsc => gsc.Ghe)
                .Include(od => od.GheSuatChieu)
                    .ThenInclude(gsc => gsc.SuatChieu)
                        .ThenInclude(sc => sc.Phim)
                .Include(od => od.GheSuatChieu)
                    .ThenInclude(gsc => gsc.SuatChieu)
                        .ThenInclude(sc => sc.ThoiGianChieu)
                .Include(od => od.GheSuatChieu)
                    .ThenInclude(gsc => gsc.SuatChieu)
                        .ThenInclude(sc => sc.NgayChieu)
                .Include(od => od.GheSuatChieu)
                    .ThenInclude(gsc => gsc.SuatChieu)
                        .ThenInclude(sc => sc.Phong)
                            .ThenInclude(ph => ph.Rap)
                .ToList();

            ViewBag.ChiTietDatVe = chiTietDatVe;
            return View(chiTietDonCuaToi);
        }    
    }
}
