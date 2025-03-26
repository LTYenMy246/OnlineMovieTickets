using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<AppNguoiDung> _userManager;
        ApplicationDbContext _db;
        public UserController(UserManager<AppNguoiDung> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public async Task<IActionResult> Index(string searchTenDN, bool? isActive, int pageNumber = 1, int pageSize = 6)
        {
            var users = _db.Users.OfType<AppNguoiDung>().AsQueryable();
            if (!string.IsNullOrEmpty(searchTenDN))
            {
                users = users.Where(u => u.UserName.Contains(searchTenDN, StringComparison.OrdinalIgnoreCase));
            }
            if (isActive.HasValue)
            {
                if (isActive.Value)
                {
                    users = users.Where(u => !u.LockoutEnabled || (u.LockoutEnd == null || u.LockoutEnd <= DateTime.Now));
                }
                else
                {
                    users = users.Where(u => u.LockoutEnabled && (u.LockoutEnd != null && u.LockoutEnd > DateTime.Now));
                }
            }
            int totalItems = await users.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            var pagedResults = await users
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewData["CurrentPage"] = pageNumber;
            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentFilter"] = searchTenDN;
            ViewData["IsActiveFilter"] = isActive;
            return View(pagedResults);
        }

        // Get
        public async Task<IActionResult> Details(string id)
        {
            var user = _db.AppNguoiDung.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Get
        public async Task<IActionResult> Lockout(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _db.AppNguoiDung.FirstOrDefault(y => y.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            if (TempData.ContainsKey("adminLockError"))
            {
                ViewBag.AdminLockError = TempData["adminLockError"];
            }

            return View(user);
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Lockout(AppNguoiDung user, string id)
        {
            var userInfo = _db.AppNguoiDung.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }

            if (await _userManager.IsInRoleAsync(userInfo, "Admin"))
            {
                var allUsers = await _userManager.Users.ToListAsync();
                var activeAdminCount = allUsers.Count(u => u.LockoutEnd == null && _userManager.IsInRoleAsync(u, "Admin").Result);
                if (activeAdminCount <= 1)
                {
                    TempData["adminLockError"] = "Cannot lock out the only active admin user.";
                    return RedirectToAction(nameof(Lockout), new { id });
                }
            }

            userInfo.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["lockout"] = "User has been locked out";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // Get
        public async Task<IActionResult> Create(string id)
        {
            return View();
        }

        // Post
        [HttpPost]
        public async Task<IActionResult>Create(AppNguoiDung user)
        {

            var existingUser = await _userManager.FindByNameAsync(user.UserName);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "Tên người dùng đã tồn tại.";
                return View();
            }
            // Tạo người dùng mới với mật khẩu
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Người dùng đã được tạo thành công!";
                ModelState.Clear();
                return View();
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code == "PasswordTooShort")
                    {
                        TempData["ErrorMessage"] = "Mật khẩu quá ngắn. Vui lòng nhập mật khẩu có ít nhất 6 kí tự.";
                    }
                    else if (error.Code == "PasswordRequiresNonAlphanumeric")
                    {
                        TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất một ký tự đặc biệt.";
                    }
                    else if (error.Code == "PasswordRequiresUpper")
                    {
                        TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất một ký tự viết hoa.";
                    }
                    else if (error.Code == "PasswordRequiresLower")
                    {
                        TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất một ký tự viết thường.";
                    }
                    else if (error.Code == "PasswordRequiresDigit")
                    {
                        TempData["ErrorMessage"] = "Mật khẩu phải có ít nhất một chữ số.";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = error.Description;
                    }
                }
            }         
            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        // Get
        public async Task<IActionResult> Active(string id)
        {
            var user = _db.AppNguoiDung.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Active(AppNguoiDung user)
        {
            var userInfo = _db.AppNguoiDung.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["lockout"] = "User has been reactivated";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        // Get
        public async Task<IActionResult> Delete(string id)
        {
            var user = _db.AppNguoiDung.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Post
        [HttpPost]
        public async Task<IActionResult> Delete(AppNguoiDung user)
        {
            var userInfo = _db.AppNguoiDung.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            _db.AppNguoiDung.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["delete"] = "User has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }
    }
}
