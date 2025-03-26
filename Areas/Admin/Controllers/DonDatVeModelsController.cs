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
    public class DonDatVeModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonDatVeModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/DonDatVeModels
        public async Task<IActionResult> Index(DateTime? screeningDate, int pageSize = 10, int pageNumber = 1)
        {
            var applicationDbContext = _context.DonDatVe
                .Include(s => s.NguoiDung)
                .AsQueryable();

            if (screeningDate.HasValue)
            {
                applicationDbContext = applicationDbContext.Where(d => d.NgayDat.Date == screeningDate.Value.Date);
            }

            int totalItems = await applicationDbContext.CountAsync();

            if (totalItems == 0)
            {
                ViewData["CurrentPage"] = 0;
                ViewData["TotalPages"] = 0;
                ViewData["PageSize"] = pageSize;
                ViewBag.ScreeningDate = screeningDate;
                return View(new List<DonDatVeModel>());
            }
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (pageNumber < 1)
            {
                pageNumber = 1;
            }
            else if (pageNumber > totalPages)
            {
                pageNumber = totalPages;
            }

            var donDatVeList = await applicationDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            ViewBag.ScreeningDate = screeningDate;

            return View(donDatVeList);
        }
        // GET: Admin/DonDatVeModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var donDatVeModel = await _context.DonDatVe
                .Include(d => d.NguoiDung)
                .FirstOrDefaultAsync(m => m.MaDonDatVe == id);
            if (donDatVeModel == null)
            {
                return NotFound();
            }

            return View(donDatVeModel);
        }

        // POST: Admin/DonDatVeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var donDatVeModel = await _context.DonDatVe.FindAsync(id);
            if (donDatVeModel != null)
            {
                _context.DonDatVe.Remove(donDatVeModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DonDatVeModelExists(int id)
        {
            return _context.DonDatVe.Any(e => e.MaDonDatVe == id);
        }
    }
}
