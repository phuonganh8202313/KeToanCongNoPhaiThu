using Microsoft.EntityFrameworkCore;
using KeToanCongNoPhaiThu.Models;

namespace KeToanCongNoPhaiThu.Data
{
    public class ReceivableDbContext : DbContext
    {
        public ReceivableDbContext(DbContextOptions<ReceivableDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<CongNo> CongNos { get; set; }
        public DbSet<ThanhToan> ThanhToans { get; set; }
        public DbSet<ThongBao> ThongBaos { get; set; }
    }
}
