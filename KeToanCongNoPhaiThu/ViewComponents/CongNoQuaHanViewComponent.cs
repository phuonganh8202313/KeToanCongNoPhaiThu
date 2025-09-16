using KeToanCongNoPhaiThu.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KeToanCongNoPhaiThu.ViewComponents
{
    public class CongNoQuaHanViewComponent : ViewComponent
    {
        private readonly ReceivableDbContext _context;

        public CongNoQuaHanViewComponent(ReceivableDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var congNoQuaHan = await _context.CongNos
                .Where(c => c.HanThanhToan < DateTime.Now && c.TrangThai != "Đã thanh toán")
                .Include(c => c.KhachHang)
                .ToListAsync();

            return View(congNoQuaHan);
        }
    }
}
