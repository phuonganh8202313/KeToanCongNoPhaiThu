using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KeToanCongNoPhaiThu.Controllers
{
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
            var list = _context.HoaDons.ToList();
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
            var hd = _context.HoaDons.Find(id);
            if (hd == null) return NotFound();
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
            var hd = _context.HoaDons.FirstOrDefault(x => x.MaHD == id);
            if (hd == null) return NotFound();
            return View(hd);
        }
    }
}
