using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GheSuatChieuModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GheSuatChieuModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GheSuatChieuModels
        public async Task<IActionResult> Index(string searchMaSuatChieu, int pageNumber = 1, int pageSize = 10)
        {
            ViewData["CurrentFilterSuatChieu"] = searchMaSuatChieu;
            var query = _context.GheSuatChieu
                .Include(gs => gs.Ghe)       
                .Include(gs => gs.SuatChieu) 
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchMaSuatChieu))
            {
                if (int.TryParse(searchMaSuatChieu, out int suatChieuId))
                {
                    query = query.Where(gs => gs.MaSuatChieu == suatChieuId);
                }
            }
            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var pagedResults = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentFilter"] = searchMaSuatChieu;
            return View(pagedResults);
        }

		public async Task<IActionResult> Create()
		{
            var suatChieuDaCoGhe = await _context.GheSuatChieu
                .Select(gsc => gsc.MaSuatChieu)
                .Distinct()
                .ToListAsync();

            var suatChieuList = await _context.SuatChieu
                .Where(s => !suatChieuDaCoGhe.Contains(s.MaSuatChieu))
                .Select(s => new SelectListItem
                {
                    Value = s.MaSuatChieu.ToString(),
                    Text = $"{s.Phim.TenPhim} - {s.Phong.TenPhong} - {s.ThoiGianChieu.ThoiGianChieu} - {s.NgayChieu.NgayChieu}"
                })
                .ToListAsync();

            ViewBag.SuatChieuList = suatChieuList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GheSuatChieuModel gheSuatChieuModel, int MaSuatChieu)
        {
            if (MaSuatChieu == 0)
            {
                var suatChieuList = await _context.SuatChieu.Include(s => s.Phong).ToListAsync();
                ViewBag.SuatChieuList = new SelectList(suatChieuList, "MaSuatChieu", "Phim.TenPhim");
                ModelState.AddModelError("MaSuatChieu", "Bạn phải chọn một suất chiếu.");
                return View();
            }

            // Lấy thông tin suất chiếu và phòng chiếu tương ứng
            var suatChieu = await _context.SuatChieu
                .Include(s => s.Phong)
                .FirstOrDefaultAsync(s => s.MaSuatChieu == MaSuatChieu);

            if (suatChieu == null)
            {
                return NotFound("Suất chiếu không tồn tại.");
            }
            // Lấy danh sách ghế của phòng chiếu
            var danhSachGhe = await _context.Ghe
                .Where(g => g.MaPhong == suatChieu.MaPhong)
                .ToListAsync();

            if (danhSachGhe == null || !danhSachGhe.Any())
            {
                ModelState.AddModelError("MaPhong", "Phòng chiếu này không có ghế.");
                return View();
            }
            // Tạo ghế suất chiếu cho tất cả các ghế trong phòng
            foreach (var ghe in danhSachGhe)
            {
                var gheSuatChieu = new GheSuatChieuModel
                {
                    MaSuatChieu = MaSuatChieu,
                    MaGhe = ghe.MaGhe,
                    TrangThaiGhe = false // Ghế mặc định là trống
                };

                _context.GheSuatChieu.Add(gheSuatChieu);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll(int maSuatChieu)
        {
            var gheSuatChieuList = await _context.GheSuatChieu
               .Where(g => g.MaSuatChieu == maSuatChieu)
               .ToListAsync();

            if (gheSuatChieuList.Count > 0)
            {
                _context.GheSuatChieu.RemoveRange(gheSuatChieuList);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
