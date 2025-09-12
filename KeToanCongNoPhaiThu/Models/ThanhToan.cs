using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class ThanhToan
    {
        [Key]
        public int MaTT { get; set; }

        public DateTime NgayThanhToan { get; set; }
        public string NoiDung { get; set; }
        public decimal SoTienThanhToan { get; set; }
        public string NguoiGhiNhan { get; set; }

        [ForeignKey("HoaDon")]
        public int MaHD { get; set; }
        public HoaDon HoaDon { get; set; }
    }
}
