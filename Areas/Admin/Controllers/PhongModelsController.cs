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
    public class PhongModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PhongModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/PhongModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PhongModel.Include(p => p.Rap);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/PhongModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongModel = await _context.PhongModel
                .Include(p => p.Rap)
                .FirstOrDefaultAsync(m => m.MaPhong == id);
            if (phongModel == null)
            {
                return NotFound();
            }

            return View(phongModel);
        }

        // GET: Admin/PhongModels/Create
        public IActionResult Create()
        {
            ViewData["MaRap"] = new SelectList(_context.Rap, "MaRap", "TenRap");
            return View();
        }

        // POST: Admin/PhongModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaPhong,TenPhong,MaRap,SoLuongGhe")] PhongModel phongModel)
        {
            ModelState.Clear();
            if (ModelState.IsValid)
            {
                _context.Add(phongModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaRap"] = new SelectList(_context.Rap, "MaRap", "TenRap", phongModel.MaRap);
            return View(phongModel);
        }

        // GET: Admin/PhongModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongModel = await _context.PhongModel.FindAsync(id);
            if (phongModel == null)
            {
                return NotFound();
            }
            ViewData["MaRap"] = new SelectList(_context.Rap, "MaRap", "TenRap", phongModel.MaRap);
            return View(phongModel);
        }

        // POST: Admin/PhongModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaPhong,TenPhong,MaRap,SoLuongGhe")] PhongModel phongModel)
        {
            // Kiểm tra nếu id không khớp với mã phòng
            if (id != phongModel.MaPhong)
            {
                return NotFound();
            }
            ModelState.Clear();
            // Kiểm tra tính hợp lệ của model
            if (ModelState.IsValid)
            {
                try
                {
                    // Tìm phòng hiện tại trong cơ sở dữ liệu
                    var existingPhongModel = await _context.PhongModel.FindAsync(id);
                    if (existingPhongModel == null)
                    {
                        return NotFound();
                    }

                    // Cập nhật thông tin phòng
                    existingPhongModel.TenPhong = phongModel.TenPhong; // Cập nhật tên phòng
                    existingPhongModel.SoLuongGhe = phongModel.SoLuongGhe; // Cập nhật số lượng ghế

                    _context.Update(existingPhongModel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index)); // Chuyển hướng về danh sách phòng sau khi lưu thành công
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Nếu không tồn tại phòng với mã hiện tại
                    if (!PhongModelExists(phongModel.MaPhong))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["MaRap"] = new SelectList(_context.Rap, "MaRap", "TenRap", phongModel.MaRap);
            return View(phongModel); 
        }

        // GET: Admin/PhongModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phongModel = await _context.PhongModel
                .Include(p => p.Rap)
                .FirstOrDefaultAsync(m => m.MaPhong == id);
            if (phongModel == null)
            {
                return NotFound();
            }

            return View(phongModel);
        }

        // POST: Admin/PhongModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var phongModel = await _context.PhongModel.FindAsync(id);
            if (phongModel != null)
            {
                _context.PhongModel.Remove(phongModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhongModelExists(int id)
        {
            return _context.PhongModel.Any(e => e.MaPhong == id);
        }
    }
}
