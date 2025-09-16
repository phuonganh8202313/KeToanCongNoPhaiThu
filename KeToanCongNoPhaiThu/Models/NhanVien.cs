using System.ComponentModel.DataAnnotations;

namespace KeToanCongNoPhaiThu.Models
{
    public class NhanVien
    {
        [Key]
        public string MaNV { get; set; }

        [Required]
        public string TenNV { get; set; }

        // có thể thêm các thông tin khác như email, số điện thoại...
    }
}
