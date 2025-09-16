using System.ComponentModel.DataAnnotations;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace KeToanCongNoPhaiThu.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<NguoiDung> _userManager;
        private readonly SignInManager<NguoiDung> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<NguoiDung> userManager,
            SignInManager<NguoiDung> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string TenDangNhap { get; set; }

            [Required]
            public string HoTen { get; set; }

            [DataType(DataType.Date)]
            public DateTime? NgaySinh { get; set; }

            [Required]
            public string VaiTro { get; set; }

            [Required, DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            if (!ModelState.IsValid) return Page();

            // Tạo user, đảm bảo các trường NOT NULL có giá trị
            var user = new NguoiDung
            {
                UserName = Input.TenDangNhap,
                TenDangNhap = Input.TenDangNhap,
                Email = Input.Email,
                HoTen = string.IsNullOrWhiteSpace(Input.HoTen) ? "Chưa có tên" : Input.HoTen,
                NgaySinh = Input.NgaySinh ?? DateTime.Now,
                VaiTro = Input.VaiTro,
                IsApproved = Input.VaiTro == "Admin"  // Admin tự duyệt, còn lại false
            };

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("Người dùng mới đã tạo tài khoản.");

                // Tạo role nếu chưa tồn tại
                if (!await _roleManager.RoleExistsAsync(Input.VaiTro))
                    await _roleManager.CreateAsync(new IdentityRole(Input.VaiTro));

                await _userManager.AddToRoleAsync(user, Input.VaiTro);

                if (Input.VaiTro == "Admin")
                {
                    // Admin tự login
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect("/NguoiDungs/Index");
                }
                else
                {
                    // Giám đốc / Kế toán chưa được duyệt → thông báo chờ duyệt
                    TempData["Message"] = "Tài khoản đã đăng ký. Vui lòng chờ Admin duyệt trước khi đăng nhập.";
                    return RedirectToPage("/Account/Login");
                }
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
