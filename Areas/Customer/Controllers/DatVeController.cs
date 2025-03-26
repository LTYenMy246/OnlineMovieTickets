using Microsoft.AspNetCore.Mvc;
using OnlineMovieTicket.Data;

namespace OnlineMovieTicket.Areas.Customer.Controllers
{
    public class DatVeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DatVeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var rapList = _context.Rap.ToList();
            ViewBag.Raps = rapList;

            return View();
        }

        [HttpPost]
        public IActionResult Dat(int rapId, int phongId, int suatChieuId, List<int> gheIds)
        {
            // Lưu logic đặt vé ở đây
            // Ví dụ: cập nhật trạng thái ghế đã được đặt
            var gheSuatChieu = _context.GheSuatChieu
                .Where(g => gheIds.Contains(g.MaGhe))
                .ToList();

            foreach (var ghe in gheSuatChieu)
            {
                ghe.TrangThaiGhe = true; // Đánh dấu ghế đã đặt
            }

            _context.SaveChanges();

            // Chuyển hướng đến trang xác nhận hoặc trang khác
            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }


    }


}
