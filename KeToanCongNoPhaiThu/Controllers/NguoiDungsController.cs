using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NguoiDungsController : Controller
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public NguoiDungsController(UserManager<NguoiDung> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Users
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewData["Roles"] = new SelectList(roles);
            return View();
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string email, string password, string fullName, string tenDangNhap, string role)
        {
            if (!ModelState.IsValid)
                return View();

            var user = new NguoiDung
            {
                UserName = string.IsNullOrEmpty(tenDangNhap) ? email : tenDangNhap,  // tên đăng nhập
                TenDangNhap = string.IsNullOrEmpty(tenDangNhap) ? email : tenDangNhap,
                Email = email,
                HoTen = fullName,
                VaiTro = role,
                IsApproved = role == "Admin" ? true : false // Admin tự duyệt
            };

            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View();
            }

            if (!string.IsNullOrEmpty(role))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));

                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            var userRoles = await _userManager.GetRolesAsync(user);
            ViewData["Roles"] = new SelectList(roles, userRoles.FirstOrDefault());

            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string fullName, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            user.HoTen = string.IsNullOrWhiteSpace(fullName) ? "Chưa có tên" : fullName;

            // Nếu đổi vai trò, reset IsApproved nếu không phải Admin
            if (user.VaiTro != role)
            {
                user.VaiTro = role;
                if (role != "Admin")
                    user.IsApproved = false;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(user);
            }

            // Cập nhật Role
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!string.IsNullOrEmpty(role))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole(role));
                await _userManager.AddToRoleAsync(user, role);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
                await _userManager.DeleteAsync(user);

            return RedirectToAction(nameof(Index));
        }

        // Admin duyệt Giám đốc hoặc Kế toán
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (user.VaiTro == "Giám đốc" || user.VaiTro == "Kế toán")
            {
                user.IsApproved = true;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
