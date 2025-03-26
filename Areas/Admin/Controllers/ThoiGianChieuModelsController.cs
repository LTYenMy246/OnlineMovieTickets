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
    public class ThoiGianChieuModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ThoiGianChieuModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ThoiGianChieuModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.ThoiGianChieu.ToListAsync());
        }

        // GET: Admin/ThoiGianChieuModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thoiGianChieuModel = await _context.ThoiGianChieu
                .FirstOrDefaultAsync(m => m.MaThoiGianChieu == id);
            if (thoiGianChieuModel == null)
            {
                return NotFound();
            }

            return View(thoiGianChieuModel);
        }

        // GET: Admin/ThoiGianChieuModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ThoiGianChieuModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaThoiGianChieu,ThoiGianChieu")] ThoiGianChieuModel thoiGianChieuModel)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng lặp
                bool isDuplicate = await _context.ThoiGianChieu
                    .AnyAsync(t => t.ThoiGianChieu == thoiGianChieuModel.ThoiGianChieu
                               && t.MaThoiGianChieu != thoiGianChieuModel.MaThoiGianChieu);

                if (isDuplicate)
                {
                    ModelState.AddModelError(string.Empty, "Thời gian chiếu đã tồn tại.");
                    return View(thoiGianChieuModel);
                }

                _context.Add(thoiGianChieuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(thoiGianChieuModel);
        }

        // GET: Admin/ThoiGianChieuModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thoiGianChieuModel = await _context.ThoiGianChieu.FindAsync(id);
            if (thoiGianChieuModel == null)
            {
                return NotFound();
            }
            return View(thoiGianChieuModel);
        }

        // POST: Admin/ThoiGianChieuModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaThoiGianChieu,ThoiGianChieu")] ThoiGianChieuModel thoiGianChieuModel)
        {
            if (id != thoiGianChieuModel.MaThoiGianChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thoiGianChieuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThoiGianChieuModelExists(thoiGianChieuModel.MaThoiGianChieu))
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
            return View(thoiGianChieuModel);
        }

        // GET: Admin/ThoiGianChieuModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thoiGianChieuModel = await _context.ThoiGianChieu
                .FirstOrDefaultAsync(m => m.MaThoiGianChieu == id);
            if (thoiGianChieuModel == null)
            {
                return NotFound();
            }

            return View(thoiGianChieuModel);
        }

        // POST: Admin/ThoiGianChieuModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thoiGianChieuModel = await _context.ThoiGianChieu.FindAsync(id);
            if (thoiGianChieuModel != null)
            {
                _context.ThoiGianChieu.Remove(thoiGianChieuModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThoiGianChieuModelExists(int id)
        {
            return _context.ThoiGianChieu.Any(e => e.MaThoiGianChieu == id);
        }
    }
}
