using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeToanCongNoPhaiThu.Models
{ 
    public class ThanhToan
{
    [Key]
    public string MaTT { get; set; }

    public DateTime NgayThanhToan { get; set; }
    public string NoiDung { get; set; }
    public decimal SoTienThanhToan { get; set; }

    [ForeignKey("NhanVien")]
    public string NguoiGhiNhanId { get; set; }
    [ValidateNever]
    public NhanVien? NhanVien { get; set; }

    [ForeignKey("HoaDon")]
    public string MaHD { get; set; }
    [ValidateNever]
    public HoaDon? HoaDon { get; set; }

    [Required]
    public string HinhThuc { get; set; } // "Tiền mặt" hoặc "Chuyển khoản"

    // Nếu hình thức chuyển khoản
    public string? NganHangNhan { get; set; }
    public string? TaiKhoanNhan { get; set; }
}
}
