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
    public class SuKienModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SuKienModelsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Admin/SuKienModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SuKien.Include(s => s.Phim);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/SuKienModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKienModel = await _context.SuKien
                .Include(s => s.Phim)
                .FirstOrDefaultAsync(m => m.MaSuKien == id);
            if (suKienModel == null)
            {
                return NotFound();
            }

            return View(suKienModel);
        }

        // GET: Admin/SuKienModels/Create
        public IActionResult Create()
        {
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim");
            return View();
        }

        // POST: Admin/SuKienModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSuKien,TenSuKien,AnhSuKien,NoiDungSuKien,NgayDangSuKien,MaPhim")] SuKienModel suKienModel, IFormFile anhSuKien)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                //Anh su kien
                if (anhSuKien != null && anhSuKien.Length > 0)
                {
                    string fileName = Path.GetFileName(anhSuKien.FileName);
                    string filePath = Path.Combine(_env.WebRootPath, "images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await anhSuKien.CopyToAsync(stream);
                    }
                    suKienModel.AnhSuKien = $"images/{fileName}";
                }
                _context.Add(suKienModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suKienModel.MaPhim);
            return View(suKienModel);
        }

        // GET: Admin/SuKienModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKienModel = await _context.SuKien.FindAsync(id);
            if (suKienModel == null)
            {
                return NotFound();
            }
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suKienModel.MaPhim);
            return View(suKienModel);
        }

        // POST: Admin/SuKienModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SuKienModel suKienModel, IFormFile anhSuKien)
        {
            var findProd = _context.SuKien.Where(c => c.MaSuKien == suKienModel.MaSuKien).AsNoTracking().FirstOrDefault();
            if (findProd == null)
            {
                return NotFound();
            }

            ModelState.Clear();
            if (ModelState.IsValid)
            {
                // Xử lý ảnh phim
                if (anhSuKien != null && anhSuKien.Length > 0)
                {
                    var imagePath = Path.Combine(_env.WebRootPath, "images", Path.GetFileName(anhSuKien.FileName));
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await anhSuKien.CopyToAsync(stream);
                    }
                    suKienModel.AnhSuKien = "images/" + anhSuKien.FileName; // Đường dẫn mới cho ảnh phim
                }
                else
                {
                    // Nếu không upload ảnh mới, giữ nguyên ảnh cũ
                    suKienModel.AnhSuKien = findProd.AnhSuKien;
                }
                try
                {
                    _context.Update(suKienModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuKienModelExists(suKienModel.MaSuKien))
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
            ViewData["MaPhim"] = new SelectList(_context.Phim, "MaPhim", "TenPhim", suKienModel.MaPhim);
            return View(suKienModel);
        }

        // GET: Admin/SuKienModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suKienModel = await _context.SuKien
                .Include(s => s.Phim)
                .FirstOrDefaultAsync(m => m.MaSuKien == id);
            if (suKienModel == null)
            {
                return NotFound();
            }

            return View(suKienModel);
        }

        // POST: Admin/SuKienModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suKienModel = await _context.SuKien.FindAsync(id);
            if (suKienModel != null)
            {
                _context.SuKien.Remove(suKienModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuKienModelExists(int id)
        {
            return _context.SuKien.Any(e => e.MaSuKien == id);
        }
    }
}
