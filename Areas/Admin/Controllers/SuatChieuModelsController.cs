using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SuatChieuModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuatChieuModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index(string movieName, DateTime? screeningDate, string roomName, int pageSize = 10, int pageNumber = 1)
        {
            var applicationDbContext = _context.SuatChieu
                .Include(s => s.NgayChieu)
                .Include(s => s.Phim)
                .Include(s => s.Phong)
                .Include(s => s.ThoiGianChieu)
                .AsQueryable();

            if (!string.IsNullOrEmpty(movieName))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Phim.TenPhim.Contains(movieName));
            }

            // Lọc theo ngày chiếu
            if (screeningDate.HasValue)
            {
                // Chuyển đổi DateTime thành DateOnly
                var screeningDateOnly = DateOnly.FromDateTime(screeningDate.Value);

                applicationDbContext = applicationDbContext.Where(s =>
                    s.NgayChieu != null &&
                    s.NgayChieu.NgayChieu == screeningDateOnly);
            }
            ViewBag.SelectedDate = screeningDate;


            if (!string.IsNullOrEmpty(roomName))
            {
                applicationDbContext = applicationDbContext.Where(s => s.Phong.TenPhong.Contains(roomName));
            }

            // Tính tổng số trang
            int totalItems = await applicationDbContext.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var suatChieuList = await applicationDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentFilter"] = movieName; 
            ViewBag.RoomName = roomName; 
            ViewBag.ScreeningDate = screeningDate;
            return View(suatChieuList);

        }

        // GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieuModel = await _context.SuatChieu
                .Include(s => s.NgayChieu)
                .Include(s => s.Phim)
                .Include(s => s.Phong)
                .Include(s => s.ThoiGianChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);
            if (suatChieuModel == null)
            {
                return NotFound();
            }

            return View(suatChieuModel);
        }

        // GET
        public IActionResult Create()
        {
            ViewData["MaNgayChieu"] = new SelectList(_context.NgayChieu, "MaNgayChieu", "NgayChieu");
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim");
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong");
            ViewData["MaThoiGianChieu"] = new SelectList(_context.ThoiGianChieu, "MaThoiGianChieu", "ThoiGianChieu");
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSuatChieu,MaPhim,MaPhong,MaNgayChieu,MaThoiGianChieu,GiaVeSuatChieu")] SuatChieuModel suatChieuModel)
        {         
            ModelState.Clear();
            // Kiểm tra nếu đã có suất chiếu với cùng phòng, thời gian chiếu và ngày chiếu
            var existingSuatChieu = await _context.SuatChieu
                .AnyAsync(sc => sc.MaPhong == suatChieuModel.MaPhong &&
                                sc.MaThoiGianChieu == suatChieuModel.MaThoiGianChieu &&
                                sc.MaNgayChieu == suatChieuModel.MaNgayChieu);

            if (existingSuatChieu)
            {
                ModelState.AddModelError(string.Empty, "Đã có suất chiếu trùng với phòng, thời gian chiếu và ngày chiếu.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(suatChieuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNgayChieu"] = new SelectList(_context.NgayChieu, "MaNgayChieu", "NgayChieu", suatChieuModel.MaNgayChieu);
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suatChieuModel.MaPhim);
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", suatChieuModel.MaPhong);
            ViewData["MaThoiGianChieu"] = new SelectList(_context.ThoiGianChieu, "MaThoiGianChieu", "ThoiGianChieu", suatChieuModel.MaThoiGianChieu);
            return View(suatChieuModel);
        }

        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieuModel = await _context.SuatChieu.FindAsync(id);
            if (suatChieuModel == null)
            {
                return NotFound();
            }
            ViewData["MaNgayChieu"] = new SelectList(_context.NgayChieu, "MaNgayChieu", "NgayChieu", suatChieuModel.MaNgayChieu);
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suatChieuModel.MaPhim);
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", suatChieuModel.MaPhong);
            ViewData["MaThoiGianChieu"] = new SelectList(_context.ThoiGianChieu, "MaThoiGianChieu", "ThoiGianChieu", suatChieuModel.MaThoiGianChieu);
            return View(suatChieuModel);
        }

        // POST
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSuatChieu,MaPhim,MaPhong,MaNgayChieu,MaThoiGianChieu,GiaVeSuatChieu")] SuatChieuModel suatChieuModel)
        {
            if (id != suatChieuModel.MaSuatChieu)
            {
                return NotFound();
            }
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suatChieuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuatChieuModelExists(suatChieuModel.MaSuatChieu))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaNgayChieu"] = new SelectList(_context.NgayChieu, "MaNgayChieu", "NgayChieu", suatChieuModel.MaNgayChieu);
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suatChieuModel.MaPhim);
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", suatChieuModel.MaPhong);
            ViewData["MaThoiGianChieu"] = new SelectList(_context.ThoiGianChieu, "MaThoiGianChieu", "ThoiGianChieu", suatChieuModel.MaThoiGianChieu);
            return View(suatChieuModel);
        }

        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suatChieuModel = await _context.SuatChieu
                .Include(s => s.NgayChieu)
                .Include(s => s.Phim)
                .Include(s => s.Phong)
                .Include(s => s.ThoiGianChieu)
                .FirstOrDefaultAsync(m => m.MaSuatChieu == id);
            if (suatChieuModel == null)
            {
                return NotFound();
            }

            return View(suatChieuModel);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suatChieuModel = await _context.SuatChieu.FindAsync(id);
            if (suatChieuModel != null)
            {
                _context.SuatChieu.Remove(suatChieuModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuatChieuModelExists(int id)
        {
            return _context.SuatChieu.Any(e => e.MaSuatChieu == id);
        }
    }
}
