using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class HoaDon
    {
        [Key]
        public int MaHD { get; set; }

        public DateTime NgayLap { get; set; }
        public decimal TongTien { get; set; }
        public string TrangThai { get; set; }

        [ForeignKey("KhachHang")]
        public int MaKH { get; set; }
        public KhachHang KhachHang { get; set; }

        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public ICollection<ThanhToan> ThanhToans { get; set; }

    }
}
