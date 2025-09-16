using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq; // Cần cho Sum()

namespace KeToanCongNoPhaiThu.Models
{
    public class HoaDon
    {
        [Key]
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public string MaHD { get; set; }

        [DataType(DataType.Date)]
        public DateTime NgayLap { get; set; }

        [NotMapped]
        public decimal TongTien => ChiTietHoaDons?.Sum(c => c.SoTien) ?? 0;

        public string TrangThai { get; set; }

        [ForeignKey("KhachHang")]
        public string MaKH { get; set; }

        [ValidateNever]
        public KhachHang KhachHang { get; set; }

        [ValidateNever]
        public ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

        [ValidateNever]
        public ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
    }
}
