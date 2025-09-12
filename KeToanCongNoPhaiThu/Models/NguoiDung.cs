using System.ComponentModel.DataAnnotations;

namespace KeToanCongNoPhaiThu.Models
{
    public class NguoiDung
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(50)]
        public string TenDangNhap { get; set; }

        [Required, MaxLength(100)]
        public string MatKhau { get; set; }

        [Required, MaxLength(20)]
        public string VaiTro { get; set; } // admin, ke toan, giam doc
    }
}
