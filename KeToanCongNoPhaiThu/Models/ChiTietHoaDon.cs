using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class ChiTietHoaDon
    {
        [Key]
        public int MaCTHD { get; set; }

        [ForeignKey("HoaDon")]
        public int MaHD { get; set; }
        public HoaDon HoaDon { get; set; }

        [MaxLength(200)]
        public string NoiDung { get; set; }

        public decimal SoTien { get; set; }
    }
}
