using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// 1. Cấu hình DbContext
// -------------------------
builder.Services.AddDbContext<ReceivableDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// -------------------------
// 2. Cấu hình Identity (User + Role)
// -------------------------
builder.Services.AddIdentity<NguoiDung, IdentityRole>(options =>
{
    // SignIn
    options.SignIn.RequireConfirmedAccount = true;

    // Password
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ReceivableDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI(); // 👉 để scaffold UI Identity hoạt động

// -------------------------
// 3. Thêm MVC + Razor Pages
// -------------------------
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// -------------------------
// 4. Build App
// -------------------------
var app = builder.Build();

// -------------------------
// 5. Seed dữ liệu (roles + users)
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<NguoiDung>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await SeedData.Initialize(userManager, roleManager);
}

// -------------------------
// 6. Middleware
// -------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// -------------------------
// 7. Map routes
// -------------------------

// Route MVC cho controller bình thường
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Route cho Razor Pages (Identity)
app.MapRazorPages();

// Redirect gốc "/" → Login
app.MapGet("/", context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    return Task.CompletedTask;
});

app.Run();
