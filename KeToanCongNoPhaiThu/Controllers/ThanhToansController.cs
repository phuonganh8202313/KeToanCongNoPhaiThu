using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Kế Toán,Giám đốc")]
    public class ThanhToansController : Controller
    {
        private readonly ReceivableDbContext _context;

        public ThanhToansController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: ThanhToans
        public async Task<IActionResult> Index()
        {
            var list = _context.ThanhToans.Include(t => t.HoaDon);
            return View(await list.ToListAsync());
        }

        // GET: ThanhToans/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToans
                .Include(t => t.HoaDon)
                .FirstOrDefaultAsync(m => m.MaTT == id);

            if (thanhToan == null) return NotFound();

            return View(thanhToan);
        }

        // GET: ThanhToans/Create
        public IActionResult Create()
        {
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD");
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "MaNV", "TenNV");
            return View();
        }

        // POST: ThanhToans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTT,NgayThanhToan,NoiDung,SoTienThanhToan,NguoiGhiNhan,MaHD")] ThanhToan thanhToan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(thanhToan);

                var congNo = _context.CongNos.FirstOrDefault(c => c.MaKH == thanhToan.HoaDon.MaKH);
                if (congNo != null)
                {
                    congNo.SoTienNo -= thanhToan.SoTienThanhToan;
                    if (congNo.SoTienNo <= 0)
                    {
                        congNo.TrangThai = "Đã thanh toán";
                    }
                }

                // Cập nhật trạng thái hóa đơn
                var hoaDon = _context.HoaDons.Find(thanhToan.MaHD);
                if (hoaDon != null)
                {
                    if (congNo != null && congNo.SoTienNo <= 0)
                        hoaDon.TrangThai = "Đã thanh toán";
                    else
                        hoaDon.TrangThai = "Thanh toán một phần";
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", thanhToan.MaHD);
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "MaNV", "TenNV", thanhToan.NguoiGhiNhanId);
            return View(thanhToan);
        }

        // GET: ThanhToans/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan == null) return NotFound();

            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", thanhToan.MaHD);
            return View(thanhToan);
        }

        // POST: ThanhToans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaTT,NgayThanhToan,NoiDung,SoTienThanhToan,NguoiGhiNhan,MaHD")] ThanhToan thanhToan)
        {
            if (id != thanhToan.MaTT) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(thanhToan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThanhToanExists(thanhToan.MaTT)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", thanhToan.MaHD);
            return View(thanhToan);
        }

        // GET: ThanhToans/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var thanhToan = await _context.ThanhToans
                .Include(t => t.HoaDon)
                .FirstOrDefaultAsync(m => m.MaTT == id);
            if (thanhToan == null) return NotFound();

            return View(thanhToan);
        }

        // POST: ThanhToans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ThanhToanExists(string id)
        {
            return _context.ThanhToans.Any(e => e.MaTT == id);
        }
    }
}
