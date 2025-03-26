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
    public class NgayChieuModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NgayChieuModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/NgayChieuModels
        public async Task<IActionResult> Index(int? month, int? year, int pageSize = 10, int pageNumber = 1)
        {
            var days = await _context.NgayChieu.ToListAsync();

            // Kiểm tra nếu month và year có giá trị
            if (month.HasValue && year.HasValue)
            {
                days = days.Where(d => d.NgayChieu.Month == month && d.NgayChieu.Year == year).ToList();
            }
            // Phân trang
            var totalItems = days.Count; // Tổng số bản ghi
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize); // Tổng số trang

            var pagedDays = days.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList(); // Lấy bản ghi cho trang hiện tại

            ViewBag.CurrentPage = pageNumber; // Trang hiện tại
            ViewBag.TotalPages = totalPages; // Tổng số trang

            return View(pagedDays);
        }

        // GET: Admin/NgayChieuModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngayChieuModel = await _context.NgayChieu
                .FirstOrDefaultAsync(m => m.MaNgayChieu == id);

            if (ngayChieuModel == null)
            {
                return NotFound();
            }

            return View(ngayChieuModel);
        }

        // GET: Admin/NgayChieuModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/NgayChieuModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaNgayChieu,NgayChieu")] NgayChieuModel ngayChieuModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp ngày chiếu
                bool isDuplicate = await _context.NgayChieu
                    .AnyAsync(n => n.NgayChieu == ngayChieuModel.NgayChieu && n.MaNgayChieu != ngayChieuModel.MaNgayChieu);

                if (isDuplicate)
                {
                    ModelState.AddModelError(string.Empty, "Ngày chiếu đã tồn tại.");
                    return View(ngayChieuModel);
                }

                if (ngayChieuModel.MaNgayChieu == 0)
                {
                    // Thêm mới
                    _context.Add(ngayChieuModel);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ngayChieuModel);
        }

        // GET: Admin/NgayChieuModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngayChieuModel = await _context.NgayChieu.FindAsync(id);
            if (ngayChieuModel == null)
            {
                return NotFound();
            }
            return View(ngayChieuModel);
        }

        // POST: Admin/NgayChieuModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaNgayChieu,NgayChieu")] NgayChieuModel ngayChieuModel)
        {
            if (id != ngayChieuModel.MaNgayChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ngayChieuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NgayChieuModelExists(ngayChieuModel.MaNgayChieu))
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
            return View(ngayChieuModel);
        }

        // GET: Admin/NgayChieuModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ngayChieuModel = await _context.NgayChieu
                .FirstOrDefaultAsync(m => m.MaNgayChieu == id);
            if (ngayChieuModel == null)
            {
                return NotFound();
            }

            return View(ngayChieuModel);
        }

        // POST: Admin/NgayChieuModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ngayChieuModel = await _context.NgayChieu.FindAsync(id);
            if (ngayChieuModel == null)
            {
                return NotFound();
            }
            _context.NgayChieu.Remove(ngayChieuModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NgayChieuModelExists(int id)
        {
            return _context.NgayChieu.Any(e => e.MaNgayChieu == id);
        }
    }
}
