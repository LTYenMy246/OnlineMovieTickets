using Microsoft.AspNetCore.Mvc;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    public class DatVe : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
