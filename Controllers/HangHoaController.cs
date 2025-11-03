using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLHangHoa.Models;
using System.Text.Json;
using ClosedXML.Excel;
using System.Drawing;


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
            .Include(h => h.DvtNhap)
            .Include(h => h.DvtXuat)
            .Include(h => h.DuongDung)
            .Include(h => h.Nuoc)
            .Include(h => h.HangSx)
            .Include(h => h.NhaThau)
            .Include(h => h.NhomChiPhi)
            .Include(h => h.NguonChiTra)
           .AsNoTracking()
           .Where(h => h.TrangThai == 1)
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

               DonViTinhNhap = h.DvtNhap != null ? h.DvtNhap.TenDvt : "",
               DonViTinhXuat = h.DvtXuat != null ? h.DvtXuat.TenDvt : "",
               DuongDung = h.DuongDung != null ? h.DuongDung.TenDuong : "",
               // Row 3
               NuocId = h.NuocId,
               HangSxId = h.HangSxId,
               NuocSanXuat = h.Nuoc != null ? h.Nuoc.TenNuoc : "",
               HangSanXuat = h.HangSx != null ? h.HangSx.TenHang : "",

               // Row 4
               HamLuong = h.HamLuong,
               HoatChat = h.HoatChatText,

               // Row 5
               MaAnhXa = h.MaAnhXa,
               MaBarCode = h.MaBarcode,
               MaPpCheBien = h.MaPpCheBien,
               NhomChiPhiId = h.NhomChiPhiId,
               NguonChiTraId = h.NguonChiTraId,
               MaNhomChiPhi = h.NhomChiPhi != null ? h.NhomChiPhi.TenNhom : "",
               NguonChiTra = h.NguonChiTra != null ? h.NguonChiTra.TenNguon : "",
               // Row 6
               DangBaoChe = h.DangBaoChe,
               QuyCachDongGoi = h.QuyCachDongGoi,

               // Row 7
               SoDangKy = h.SoDangKy,
               ThongTinThau = h.ThongTinThau,
               IdNhaThau = h.NhaThau != null ? (int?)h.NhaThau.Id : null,
               NhaThau = h.NhaThau != null ? h.NhaThau.TenNhaThau : "",
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
        [HttpGet]
        public async Task<IActionResult> KhoiPhuc()
        {
            var data = await _context.HangHoas
           .Include(h => h.NhomHangHoa)
           .Include(h => h.DuongDung)
           .Include(h => h.NhaThau)
           .AsNoTracking()
           .Where(h => h.TrangThai == 0)
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
            return View("Index");
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
            return $"{maNhom}.{nextNumber:D4}";
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

                hangHoa.TrangThai = 0;
                _context.HangHoas.Update(hangHoa);
                _context.SaveChanges();

                TempData["Success"] = "Xóa thành công.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật: " + ex.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public IActionResult KhoiPhucHangHoa(string MaHang)
        {
            try
            {
                var hangHoa = _context.HangHoas.FirstOrDefault(h => h.MaHang == MaHang);
                if (hangHoa == null)
                {
                    TempData["Error"] = "Hàng hóa không tồn tại.";
                    return RedirectToAction("Index");
                }

                hangHoa.TrangThai = 1;
                _context.HangHoas.Update(hangHoa);
                _context.SaveChanges();

                TempData["Success"] = "Khôi phục thành công.";
                return RedirectToAction("KhoiPhuc");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Đã xảy ra lỗi khi cập nhật: " + ex.Message;
                return RedirectToAction("KhoiPhuc");
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
                TempData["Error"] = "Có lỗi xảy ra. Vui lòng thử lại." + ex.Message;
                return View("Index");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromBody] ExportRequest request)
        {
            try
            {
                // Lấy dữ liệu cần export
                var query = _context.HangHoas
                   .Include(h => h.NhomHangHoa)
                   .Include(h => h.DvtNhap)
                   .Include(h => h.DvtXuat)
                   .Include(h => h.DuongDung)
                   .Include(h => h.Nuoc)
                   .Include(h => h.HangSx)
                   .Include(h => h.NhaThau)
                   .Include(h => h.NhomChiPhi)
                   .Include(h => h.NguonChiTra)
                   .AsNoTracking()
                   .Where(h => h.TrangThai == 1);

                // Lọc theo điều kiện
                if (!string.IsNullOrEmpty(request.MaNhom))
                {
                    query = query.Where(h => h.NhomHangHoa.MaNhom == request.MaNhom);
                }

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var term = request.SearchTerm.ToLower();
                    query = query.Where(h =>
                        h.TenHang.ToLower().Contains(term) ||
                        h.MaHang.ToLower().Contains(term));
                }

                var data = await query
                           .Select(h => new
                           {
                               NhomHangHoa = h.NhomHangHoa.TenNhom,
                               TenHang = h.TenHang,
                               MaHang = h.MaHang,
                               DonViTinhNhap = h.DvtNhap != null ? h.DvtNhap.TenDvt : "",
                               DonViTinhXuat = h.DvtXuat != null ? h.DvtXuat.TenDvt : "",
                               SoLuongQuyDoi = h.SoLuongQuyDoi,
                               MaDuong = h.MaDuongDung,
                               DuongDung = h.DuongDung != null ? h.DuongDung.TenDuong : "",
                               NuocSanXuat = h.Nuoc != null ? h.Nuoc.TenNuoc : "",
                               HangSanXuat = h.HangSx != null ? h.HangSx.TenHang : "",
                               HamLuong = h.HamLuong,
                               HoatChat = h.HoatChatText,
                               MaAnhXa = h.MaAnhXa,
                               MaPpCheBien = h.MaPpCheBien,
                               SoLuongMin = h.SlMin,
                               SoLuongMax = h.SlMax,
                               SoNgayDung = h.SoNgayDung,
                               NhomChiPhi = h.NhomChiPhi != null ? h.NhomChiPhi.TenNhom : "",
                               NguonChiTra = h.NguonChiTra != null ? h.NguonChiTra.TenNguon : "",
                               DangBaoChe = h.DangBaoChe,
                               Bhyt = h.Bhyt ? "Có" : "Không",
                               MaBarcode = h.MaBarcode,
                               SoDangKy = h.SoDangKy,
                               QuyCachDongGoi = h.QuyCachDongGoi,
                               ThongTinThau = h.ThongTinThau,
                               NhaThau = h.NhaThau != null ? h.NhaThau.TenNhaThau : "",
                               GiaThau = h.GiaThau,
                               TiLeBHYT = h.TiLeBhyt,
                               TiLeThanhToan = h.TiLeThanhToan
                           })
                           .OrderBy(x => x.TenHang)
                           .ToListAsync();

                // Tạo Excel file
                using (var workbook = new XLWorkbook())
                {
                    var ws = workbook.Worksheets.Add("Danh sách hàng hóa");

                    // === Header ===
                    string[] headers = new[]
                    {
                "Nhóm hàng hóa", "Tên hàng hóa", "Mã hàng hóa",
                "Đơn vị tính nhập", "Đơn vị tính xuất", "Số lượng quy đổi",
                "Mã đường dùng", "Đường dùng", "Nước sản xuất", "Hãng sản xuất",
                "Hàm lượng", "Hoạt chất", "Mã ánh xạ", "Mã PP chế biến",
                "SL min", "SL max", "Số ngày dùng", "Nhóm chi phí",
                "Nguồn chi trả", "Dạng bào chế", "BHYT", "Mã barcode",
                "Số đăng ký", "Quy cách đóng gói", "Thông tin thầu",
                "Nhà thầu", "Giá thầu", "Tỷ lệ BHYT", "Tỷ lệ thanh toán"
            };

                    for (int i = 0; i < headers.Length; i++)
                        ws.Cell(1, i + 1).Value = headers[i];

                    var headerRange = ws.Range(1, 1, 1, headers.Length);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

                    // === Data ===
                    int row = 2;
                    foreach (var item in data)
                    {
                        ws.Cell(row, 1).Value = item.NhomHangHoa;
                        ws.Cell(row, 2).Value = item.TenHang;
                        ws.Cell(row, 3).Value = item.MaHang;
                        ws.Cell(row, 4).Value = item.DonViTinhNhap;
                        ws.Cell(row, 5).Value = item.DonViTinhXuat;
                        ws.Cell(row, 6).Value = item.SoLuongQuyDoi;
                        ws.Cell(row, 7).Value = item.MaDuong;
                        ws.Cell(row, 8).Value = item.DuongDung;
                        ws.Cell(row, 9).Value = item.NuocSanXuat;
                        ws.Cell(row, 10).Value = item.HangSanXuat;
                        ws.Cell(row, 11).Value = item.HamLuong;
                        ws.Cell(row, 12).Value = item.HoatChat;
                        ws.Cell(row, 13).Value = item.MaAnhXa;
                        ws.Cell(row, 14).Value = item.MaPpCheBien;
                        ws.Cell(row, 15).Value = item.SoLuongMin;
                        ws.Cell(row, 16).Value = item.SoLuongMax;
                        ws.Cell(row, 17).Value = item.SoNgayDung;
                        ws.Cell(row, 18).Value = item.NhomChiPhi;
                        ws.Cell(row, 19).Value = item.NguonChiTra;
                        ws.Cell(row, 20).Value = item.DangBaoChe;
                        ws.Cell(row, 21).Value = item.Bhyt;
                        ws.Cell(row, 22).Value = item.MaBarcode;
                        ws.Cell(row, 23).Value = item.SoDangKy;
                        ws.Cell(row, 24).Value = item.QuyCachDongGoi;
                        ws.Cell(row, 25).Value = item.ThongTinThau;
                        ws.Cell(row, 26).Value = item.NhaThau;
                        ws.Cell(row, 27).Value = item.GiaThau;
                        ws.Cell(row, 28).Value = item.TiLeBHYT;
                        ws.Cell(row, 29).Value = item.TiLeThanhToan;
                        row++;
                    }

                    ws.Columns().AdjustToContents();

                    // Trả về file
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        var fileName = $"DanhSachHangHoa_{DateTime.Now:yyyyMMdd}.xlsx";

                        return File(content,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        public class ExportRequest
        {
            public string? MaNhom { get; set; }
            public string? SearchTerm { get; set; }
        }
        [HttpPost]
        public async Task<IActionResult> ImportExcel([FromBody] List<HangHoaDto> data)
        {

            var successCount = 0;
            var errors = new List<string>();

            try
            {
                foreach (var row in data)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(row.NhomHangHoa) ||
                            string.IsNullOrWhiteSpace(row.TenHang) ||
                            string.IsNullOrWhiteSpace(row.DonViTinhNhap) ||
                            string.IsNullOrWhiteSpace(row.DonViTinhXuat) ||
                            string.IsNullOrWhiteSpace(row.MaAnhXa) ||
                            string.IsNullOrWhiteSpace(row.NhomChiPhi) ||
                            string.IsNullOrWhiteSpace(row.NguonChiTra))
                        {
                            errors.Add($"Thiếu thông tin bắt buộc tại dòng {data.IndexOf(row) + 1}");
                            continue;
                        }
                        int line = data.IndexOf(row) + 1;

                        // Validate cơ bản
                        if (string.IsNullOrEmpty(row.TenHang))
                        {
                            errors.Add($"Dòng {line}: Thiếu tên hàng hóa");
                            continue;
                        }

                        // === Tìm các Id tham chiếu ===
                        var nhom = await _context.NhomHangHoas
                            .FirstOrDefaultAsync(x => x.TenNhom == row.NhomHangHoa);

                        if (nhom == null)
                        {
                            errors.Add($"Dòng {line}: Không tìm thấy nhóm hàng hóa '{row.NhomHangHoa}'");
                            continue;
                        }

                        var duongDung = await _context.DuongDungs
                            .FirstOrDefaultAsync(x => x.MaDuong == row.DuongDung);

                        var dvtNhap = await _context.DonViTinhs
                            .FirstOrDefaultAsync(x => x.TenDvt == row.DonViTinhNhap);

                        var dvtXuat = await _context.DonViTinhs
                            .FirstOrDefaultAsync(x => x.TenDvt == row.DonViTinhXuat);

                        var nuoc = await _context.NuocSanXuats
                            .FirstOrDefaultAsync(x => x.TenNuoc == row.NuocSanXuat);

                        var hangSx = await _context.HangSanXuats
                            .FirstOrDefaultAsync(x => x.TenHang == row.HangSanXuat);

                        var nhaThau = await _context.NhaThaus
                            .FirstOrDefaultAsync(x => x.TenNhaThau == row.NhaThau);

                        var nhomChiPhi = await _context.NhomChiPhis
                            .FirstOrDefaultAsync(x => x.TenNhom == row.NhomChiPhi);

                        var nguonChiTra = await _context.NguonChiTras
                            .FirstOrDefaultAsync(x => x.TenNguon == row.NguonChiTra);

                        if (nhom == null || dvtNhap == null || dvtXuat == null || nhomChiPhi == null || nguonChiTra == null)
                        {
                            errors.Add($"Không tìm thấy khóa ngoại tại dòng {data.IndexOf(row) + 1}");
                            continue;
                        }

                        // === Tạo đối tượng mới ===
                        var hangHoa = new HangHoa
                        {
                            TenHang = row.TenHang,
                            MaNhom = nhom.Id,
                            DvtNhapId = dvtNhap.Id,
                            DvtXuatId = dvtXuat.Id,
                            SoLuongQuyDoi = row.SoLuongQuyDoi,
                            MaDuongDung = row.DuongDung,
                            NuocId = nuoc?.Id,
                            HangSxId = hangSx?.Id,
                            HamLuong = row.HamLuong,
                            HoatChatText = row.HoatChat,
                            MaAnhXa = row.MaAnhXa,
                            MaPpCheBien = row.MaPPCheBien,
                            SlMin = row.SoLuongMin,
                            SlMax = row.SoLuongMax,
                            SoNgayDung = row.SoNgayDung?? 0,
                            NhomChiPhiId = nhomChiPhi.Id,
                            NguonChiTraId = nguonChiTra.Id,
                            DangBaoChe = row.DangBaoChe,
                            Bhyt = (row.Bhyt ?? "").Trim().ToLower() == "có",
                            MaBarcode = row.MaBarcode,
                            SoDangKy = row.SoDangKy,
                            QuyCachDongGoi = row.QuyCachDongGoi,
                            ThongTinThau = row.ThongTinThau,
                            MaNhaThau = nhaThau?.Id,
                            GiaThau = row.GiaThau,
                            TiLeBhyt = row.TiLeBHYT,
                            TiLeThanhToan =row.TiLeThanhToan,
                            TrangThai = 1
                        };
                        hangHoa.MaHang = await TaoMaHangHoa(hangHoa.MaNhom);
                        _context.HangHoas.Add(hangHoa);
                        await _context.SaveChangesAsync();
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        var inner = ex.InnerException?.Message ?? "";
                        errors.Add($"Dòng {data.IndexOf(row) + 1}: {ex.Message} {inner}");
                    }
                }

                return Json(new
                {
                    success = true,
                    successCount,
                    totalCount = data.Count,
                    errors
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        private double? ParseNullableDouble(string input)
        {
            if (double.TryParse(input, out var val))
                return val;
            return null;
        }

        private int? ParseNullableInt(string input)
        {
            if (int.TryParse(input, out var val))
                return val;
            return null;
        }

        private decimal? ParseNullableDecimal(string input)
        {
            if (decimal.TryParse(input, out var val))
                return val;
            return null;
        }

    }

}
