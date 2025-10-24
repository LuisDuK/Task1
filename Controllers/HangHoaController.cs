using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHangHoa.Models;
using System.Text.Json;

namespace QLHangHoa.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly QLThuocContext _context;
        private readonly ILogger<HangHoaController> _logger;
        public HangHoaController(QLThuocContext context, ILogger<HangHoaController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _context.HangHoas
           .Include(h => h.NhomHangHoa)
           .Include(h => h.DuongDung)
           .AsNoTracking()
           .Select(h => new
           {
               Id = h.Id,

               // Row 1
               IdNhom = h.NhomHangHoa.Id,
               MaNhom = h.NhomHangHoa.MaNhom,
               TenNhom = h.NhomHangHoa.TenNhom,
               MaHang = h.MaHang,
               TenHang = h.TenHang,

               // Row 2
               DvtNhapId = h.DvtNhapId,
               DvtXuatId = h.DvtXuatId,
               SoLuongQuyDoi = h.SoLuongQuyDoi,
               MaDuong = h.MaDuongDung,

               // Row 3
               NuocId = h.NuocId,
               HangSxId = h.HangSxId,

               // Row 4
               HamLuong = h.HamLuong,
               HoatChat = h.HoatChatText, 

               // Row 5
               MaAnhXa = h.MaAnhXa,
               MaBarCode = h.MaBarcode,      
               MaPpCheBien = h.MaPpCheBien,
               NhomChiPhiId = h.NhomChiPhiId,
               NguonChiTraId = h.NguonChiTraId,

               // Row 6
               DangBaoChe = h.DangBaoChe,
               QuyCachDongGoi = h.QuyCachDongGoi,

               // Row 7
               SoDangKy = h.SoDangKy,
               ThongTinThau = h.ThongTinThau,
               MaNhaThau = h.MaNhaThau,
               GiaThau = h.GiaThau,

               // Row 8
               TiLeBHYT = h.TiLeBhyt,
               TiLeThanhToan = h.TiLeThanhToan,
               SlMin = h.SlMin,
               SlMax = h.SlMax,
               SoNgayDung = h.SoNgayDung,
               Bhyt = h.Bhyt
           })
           .OrderBy(x => x.TenHang)
           .ToListAsync();

            var nhomHangHoa = await _context.NhomHangHoas
                .AsNoTracking()
                .Select(n => new
                {
                    Id = n.Id,
                    MaNhom = n.MaNhom,
                    TenNhom = n.TenNhom
                })
                .OrderBy(x => x.TenNhom)
                .ToListAsync();
            ViewBag.NhomHangHoaJson = JsonSerializer.Serialize(nhomHangHoa);

            var donViTinh = await _context.DonViTinhs
                .AsNoTracking()
                .Select(d => new
                {
                    Id = d.Id,
                    MaDonVi = d.MaDvt,
                    TenDonVi = d.TenDvt
                })
                .OrderBy(x => x.TenDonVi)
                .ToListAsync();

            // Lấy danh sách đường dùng 
            var duongDung = await _context.DuongDungs
                .AsNoTracking()
                .Select(d => new
                {
                    Id = d.Id,
                    MaDuong = d.MaDuong,
                    TenDuong = d.TenDuong
                })
                .OrderBy(x => x.TenDuong)
                .ToListAsync();

            // Lấy danh sách nước sản xuất
            var nuocSanXuat = await _context.NuocSanXuats
                .AsNoTracking()
                .Select(n => new
                {
                    Id = n.Id,
                    MaNuoc = n.MaNuoc,
                    TenNuoc = n.TenNuoc
                })
                .OrderBy(x => x.TenNuoc)
                .ToListAsync();

            // Lấy danh sách hãng sản xuất 
            var hangSanXuat = await _context.HangSanXuats
                .AsNoTracking()
                .Select(h => new
                {

                    MaHangSx = h.MaHangSx,
                    TenHang = h.TenHang
                })
                .OrderBy(x => x.TenHang)
                .ToListAsync();

            // Lấy danh sách nhóm chi phí 
            var nhomChiPhi = await _context.NhomChiPhis
                .AsNoTracking()
                .Select(n => new
                {
                    Id = n.Id,
                    MaNhom = n.MaNhom,
                    TenNhom = n.TenNhom
                })
                .OrderBy(x => x.TenNhom)
                .ToListAsync();

            // Lấy danh sách nhà thầu
            var nhaThau = await _context.NhaThaus
                .AsNoTracking()
                .Select(n => new
                {
                    Id = n.Id,
                    MaNhaThau = n.MaNhaThau,
                    TenNhaThau = n.TenNhaThau
                })
                .OrderBy(x => x.TenNhaThau)
                .ToListAsync();
            // Lấy danh sách nguồn chi trả 
            var nguonChiTra = await _context.NguonChiTras
                .AsNoTracking()
                .Select(n => new
                {
                    Id = n.Id,
                    MaNguon = n.MaNguon,
                    TenNguon = n.TenNguon
                })
                .OrderBy(x => x.TenNguon)
                .ToListAsync();

            ViewBag.DataJson = JsonSerializer.Serialize(data);
            ViewBag.DonViTinhJson = JsonSerializer.Serialize(donViTinh);
            ViewBag.DuongDungJson = JsonSerializer.Serialize(duongDung);
            ViewBag.NuocSanXuatJson = JsonSerializer.Serialize(nuocSanXuat);
            ViewBag.HangSanXuatJson = JsonSerializer.Serialize(hangSanXuat);
            ViewBag.NhomChiPhiJson = JsonSerializer.Serialize(nhomChiPhi);
            ViewBag.NhaThauJson = JsonSerializer.Serialize(nhaThau);
            ViewBag.NguonChiTraJson = JsonSerializer.Serialize(nguonChiTra);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ThemHangHoa(HangHoa vm)
        {
            try
            {
                // Map sang entity
                var hangHoa = new HangHoa
                {
                    MaNhom = vm.MaNhom,
                    TenHang = vm.TenHang,
                    DvtNhapId = vm.DvtNhapId,
                    DvtXuatId = vm.DvtXuatId,
                    SoLuongQuyDoi = vm.SoLuongQuyDoi,

                    MaDuongDung = vm.MaDuongDung,
                    NuocId = vm.NuocId,
                    HangSxId = vm.HangSxId,
                    HamLuong = vm.HamLuong,
                    HoatChatText = vm.HoatChatText,
                    DangBaoChe = vm.DangBaoChe,
                    QuyCachDongGoi = vm.QuyCachDongGoi,
                    SoDangKy = vm.SoDangKy,

                    MaAnhXa = vm.MaAnhXa,
                    MaBarcode = vm.MaBarcode,
                    MaPpCheBien = vm.MaPpCheBien,

                    NhomChiPhiId = vm.NhomChiPhiId,
                    NguonChiTraId = vm.NguonChiTraId,

                    Bhyt = vm.Bhyt,
                    TiLeBhyt = vm.TiLeBhyt,
                    TiLeThanhToan = vm.TiLeThanhToan,

                    ThongTinThau = vm.ThongTinThau,
                    MaNhaThau = vm.MaNhaThau,
                    GiaThau = vm.GiaThau,
                    SlMin = vm.SlMin,
                    SlMax = vm.SlMax,

                    SoNgayDung = vm.SoNgayDung
                };

                // Sinh mã tự động
                hangHoa.MaHang = await TaoMaHangHoa(hangHoa.MaNhom);

                // Tránh trùng 
                var dup = await _context.HangHoas.AnyAsync(h => h.MaHang == hangHoa.MaHang);
                if (dup)
                {
                    ModelState.AddModelError(string.Empty, "Mã hàng hóa đã tồn tại.");
                    return View("index");
                }

                _context.HangHoas.Add(hangHoa);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Thêm hàng hóa thành công!";
                return RedirectToAction("Index"); 
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại.";
                ModelState.AddModelError("", "Dữ liệu đã bị thay đổi bởi người khác. Vui lòng thử lại.");
                return View("index");
            }
        }
        [HttpPost]
        private async Task<string> TaoMaHangHoa(long nhomHangHoaId)
        {
            // Lấy mã nhóm
            var maNhom = await _context.NhomHangHoas
                .Where(n => n.Id == nhomHangHoaId)
                .Select(n => n.MaNhom)
                .FirstOrDefaultAsync();
           

            if (string.IsNullOrEmpty(maNhom))
            {
                throw new Exception("Không tìm thấy nhóm hàng hóa");
            }

            // Lấy mã hàng hóa lớn nhất trong nhóm này
            var lastMaHang = await _context.HangHoas
                .Where(h => h.MaHang.StartsWith(maNhom))
                .OrderByDescending(h => h.MaHang)
                .Select(h => h.MaHang)
                .FirstOrDefaultAsync();

            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastMaHang))
            {
                var parts = lastMaHang.Split('.');
                if (parts.Length == 2 && int.TryParse(parts[1], out int currentNumber))
                {
                    nextNumber = currentNumber + 1;
                }
            }
            return $"{nhomHangHoaId}.{nextNumber:D4}";
        }
        [HttpPost]
        public IActionResult XoaHangHoa(string MaHang)
        {
            try
            {
                var hangHoa = _context.HangHoas.FirstOrDefault(h => h.MaHang == MaHang);
                if (hangHoa == null)
                {
                    TempData["Error"] = "Hàng hóa không tồn tại.";
                    return RedirectToAction("Index");
                }

                _context.HangHoas.Remove(hangHoa);
                _context.SaveChanges();

                TempData["Success"] = "Xóa hàng hóa thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi xóa: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SuaHangHoa(HangHoa vm)
        {
            try
            {
                var hangHoa = await _context.HangHoas.FirstOrDefaultAsync(h => h.MaHang == vm.MaHang);
                if (hangHoa == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy hàng hóa cần sửa.");
                    return View("index");
                }

                // Cập nhật thông tin
                hangHoa.MaNhom = vm.MaNhom;
                hangHoa.TenHang = vm.TenHang;
                hangHoa.DvtNhapId = vm.DvtNhapId;
                hangHoa.DvtXuatId = vm.DvtXuatId;
                hangHoa.SoLuongQuyDoi = vm.SoLuongQuyDoi;

                hangHoa.MaDuongDung = vm.MaDuongDung;
                hangHoa.NuocId = vm.NuocId;
                hangHoa.HangSxId = vm.HangSxId;
                hangHoa.HamLuong = vm.HamLuong;
                hangHoa.HoatChatText = vm.HoatChatText;
                hangHoa.DangBaoChe = vm.DangBaoChe;
                hangHoa.QuyCachDongGoi = vm.QuyCachDongGoi;
                hangHoa.SoDangKy = vm.SoDangKy;

                hangHoa.MaAnhXa = vm.MaAnhXa;
                hangHoa.MaBarcode = vm.MaBarcode;
                hangHoa.MaPpCheBien = vm.MaPpCheBien;

                hangHoa.NhomChiPhiId = vm.NhomChiPhiId;
                hangHoa.NguonChiTraId = vm.NguonChiTraId;

                hangHoa.Bhyt = vm.Bhyt;
                hangHoa.TiLeBhyt = vm.TiLeBhyt;
                hangHoa.TiLeThanhToan = vm.TiLeThanhToan;

                hangHoa.ThongTinThau = vm.ThongTinThau;
                hangHoa.MaNhaThau = vm.MaNhaThau;
                hangHoa.GiaThau = vm.GiaThau;
                hangHoa.SlMin = vm.SlMin;
                hangHoa.SlMax = vm.SlMax;
                hangHoa.SoNgayDung = vm.SoNgayDung;

                _context.HangHoas.Update(hangHoa);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Cập nhật hàng hóa thành công!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi sửa hàng hóa: " + ex.Message);
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại.";
                return View("index");
            }
        }

    }

}

