using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Giám Đốc")]
    public class BaoCaoController : Controller
    {
        private readonly ReceivableDbContext _context;

        public BaoCaoController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: BaoCao/TongQuan
        public async Task<IActionResult> TongQuan()
        {
            var tongDoanhThu = await _context.HoaDons.SumAsync(h => h.TongTien);
            var tongThanhToan = await _context.ThanhToans.SumAsync(t => t.SoTienThanhToan);
            var tongNo = await _context.CongNos.SumAsync(c => c.SoTienNo);

            ViewBag.TongDoanhThu = tongDoanhThu;
            ViewBag.TongThanhToan = tongThanhToan;
            ViewBag.TongNo = tongNo;

            return View();
        }

        // GET: BaoCao/CongNoTheoKhachHang
        public async Task<IActionResult> CongNoTheoKhachHang()
        {
            var data = await _context.CongNos
                .Include(c => c.KhachHang)
                .GroupBy(c => c.MaKH)
                .Select(g => new
                {
                    KhachHang = g.First().KhachHang.TenKH,
                    TongNo = g.Sum(x => x.SoTienNo)
                }).ToListAsync();

            return View(data);
        }

        // GET: BaoCao/DoanhThuTheoThang
        public async Task<IActionResult> DoanhThuTheoThang()
        {
            var data = await _context.HoaDons
                .GroupBy(h => new { h.NgayLap.Month, h.NgayLap.Year })
                .Select(g => new
                {
                    Thang = g.Key.Month + "/" + g.Key.Year,
                    DoanhThu = g.Sum(x => x.TongTien)
                }).ToListAsync();

            return View(data);
        }

        public IActionResult KiemTraCongNoQuaHan()
        {
            var congNosQuaHan = _context.CongNos
                .Include(c => c.KhachHang)
                .Where(c => c.HanThanhToan < DateTime.Now && c.TrangThai != "Đã thanh toán")
                .ToList();

            foreach (var cn in congNosQuaHan)
            {
                var tb = new ThongBao
                {
                    MaTB = Guid.NewGuid().ToString(),
                    MaKH = cn.MaKH,
                    NoiDung = $"Khách hàng {cn.KhachHang.TenKH} đã quá hạn thanh toán công nợ {cn.MaCongNo}",
                    NgayGui = DateTime.Now
                };
                _context.ThongBaos.Add(tb);
            }

            _context.SaveChanges();
            return RedirectToAction("TongQuan");
        }

    }
}
