using System.ComponentModel.DataAnnotations;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeToanCongNoPhaiThu.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly UserManager<NguoiDung> _userManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<NguoiDung> signInManager,
                          UserManager<NguoiDung> userManager,
                          ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            public string TenDangNhapOrEmail { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            // Tìm user theo Email hoặc Tên đăng nhập
            var user = await _userManager.FindByEmailAsync(Input.TenDangNhapOrEmail)
                       ?? await _userManager.FindByNameAsync(Input.TenDangNhapOrEmail);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập/Email hoặc mật khẩu không đúng.");
                return Page();
            }

            // Kiểm tra trạng thái IsApproved cho Giám đốc và Kế toán
            var rolesRequiringApproval = new[] { "Giám đốc", "Kế toán" };
            foreach (var role in rolesRequiringApproval)
            {
                if (await _userManager.IsInRoleAsync(user, role) && !user.IsApproved)
                {
                    ModelState.AddModelError(string.Empty, $"Tài khoản {role.ToLower()} chưa được duyệt bởi Admin.");
                    return Page();
                }
            }

            // Đăng nhập
            var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                _logger.LogInformation("Đăng nhập thành công.");

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return LocalRedirect("/NguoiDungs/Index");

                if (await _userManager.IsInRoleAsync(user, "Giám đốc") ||
                    await _userManager.IsInRoleAsync(user, "Kế toán"))
                    return LocalRedirect("/HoaDons/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập/Email hoặc mật khẩu không đúng.");
            }

            return Page();
        }
    }
}
