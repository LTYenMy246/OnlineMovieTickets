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
    public class BinhLuanModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BinhLuanModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/BinhLuanModels
        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 6)
        {
            var applicationDbContext = _context.BinhLuan
                .Include(b => b.NguoiBinhLuan)
                .Include(b => b.Phim)
                .AsQueryable();

            int totalItems = await applicationDbContext.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var pagedResults = await applicationDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;

            return View(pagedResults);
        }

        // GET: Admin/BinhLuanModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var binhLuanModel = await _context.BinhLuan
                .Include(b => b.NguoiBinhLuan)
                .Include(b => b.Phim)
                .FirstOrDefaultAsync(m => m.MaBinhLuan == id);
            if (binhLuanModel == null)
            {
                return NotFound();
            }

            return View(binhLuanModel);
        }

        // POST: Admin/BinhLuanModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var binhLuanModel = await _context.BinhLuan.FindAsync(id);
            if (binhLuanModel != null)
            {
                _context.BinhLuan.Remove(binhLuanModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BinhLuanModelExists(int id)
        {
            return _context.BinhLuan.Any(e => e.MaBinhLuan == id);
        }
    }
}
