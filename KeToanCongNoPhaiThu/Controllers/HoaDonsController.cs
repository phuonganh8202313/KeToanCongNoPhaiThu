using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Kế Toán,Giám đốc")]
    public class HoaDonsController : Controller
    {
        private readonly ReceivableDbContext _context;

        public HoaDonsController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: HoaDons
        public IActionResult Index()
        {
            var list = _context.HoaDons
                               .Include(h => h.KhachHang) // 👈 thêm Include
                               .ToList();
            return View(list);
        }

        // GET: HoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH");
            ViewData["TrangThaiList"] = new SelectList(new[] { "Đã thanh toán", "Chưa thanh toán", "Thanh toán một phần" });
            return View();
        }

        // POST: HoaDons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HoaDon hoaDon)
        {
            // Kiểm tra trùng MaHD
            if (_context.HoaDons.Any(h => h.MaHD == hoaDon.MaHD))
            {
                ModelState.AddModelError("MaHD", "Mã hóa đơn đã tồn tại");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.HoaDons.Add(hoaDon);

                    var congNo = new CongNo
                    {
                        MaCongNo = Guid.NewGuid().ToString(),
                        MaKH = hoaDon.MaKH,
                        SoTienNo = hoaDon.TongTien,
                        HanThanhToan = hoaDon.NgayLap.AddDays(30), // ví dụ hạn 30 ngày
                        TrangThai = "Chưa thanh toán"
                    };
                    _context.CongNos.Add(congNo);

                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index)); // thành công thì về Index
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi DB (ví dụ: format sai, null FK…)
                    ModelState.AddModelError("", "Không thể thêm hóa đơn: " + ex.Message);
                }
            }


            // Nếu có lỗi thì load lại dropdown
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewData["TrangThaiList"] = new SelectList(
                new[] { "Đã thanh toán", "Chưa thanh toán", "Thanh toán một phần" }, hoaDon.TrangThai
            );

            return View(hoaDon);
        }


        // GET: HoaDons/Edit/5
        public IActionResult Edit(string id)
        {
            var hd = _context.HoaDons
                     .Include(h => h.KhachHang) //thêm Include
                     .FirstOrDefault(h => h.MaHD == id);
            if (hd == null) return NotFound();

            // nạp lại dropdown khách hàng
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hd.MaKH);

            // nạp lại dropdown trạng thái
            ViewData["TrangThaiList"] = new SelectList(
                new[] { "Đã thanh toán", "Chưa thanh toán", "Thanh toán một phần" }, hd.TrangThai
            );

            return View(hd);
        }


        // POST: HoaDons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Update(hoaDon);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // nạp lại dropdown khi có lỗi
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
            ViewData["TrangThaiList"] = new SelectList(
                new[] { "Đã thanh toán", "Chưa thanh toán", "Thanh toán một phần" }, hoaDon.TrangThai
            );

            return View(hoaDon);
        }


        // GET: HoaDons/Delete/5
        public IActionResult Delete(string id)
        {
            var hd = _context.HoaDons.Find(id);
            if (hd == null) return NotFound();
            return View(hd);
        }

        // POST: HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var hd = _context.HoaDons.Find(id);
            if (hd != null)
            {
                _context.HoaDons.Remove(hd);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: HoaDons/Details/5
        public IActionResult Details(string id)
        {
            var hd = _context.HoaDons
                             .Include(h => h.KhachHang) // 👈 thêm Include
                             .FirstOrDefault(x => x.MaHD == id);
            if (hd == null) return NotFound();
            return View(hd);
        }
    }
}
