using Microsoft.EntityFrameworkCore;
using KeToanCongNoPhaiThu.Models;

namespace KeToanCongNoPhaiThu.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }

        // Nếu có thêm bảng khác thì khai báo tiếp ở đây
    }
}
