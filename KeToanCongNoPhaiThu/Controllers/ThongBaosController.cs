using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.Controllers
{
    [Authorize(Roles = "Kế Toán,Giám đốc")]
    public class ThongBaosController : Controller
    {
        private readonly ReceivableDbContext _context;

        public ThongBaosController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: ThongBaos
        public async Task<IActionResult> Index()
        {
            var thongBaos = await _context.ThongBaos
                .Include(t => t.KhachHang)
                .OrderByDescending(t => t.NgayGui)
                .ToListAsync();

            return View(thongBaos);

        }

        // GET: ThongBaos/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var tb = await _context.ThongBaos
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(m => m.MaTB == id);

            if (tb == null) return NotFound();

            return View(tb);
        }

        // GET: ThongBaos/Create
        public IActionResult Create()
        {
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH");
            return View();
        }

        // POST: ThongBaos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaTB,NoiDung,NgayGui,MaKH")] ThongBao tb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", tb.MaKH);
            return View(tb);
        }

        // GET: ThongBaos/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var tb = await _context.ThongBaos.FindAsync(id);
            if (tb == null) return NotFound();

            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", tb.MaKH);
            return View(tb);
        }

        // POST: ThongBaos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MaTB,NoiDung,NgayGui,MaKH")] ThongBao tb)
        {
            if (id != tb.MaTB) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThongBaoExists(tb.MaTB)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKH"] = new SelectList(_context.KhachHangs, "MaKH", "TenKH", tb.MaKH);
            return View(tb);
        }

        // GET: ThongBaos/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var tb = await _context.ThongBaos
                .Include(t => t.KhachHang)
                .FirstOrDefaultAsync(m => m.MaTB == id);
            if (tb == null) return NotFound();

            return View(tb);
        }

        // POST: ThongBaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tb = await _context.ThongBaos.FindAsync(id);
            if (tb != null)
            {
                _context.ThongBaos.Remove(tb);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ThongBaoExists(string id)
        {
            return _context.ThongBaos.Any(e => e.MaTB == id);
        }
    }
}
