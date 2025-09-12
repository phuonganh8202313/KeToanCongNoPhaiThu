using KeToanCongNoPhaiThu.Data;
using KeToanCongNoPhaiThu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KeToanCongNoPhaiThu.Controllers
{
    public class KhachHangsController : Controller
    {
        private readonly ReceivableDbContext _context;

        public KhachHangsController(ReceivableDbContext context)
        {
            _context = context;
        }

        // GET: KhachHangs
        public IActionResult Index()
        {
            var list = _context.KhachHangs.ToList();
            return View(list);
        }

        // GET: KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.KhachHangs.Add(khachHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Edit/5
        public IActionResult Edit(int id)
        {
            var kh = _context.KhachHangs.Find(id);
            if (kh == null) return NotFound();
            return View(kh);
        }

        // POST: KhachHangs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Update(khachHang);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: KhachHangs/Delete/5
        public IActionResult Delete(int id)
        {
            var kh = _context.KhachHangs.Find(id);
            if (kh == null) return NotFound();
            return View(kh);
        }

        // POST: KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var kh = _context.KhachHangs.Find(id);
            if (kh != null)
            {
                _context.KhachHangs.Remove(kh);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: KhachHangs/Details/5
        public IActionResult Details(int id)
        {
            var kh = _context.KhachHangs.FirstOrDefault(x => x.MaKH == id);
            if (kh == null) return NotFound();
            return View(kh);
        }
    }
}