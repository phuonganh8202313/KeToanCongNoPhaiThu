using Microsoft.AspNetCore.Identity;

namespace KeToanCongNoPhaiThu.Models
{
    public class NguoiDung : IdentityUser
    {
        public string TenDangNhap { get; set; }
        public string VaiTro { get; set; } // admin, ke toan, giam doc
        public string HoTen { get; set; }
        public DateTime? NgaySinh { get; set; }
        public bool IsApproved { get; set; } = true; // mặc định true, riêng Giám đốc thì set false
    }
}
