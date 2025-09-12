using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public string MaCTHD { get; set; }

        [ForeignKey("HoaDon")]
        public string MaHD { get; set; }
        public HoaDon HoaDon { get; set; }
        public string NoiDung { get; set; }

        public decimal SoTien { get; set; }
    }
}
