using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Kế Toán,Giám đốc")]
    public class ChiTietHoaDonsController : Controller
    {
        private readonly ReceivableDbContext _context;

        public ChiTietHoaDonsController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: ChiTietHoaDons
        public async Task<IActionResult> Index()
        {
            var chiTiet = _context.ChiTietHoaDons.Include(c => c.HoaDon);
            return View(await chiTiet.ToListAsync());
        }

        // GET: ChiTietHoaDons/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietHoaDons
                .Include(c => c.HoaDon)
                .FirstOrDefaultAsync(m => m.MaCTHD == id);

            if (chiTiet == null) return NotFound();

            return View(chiTiet);
        }

        // GET: ChiTietHoaDons/Create
        public IActionResult Create()
        {
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD");
            return View();
        }

        // POST: ChiTietHoaDons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaCTHD,MaHD,NoiDung,SoTien")] ChiTietHoaDon chiTiet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chiTiet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", chiTiet.MaHD);
            return View(chiTiet);
        }

        // GET: ChiTietHoaDons/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietHoaDons.FindAsync(id);
            if (chiTiet == null) return NotFound();

            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", chiTiet.MaHD);
            return View(chiTiet);
        }

        // POST: ChiTietHoaDons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaCTHD,MaHD,NoiDung,SoTien")] ChiTietHoaDon chiTiet)
        {
            if (id != chiTiet.MaCTHD) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chiTiet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChiTietExists(chiTiet.MaCTHD)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaHD"] = new SelectList(_context.HoaDons, "MaHD", "MaHD", chiTiet.MaHD);
            return View(chiTiet);
        }

        // GET: ChiTietHoaDons/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var chiTiet = await _context.ChiTietHoaDons
                .Include(c => c.HoaDon)
                .FirstOrDefaultAsync(m => m.MaCTHD == id);

            if (chiTiet == null) return NotFound();

            return View(chiTiet);
        }

        // POST: ChiTietHoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var chiTiet = await _context.ChiTietHoaDons.FindAsync(id);
            if (chiTiet != null)
            {
                _context.ChiTietHoaDons.Remove(chiTiet);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ChiTietExists(string id)
        {
            return _context.ChiTietHoaDons.Any(e => e.MaCTHD == id);
        }
    }
}
