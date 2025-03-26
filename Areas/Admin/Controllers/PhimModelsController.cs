using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;
using System.Drawing.Printing;
using System.Linq;
using PdfSharp.Drawing;
using System.IO;
using DocumentFormat.OpenXml.Wordprocessing;

[Area("Admin")]
public class PhimModelsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public PhimModelsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    // GET: Admin/PhimModels
    public async Task<IActionResult> Index(string searchTen, bool? isReleased, int pageNumber = 1, int? pageSize = null)
    {
        //int pageSize = 6; // Số lượng phim mỗi trang
        pageSize ??= 6; // Số lượng phim mỗi trang

        //Lấy danh sách phim từ cơ sở dữ liệu
        var phimQuery = from p in _context.Phim
                       select p;

        //Tìm kiếm, lọc theo tên phim
        if (!String.IsNullOrEmpty(searchTen))
        {
            phimQuery = phimQuery.Where(p => p.TenPhim.Contains(searchTen));
        }

        // Lọc theo trạng thái công chiếu, 
        if (isReleased.HasValue)
        {
            phimQuery = phimQuery.Where(p => p.TinhTrang == isReleased.Value);
        }

        //Tính tổng số trang
        int totalItems = await phimQuery.CountAsync();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        // Lấy danh sách phim cho trang hiện tại
        //var phimList = await phimQuery
        //    .Skip((pageNumber - 1) * pageSize)
        //    .Take(pageSize)
        //    .ToListAsync();

        // Lấy danh sách phim cho trang hiện tại
        var phimList = await phimQuery
            .Skip((pageNumber - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();

        //Truyền giá trị tìm kiếm vào ViewData để hiển thị lại trong form tìm kiếm
        ViewData["CurrentFilter"] = searchTen;
        ViewData["CurrentPage"] = pageNumber;
        ViewData["TotalPages"] = totalPages;
        ViewData["CurrentPageSize"] = pageSize; // Thêm vào đây
        return View(phimList);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phimModel = await _context.Phim
            .FirstOrDefaultAsync(m => m.MaPhim == id);
        if (phimModel == null)
        {
            return NotFound();
        }

        return View(phimModel);
    }

    // GET: Admin/PhimModels/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/PhimModels/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhimModel phimModel, IFormFile anhPhim /*IFormFile trailer*/)
    {
        ModelState.Clear();
        if (ModelState.IsValid)
        {
            // Kiểm tra tên phim có trùng không
            var isDuplicate = _context.Phim.FirstOrDefault(p => p.TenPhim.ToLower() == phimModel.TenPhim.ToLower());

            if (isDuplicate != null)
            {
                ModelState.AddModelError("TenPhim", "Tên phim đã tồn tại.");
                return View(phimModel);
            }

            //Anh phim
            if (anhPhim != null && anhPhim.Length > 0)
            {
                string fileName = Path.GetFileName(anhPhim.FileName);
                string filePath = Path.Combine(_env.WebRootPath, "images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhPhim.CopyToAsync(stream);
                }
                phimModel.AnhPhim = $"images/{fileName}";
            }

            _context.Add(phimModel);
            await _context.SaveChangesAsync();
            TempData["create"] = "Phim đã được tạo thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(phimModel);
    }

    // GET: Admin/PhimModels/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phimModel = await _context.Phim.FindAsync(id);
        if (phimModel == null)
        {
            return NotFound();
        }
        return View(phimModel);
    }

    // POST: Admin/PhimModels/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PhimModel phimModel, IFormFile anhPhim, IFormFile trailer)
    {
        var findProd = _context.Phim.Where(c => c.MaPhim == phimModel.MaPhim).AsNoTracking().FirstOrDefault();
        if (findProd == null)
        {
            return NotFound();
        }

        ModelState.Clear();
        if (ModelState.IsValid)
        {
            // Xử lý ảnh phim
            if (anhPhim != null && anhPhim.Length > 0)
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", Path.GetFileName(anhPhim.FileName));
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await anhPhim.CopyToAsync(stream);
                }
                phimModel.AnhPhim = "images/" + anhPhim.FileName; // Đường dẫn mới cho ảnh phim
            }
            else
            {
                // Nếu không upload ảnh mới, giữ nguyên ảnh cũ
                phimModel.AnhPhim = findProd.AnhPhim;
            }


            // Xử lý trailer phim
            if (trailer != null && trailer.Length > 0)
            {
                var trailerPath = Path.Combine(_env.WebRootPath, "videos", Path.GetFileName(trailer.FileName));
                using (var stream = new FileStream(trailerPath, FileMode.Create))
                {
                    await trailer.CopyToAsync(stream);
                }
                phimModel.TrailerPhim = "/videos/" + trailer.FileName; // Đường dẫn mới cho trailer
            }
            else
            {
                // Nếu không upload trailer mới, giữ nguyên trailer cũ
                phimModel.TrailerPhim = findProd.TrailerPhim;
            }

            try
            {
                _context.Update(phimModel);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Phim.Any(e => e.MaPhim == phimModel.MaPhim))
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
        return View(phimModel);
    }

    // GET: Admin/PhimModels/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var phimModel = await _context.Phim
            .FirstOrDefaultAsync(m => m.MaPhim == id);
        if (phimModel == null)
        {
            return NotFound();
        }

        return View(phimModel);
    }

    // POST: Admin/PhimModels/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var phimModel = await _context.Phim.FindAsync(id);
        if (phimModel != null)
        {
            _context.Phim.Remove(phimModel);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PhimModelExists(int id)
    {
        return _context.Phim.Any(e => e.MaPhim == id);
    }
}
