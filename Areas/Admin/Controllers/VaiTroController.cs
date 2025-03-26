using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineMovieTicket.Areas.Admin.Models;
using OnlineMovieTicket.Data;
using OnlineMovieTicket.Models;

namespace OnlineMovieTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class VaiTroController : Controller
    {
        RoleManager<IdentityRole> _roleManager;
        UserManager<AppNguoiDung> _userManager;
        ApplicationDbContext _db;
        public VaiTroController(RoleManager<IdentityRole> roleManager, UserManager<AppNguoiDung> userManager, ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            var roles= _roleManager.Roles.ToList();
            ViewBag.Roles = roles;
            return View();
        }
        // Get Create 
        public async Task<IActionResult> Create()
        {
            return View();
        }

        // Post Create 
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            IdentityRole role = new IdentityRole();
            role.Name = name;
            var isExist = await _roleManager.RoleExistsAsync(role.Name);
            if (isExist)
            {
                ViewBag.message = "vai trò đã tồn tại";
                ViewBag.name = name;
                return View();
            }
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                TempData["create"] = "Tạo vai trò thành công";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

		//Get Edit		
		public async Task<IActionResult>Edit(string id)
        {
            var role=await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;
            return View();
        }

		//Post Edit
		[HttpPost]
		public async Task<IActionResult> Edit(string id, string name)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role == null)
			{
				return NotFound();
			}
            role.Name = name;
			var isExist = await _roleManager.RoleExistsAsync(role.Name);
			if (isExist)
			{
				ViewBag.message = "Vai trò đã tồn tại";
				ViewBag.name = name;
				return View();
			}
			var result = await _roleManager.UpdateAsync(role);
			if (result.Succeeded)
			{
				TempData["create"] = "Cập nhật vai trò thành công";
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		// Get Delete 
		public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.id = role.Id;
            ViewBag.name = role.Name;

            if (TempData.ContainsKey("deleteRoleError"))
            {
                ViewBag.DeleteRoleError = TempData["deleteRoleError"];
			}
            return View();
        }

        // Post Delete 
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            if (role.Name == "Admin" || role.Name == "User")
            {
                TempData["deleteRoleError"] = "Không thể xóa các vai trò cơ bản.";
				return RedirectToAction(nameof(Delete), new { id });
			}

			var userInRole = await _db.UserRoles.AnyAsync(c => c.RoleId == id);
            if (userInRole)
            {
                TempData["deleteRoleError"] = "Không thể xóa vai trò. Có những người dùng được liên kết với vai trò này.";

                return RedirectToAction(nameof(Delete), new { id });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
			TempData["deleteRoleError"] = "Đã xảy ra lỗi khi cố gắng xóa vai trò.";
			return View();
        }

		// Get Assign action method
		public async Task<IActionResult> Assign()
		{
			ViewData["UserId"] = new SelectList(_db.AppNguoiDung.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
			ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
			return View();
		}

		// Post Assign action method
		[HttpPost]
		public async Task<IActionResult> Assign(PhienNguoiDungVm roleUser)
		{
			var user = _db.AppNguoiDung.FirstOrDefault(c => c.Id == roleUser.UserId);
			if (user == null)
			{
				ViewBag.message = "Người dùng không tồn tại!";
				ViewData["UserId"] = new SelectList(_db.AppNguoiDung.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
				ViewData["RoleId"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
				return View();
			}

			var isCheckRoleAssign = await _userManager.IsInRoleAsync(user, roleUser.RoleId);
			if (isCheckRoleAssign)
			{
				ViewBag.message = "Người dùng này đã có vai trò này!";
				ViewData["UserID"] = new SelectList(_db.AppNguoiDung.Where(f => f.LockoutEnd < DateTime.Now || f.LockoutEnd == null).ToList(), "Id", "UserName");
				ViewData["RoleID"] = new SelectList(_roleManager.Roles.ToList(), "Name", "Name");
			}
			var role = await _userManager.AddToRoleAsync(user, roleUser.RoleId);
			if (role.Succeeded)
			{
				TempData["save"] = "Vai trò người dùng được chỉ định";
				return RedirectToAction(nameof(Index));
			}
			return View();
		}

		public ActionResult AssignUserRole()
		{
			var result = from u in _db.UserRoles
						 join r in _db.Roles on u.RoleId equals r.Id
						 join a in _db.AppNguoiDung on u.UserId equals a.Id
						 select new BanDoVaiTroND()
						 {
							 UserId = u.UserId,
							 RoleId = u.RoleId,
							 UserName = a.UserName,
							 RoleName = r.Name
						 };
			ViewBag.UserRoles = result;
			return View();
		}

	}
}
