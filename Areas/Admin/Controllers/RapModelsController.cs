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
    public class RapModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RapModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/RapModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rap.ToListAsync());
        }

        // GET: Admin/RapModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rapModel = await _context.Rap
                .FirstOrDefaultAsync(m => m.MaRap == id);
            if (rapModel == null)
            {
                return NotFound();
            }

            return View(rapModel);
        }

        // GET: Admin/RapModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/RapModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaRap,TenRap,DiaChiRap")] RapModel rapModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rapModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rapModel);
        }

        // GET: Admin/RapModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rapModel = await _context.Rap.FindAsync(id);
            if (rapModel == null)
            {
                return NotFound();
            }
            return View(rapModel);
        }

        // POST: Admin/RapModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaRap,TenRap,DiaChiRap")] RapModel rapModel)
        {
            if (id != rapModel.MaRap)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rapModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RapModelExists(rapModel.MaRap))
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
            return View(rapModel);
        }

        // GET: Admin/RapModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rapModel = await _context.Rap
                .FirstOrDefaultAsync(m => m.MaRap == id);
            if (rapModel == null)
            {
                return NotFound();
            }

            return View(rapModel);
        }

        // POST: Admin/RapModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rapModel = await _context.Rap.FindAsync(id);
            if (rapModel != null)
            {
                _context.Rap.Remove(rapModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RapModelExists(int id)
        {
            return _context.Rap.Any(e => e.MaRap == id);
        }
    }
}
