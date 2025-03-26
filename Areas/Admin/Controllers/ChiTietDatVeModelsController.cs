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
    public class ChiTietDatVeModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChiTietDatVeModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ChiTietDatVeModels
        public async Task<IActionResult> Index(string searchMaDonDatVe, int pageSize = 10, int pageNumber = 1)
        {
            var applicationDbContext = _context.ChiTietDatVe
                .Include(c => c.DonDatVe)
                .Include(c => c.GheSuatChieu)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchMaDonDatVe))
            {
                applicationDbContext = applicationDbContext.Where(c => c.DonDatVe.MaDonDatVe.ToString().Contains(searchMaDonDatVe));
            }

            int totalItems = await applicationDbContext.CountAsync();
            if (totalItems == 0)
            {
                ViewData["CurrentPage"] = 0;
                ViewData["TotalPages"] = 0;
                ViewData["PageSize"] = pageSize;
                ViewBag.SearchMaDonDatVe = searchMaDonDatVe;
                return View(new List<ChiTietDatVeModel>());
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

            var chiTietDatVeList = await applicationDbContext
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["PageSize"] = pageSize;
            ViewBag.SearchMaDonDatVe = searchMaDonDatVe; 
            return View(chiTietDatVeList);
        }
    }
}
