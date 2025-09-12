using System.ComponentModel.DataAnnotations;

namespace KeToanCongNoPhaiThu.Models
{
    public class KhachHang
    {
        [Key]
        public int MaKH { get; set; }

        [Required, MaxLength(30)]
        public string TenKH { get; set; }

        [MaxLength(200)]
        public string DiaChi { get; set; }

        [MaxLength(20)]
        public string SDT { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        public ICollection<CongNo> CongNos { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
        public ICollection<ThongBao> ThongBaos { get; set; }
    }
}
