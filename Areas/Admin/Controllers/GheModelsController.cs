using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Wordprocessing;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GheModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GheModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/GheModels
        public async Task<IActionResult> Index(string searchTenPhong, int pageSize = 10, int pageNumber = 1)
        {
            // Bắt đầu truy vấn
            var gheQuery = _context.Ghe.Include(g => g.Phong).AsQueryable();

            // Kiểm tra nếu có từ khóa tìm kiếm
            if (!string.IsNullOrEmpty(searchTenPhong))
            {
                gheQuery = gheQuery.Where(g => g.Phong.TenPhong.Contains(searchTenPhong));
            }

            // Tính tổng số trang
            int totalItems = await gheQuery.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            // Lấy danh sách ghế cho trang hiện tại
            var gheList = await gheQuery
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Truyền dữ liệu cần thiết vào ViewData
            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentFilter"] = searchTenPhong; // Lưu lại bộ lọc hiện tại

            return View(gheList);
        }

        // GET: Admin/GheModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gheModel = await _context.Ghe
                .Include(g => g.Phong)
                .FirstOrDefaultAsync(m => m.MaGhe == id);
            if (gheModel == null)
            {
                return NotFound();
            }

            return View(gheModel);
        }

        // GET: Admin/GheModels/Create
        public IActionResult Create()
        {
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong");
            return View();
        }

        // POST: Admin/GheModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GheModel gheModel, string TenGhe, int SoLuong, int MaPhong)
        {
            ModelState.Clear();

            if (SoLuong > 0 && !string.IsNullOrEmpty(TenGhe))
            {
                for (int i = 1; i <= SoLuong; i++)
                {
                    // Tạo tên ghế theo định dạng TenGhe + số thứ tự (A1, A2,...)
                    var gheMoi = new GheModel
                    {
                        TenGhe = $"{TenGhe}{i}",
                        MaPhong = MaPhong
                    };

                    // Thêm ghế vào database
                    _context.Add(gheMoi);
                }

                // Lưu thay đổi vào database
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", MaPhong);
            return View();
        }

        // GET: Admin/GheModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gheModel = await _context.Ghe.FindAsync(id);
            if (gheModel == null)
            {
                return NotFound();
            }
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", gheModel.MaPhong);
            return View(gheModel);
        }

        // POST: Admin/GheModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaGhe,TenGhe,MaPhong")] GheModel gheModel)
        {
            if (id != gheModel.MaGhe)
            {
                return NotFound();
            }
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gheModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GheModelExists(gheModel.MaGhe))
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
            ViewData["MaPhong"] = new SelectList(_context.PhongModel, "MaPhong", "TenPhong", gheModel.MaPhong);
            return View(gheModel);
        }

        // GET: Admin/GheModels/Delete/5
        public async Task<IActionResult> Delete(int? maPhong)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            //var gheModel = await _context.Ghe
            //    .Include(g => g.Phong)
            //    .FirstOrDefaultAsync(m => m.MaGhe == id);
            //if (gheModel == null)
            //{
            //    return NotFound();
            //}

            //return View(gheModel);

            if (maPhong == null)
            {
                return NotFound();
            }

            // Kiểm tra xem có ghế nào trong phòng không
            var gheList = await _context.Ghe
                .Include(g => g.Phong)
                .Where(g => g.MaPhong == maPhong)
                .ToListAsync();

            if (gheList.Count == 0)
            {
                return NotFound(); // Không tìm thấy ghế nào trong phòng
            }

            return View(gheList); // Trả về danh sách ghế để xác nhận xóa
        }

        // POST: Admin/GheModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int maPhong)
        {
            //var gheModel = await _context.Ghe.FindAsync(id);
            //if (gheModel != null)
            //{
            //    _context.Ghe.Remove(gheModel);
            //}

            //await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));

            // Lấy tất cả ghế trong phòng theo mã phòng
            var gheList = await _context.Ghe
                .Where(g => g.MaPhong == maPhong)
                .ToListAsync();

            if (gheList.Count > 0)
            {
                _context.Ghe.RemoveRange(gheList); // Xóa tất cả ghế
            }

            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
            return RedirectToAction(nameof(Index));
        }

        private bool GheModelExists(int id)
        {
            return _context.Ghe.Any(e => e.MaGhe == id);
        }
    }
}
