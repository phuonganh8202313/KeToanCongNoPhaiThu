using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Kế Toán,Giám đốc")]
    public class CongNosController : Controller
    {
        private readonly ReceivableDbContext _context;

        public CongNosController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: CongNos
        public async Task<IActionResult> Index()
        {
            var congNos = await _context.CongNos.Include(c => c.KhachHang).ToListAsync();

            foreach (var cn in congNos)
            {
                if (cn.TrangThai != "Đã thanh toán" && cn.HanThanhToan < DateTime.Now)
                {
                    // Kiểm tra xem đã tạo thông báo chưa (tránh trùng lặp)
                    bool exists = _context.ThongBaos.Any(tb =>
                        tb.MaKH == cn.MaKH &&
                        tb.NoiDung.Contains(cn.MaCongNo) &&
                        tb.LoaiThongBao == "Quá hạn");

                    if (!exists)
                    {
                        var tb = new ThongBao
                        {
                            MaTB = Guid.NewGuid().ToString(),
                            NoiDung = $"Công nợ {cn.MaCongNo} của khách hàng {cn.KhachHang?.TenKH} đã quá hạn thanh toán!",
                            NgayGui = DateTime.Now,
                            LoaiThongBao = "Quá hạn",
                            DaDoc = false,
                            MaKH = cn.MaKH
                        };

                        _context.ThongBaos.Add(tb);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return View(congNos);
        }

        // GET: CongNos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var congNo = await _context.CongNos
                .Include(c => c.KhachHang)
                .FirstOrDefaultAsync(m => m.MaCongNo == id);

            if (congNo == null) return NotFound();

            return View(congNo);
        }

        // GET: CongNos/Create
        public IActionResult Create()
        {
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH");
            ViewData["TrangThaiList"] = new SelectList(new List<string>
            {
                "Còn nợ",
                "Đã thanh toán",
                "Quá hạn"
            });
            return View();
        }


        // POST: CongNos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCongNo,SoTienNo,HanThanhToan,TrangThai,MaKH")] CongNo congNo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(congNo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", congNo.MaKH);
            ViewData["TrangThaiList"] = new SelectList(new List<string>
            {
                "Còn nợ",
                "Đã thanh toán",
                "Quá hạn"
            }, congNo?.TrangThai);

            return View(congNo);
        }

        // GET: CongNos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var congNo = await _context.CongNos.FindAsync(id);
            if (congNo == null) return NotFound();

            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", congNo.MaKH);
            ViewData["TrangThaiList"] = new SelectList(new List<string>
            {
                "Còn nợ",
                "Đã thanh toán",
                "Quá hạn"
            }, congNo?.TrangThai);

            return View(congNo);
        }

        // POST: CongNos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaCongNo,SoTienNo,HanThanhToan,TrangThai,MaKH")] CongNo congNo)
        {
            if (id != congNo.MaCongNo) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(congNo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CongNoExists(congNo.MaCongNo)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", congNo.MaKH);
            ViewData["TrangThaiList"] = new SelectList(new List<string>
            {
                "Còn nợ",
                "Đã thanh toán",
                "Quá hạn"
            }, congNo?.TrangThai);

            return View(congNo);
        }


        // GET: CongNos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var congNo = await _context.CongNos
                .Include(c => c.KhachHang)
                .FirstOrDefaultAsync(m => m.MaCongNo == id);
            if (congNo == null) return NotFound();

            return View(congNo);
        }

        // POST: CongNos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var congNo = await _context.CongNos.FindAsync(id);
            if (congNo != null)
            {
                _context.CongNos.Remove(congNo);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CongNoExists(string id)
        {
            return _context.CongNos.Any(e => e.MaCongNo == id);
        }
    }
}
