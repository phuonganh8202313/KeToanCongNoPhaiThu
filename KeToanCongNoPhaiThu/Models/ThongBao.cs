using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{
    public class ThongBao
    {
        [Key]
        public int MaTB { get; set; }

        public string NoiDung { get; set; }
        public DateTime NgayGui { get; set; }

        [ForeignKey("KhachHang")]
        public int MaKH { get; set; }
        public KhachHang KhachHang { get; set; }
    }
}
