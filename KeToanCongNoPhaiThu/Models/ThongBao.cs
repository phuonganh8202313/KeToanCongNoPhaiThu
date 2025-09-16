using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class ThongBao
    {
        [Key]
        public string MaTB { get; set; }

        public string NoiDung { get; set; }
        public DateTime NgayGui { get; set; }
        public string LoaiThongBao { get; set; } // Quá hạn, Thanh toán, Nhắc nhở
        public bool DaDoc { get; set; } = false;

        [ForeignKey("KhachHang")]
        public string MaKH { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
