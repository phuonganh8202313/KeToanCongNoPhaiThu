using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Identity;

public static class SeedData
{
    public static async Task Initialize(UserManager<NguoiDung> userManager, RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { "Admin", "Kế Toán", "Giám đốc" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var adminEmail = "admin@system.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new NguoiDung
            {
                UserName = "admin",
                TenDangNhap = "admin",
                Email = adminEmail,
                EmailConfirmed = true,
                HoTen = "Quản trị hệ thống",
                NgaySinh = new DateTime(1990, 1, 1),
                VaiTro = "Admin"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else
            {
                throw new Exception("Không tạo được user Admin: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
