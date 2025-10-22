using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using QLHangHoa.Models;
using System.Text.Json;

namespace QLHangHoa.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly QLThuocContext _context;
        public HangHoaController(QLThuocContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _context.HangHoas
           .Include(h => h.NhomHangHoa) 
           .Include(h => h.DuongDung)        
           .AsNoTracking()
           .Select(h => new {
               MaNhom = h.NhomHangHoa.MaNhom,      
               TenHang = h.TenHang,
               MaHang = h.MaHang,
               DuongDungId = h.DuongDungId,
               MaDuong = h.MaDuongDung ?? h.DuongDung!.MaDuong, 
               HamLuong = h.HamLuong,
               HoatChat = h.HoatChatText,
               MaAnhXa = h.MaAnhXa,
               Bhyt = h.Bhyt,
               ThongTinThau = h.ThongTinThau 
           })
           .OrderBy(x => x.TenHang)
           .ToListAsync();
            var nhomHangHoa = await _context.NhomHangHoas
                .AsNoTracking()
                .Select(n => new {
                    MaNhom = n.MaNhom,
                    TenNhom = n.TenNhom
                })
                .OrderBy(x => x.TenNhom)
                .ToListAsync();
            ViewBag.NhomHangHoaJson = JsonSerializer.Serialize(nhomHangHoa);

            var donViTinh = await _context.DonViTinhs
                .AsNoTracking()
                .Select(d => new {
                    Id = d.Id,
                    MaDonVi = d.MaDvt,
                    TenDonVi = d.TenDvt
                })
                .OrderBy(x => x.TenDonVi)
                .ToListAsync();

            // Lấy danh sách đường dùng (THÊM MỚI)
            var duongDung = await _context.DuongDungs
                .AsNoTracking()
                .Select(d => new {
                    Id = d.Id,
                    MaDuong = d.MaDuong,
                    TenDuong = d.TenDuong
                })
                .OrderBy(x => x.TenDuong)
                .ToListAsync();

            // Lấy danh sách nước sản xuất (THÊM MỚI)
            var nuocSanXuat = await _context.NuocSanXuats
                .AsNoTracking()
                .Select(n => new {
                    Id = n.Id,
                    MaNuoc = n.MaNuoc,
                    TenNuoc = n.TenNuoc
                })
                .OrderBy(x => x.TenNuoc)
                .ToListAsync();

            // Lấy danh sách hãng sản xuất (THÊM MỚI)
            var hangSanXuat = await _context.HangSanXuats
                .AsNoTracking()
                .Select(h => new {
              
                    MaHang = h.MaHangSx,
                    TenHang = h.TenHang
                })
                .OrderBy(x => x.TenHang)
                .ToListAsync();

            // Lấy danh sách nhóm chi phí (THÊM MỚI)
            var nhomChiPhi = await _context.NhomChiPhis
                .AsNoTracking()
                .Select(n => new {
                    Id = n.Id,
                    MaNhom = n.MaNhom,
                    TenNhom = n.TenNhom
                })
                .OrderBy(x => x.TenNhom)
                .ToListAsync();

            // Lấy danh sách nhà thầu
            var nhaThau = await _context.NhaThaus
                .AsNoTracking()
                .Select(n => new {
                    Id = n.Id,
                    MaNhaThau = n.MaNhaThau,
                    TenNhaThau = n.TenNhaThau
                })
                .OrderBy(x => x.TenNhaThau)
                .ToListAsync();


            ViewBag.DataJson = JsonSerializer.Serialize(data);
            ViewBag.DonViTinhJson = JsonSerializer.Serialize(donViTinh);
            ViewBag.DuongDungJson = JsonSerializer.Serialize(duongDung);
            ViewBag.NuocSanXuatJson = JsonSerializer.Serialize(nuocSanXuat);
            ViewBag.HangSanXuatJson = JsonSerializer.Serialize(hangSanXuat);
            ViewBag.NhomChiPhiJson = JsonSerializer.Serialize(nhomChiPhi);
            ViewBag.NhaThauJson = JsonSerializer.Serialize(nhaThau);
            return View();
        }
        public IActionResult XoaHangHoa(string maHangHoa)
        {
            var chiTietHangHoa= _context.HangHoas.Where(h => h.MaHang == maHangHoa).FirstOrDefault();
            if (chiTietHangHoa != null)
            {
                _context.HangHoas.Remove(chiTietHangHoa);
                _context.SaveChanges();
                return Json(new { success = true, message = "Xóa hàng hóa thành công." });
            }
            return Json(new { success = false, message = "Hàng hóa không tồn tại." });
        }

    }
}
