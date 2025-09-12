using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class CongNo
    {
        [Key]
        public int MaCongNo { get; set; }

        public decimal SoTienNo { get; set; }
        public DateTime HanThanhToan { get; set; }
        public string TrangThai { get; set; }

        [ForeignKey("KhachHang")]
        public int MaKH { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
