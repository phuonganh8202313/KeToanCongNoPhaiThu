
using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KeToanCongNoPhaiThu.Controllers
{
	public class HoaDonsController : Controller
	{
		private readonly KeToanCongNoPhaiThuContext _context;

		public HoaDonsController(KeToanCongNoPhaiThuContext context)
		{
			_context = context;
		}

		// GET: HoaDons
		public async Task<IActionResult> Index()
		{
			var hoaDons = _context.HoaDons.Include(h => h.KhachHang);
			return View(await hoaDons.ToListAsync());
		}

		// GET: HoaDons/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null) return NotFound();
			var hoaDon = await _context.HoaDons
				.Include(h => h.KhachHang)
				.FirstOrDefaultAsync(m => m.MaHD == id);
			if (hoaDon == null) return NotFound();
			return View(hoaDon);
		}

		// GET: HoaDons/Create
		public IActionResult Create()
		{
			ViewBag.MaKH = new SelectList(_context.KhachHangs, "MaKH", "TenKH");
			return View();
		}

		// POST: HoaDons/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("NgayLap,TongTien,TrangThai,MaKH")] HoaDon hoaDon)
		{
			if (ModelState.IsValid)
			{
				_context.Add(hoaDon);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewBag.MaKH = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
			return View(hoaDon);
		}

		// GET: HoaDons/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null) return NotFound();
			var hoaDon = await _context.HoaDons.FindAsync(id);
			if (hoaDon == null) return NotFound();
			ViewBag.MaKH = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
			return View(hoaDon);
		}

		// POST: HoaDons/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("MaHD,NgayLap,TongTien,TrangThai,MaKH")] HoaDon hoaDon)
		{
			if (id != hoaDon.MaHD) return NotFound();
			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(hoaDon);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!HoaDonExists(hoaDon.MaHD)) return NotFound();
					else throw;
				}
				return RedirectToAction(nameof(Index));
			}
			ViewBag.MaKH = new SelectList(_context.KhachHangs, "MaKH", "TenKH", hoaDon.MaKH);
			return View(hoaDon);
		}

		// GET: HoaDons/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) return NotFound();
			var hoaDon = await _context.HoaDons
				.Include(h => h.KhachHang)
				.FirstOrDefaultAsync(m => m.MaHD == id);
			if (hoaDon == null) return NotFound();
			return View(hoaDon);
		}

		// POST: HoaDons/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var hoaDon = await _context.HoaDons.FindAsync(id);
			if (hoaDon != null)
			{
				_context.HoaDons.Remove(hoaDon);
				await _context.SaveChangesAsync();
			}
			return RedirectToAction(nameof(Index));
		}

		private bool HoaDonExists(int id)
		{
			return _context.HoaDons.Any(e => e.MaHD == id);
		}
	}
}

