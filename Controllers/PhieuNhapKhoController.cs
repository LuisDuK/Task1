using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLHangHoa.Models;

namespace QLHangHoa.Controllers
{
    public class PhieuNhapKhoController : Controller
    {
        private readonly QLThuocContext _context;

        public PhieuNhapKhoController(QLThuocContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy danh sách nhà cung cấp
            var nhaCungCap = await _context.NhaCungCaps
                .AsNoTracking()
                .Select(n => new {
                    NhaCungCapId = n.NhaCungCapId,
                    TenNhaCungCap = n.TenNhaCungCap,
            
                })
                .OrderBy(x => x.TenNhaCungCap)
                .ToListAsync();

            // Lấy danh sách kho
            /* var kho = await _context.Khos
                 .AsNoTracking()
                 .Select(k => new {
                     Id = k.Id,
                     MaKho = k.MaKho,
                     TenKho = k.TenKho
                 })
                 .OrderBy(x => x.TenKho)
                 .ToListAsync();*/
            var hangHoa = await _context.HangHoas
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
            // Lấy danh sách phiếu nhập
            var phieuNhap = await _context.PhieuNhaps
       .Include(p => p.NhaCungCap)
       .AsNoTracking()
       .OrderByDescending(p => p.PhieuNhapId)
       .Select(p => new
       {
           p.PhieuNhapId,
           p.MaPhieuNhap,
           p.NgayNhap,
           p.NgayHoaDon,
           p.SoHoaDon,
           p.TongTienHang,
           p.TongChietKhau,
           p.TongThue,
           p.TongCong,
           p.NguoiNhap,
           TenNhaCungCap = p.NhaCungCap != null ? p.NhaCungCap.TenNhaCungCap : ""
       })
       .ToListAsync();

            // Serialize dữ liệu
            var options = new JsonSerializerOptions
            {
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false
            };

            ViewBag.NhaCungCapJson = JsonSerializer.Serialize(nhaCungCap, options);
            ViewBag.PhieuNhapJson = JsonSerializer.Serialize(phieuNhap, options);
            ViewBag.HangHoaJson = JsonSerializer.Serialize(hangHoa, options);
            // ViewBag.KhoJson = JsonSerializer.Serialize(kho, options);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SavePhieuNhap([FromBody] PhieuNhapViewModel model)
        {
            if (model == null || model.ChiTietPhieuNhaps == null || model.ChiTietPhieuNhaps.Count == 0)
                return BadRequest("Dữ liệu phiếu nhập không hợp lệ.");

            using var tran = await _context.Database.BeginTransactionAsync();

            try
            {
                // TẠO MÃ TỰ ĐỘNG
                string maPhieu = $"PN-{DateTime.Now:yyMMddHHmmss}";

                // TÍNH TỔNG 
                decimal tongHang = 0, tongCk = 0, tongVat = 0, tongCong = 0;

                foreach (var c in model.ChiTietPhieuNhaps)
                {
                    var tienHang = c.SoLuong * c.DonGiaNhap;
                    var tienSauCk = tienHang * (1 - (c.ChietKhau ?? 0) / 100);

                    // Nếu VAT là % thì tính theo phần trăm, nếu người dùng nhập tiền thì lấy luôn
                    decimal tienVat = 0;
                    if (c.Vat.HasValue)
                    {
                        if (c.Vat <= 100)
                            tienVat = tienSauCk * (c.Vat.Value / 100); // VAT là %
                        else
                            tienVat = c.Vat.Value; // VAT là số tiền
                    }

                    var thanhTien = tienSauCk + tienVat;

                    tongHang += tienHang;
                    tongCk += (tienHang - tienSauCk);
                    tongVat += tienVat;
                    tongCong += thanhTien;
                }

                //TẠO PHIẾU NHẬP 
                var phieu = new PhieuNhap
                {
                    MaPhieuNhap = maPhieu,
                    NgayNhap = DateTime.Now,
                    NgayHoaDon = model.NgayHoaDon,
                    SoHoaDon = model.SoHoaDon,
                    KyHieuHoaDon = model.KyHieuHoaDon,
                    NhaCungCapId = model.NhaCungCapId,
                    NguoiNhap = model.NguoiNhap,
                    GhiChu = model.GhiChu,
                    TongTienHang = tongHang,
                    TongChietKhau = tongCk,
                    TongThue = tongVat,
                    TongCong = tongCong,
                    TrangThai = 1
                };

                _context.PhieuNhaps.Add(phieu);
                await _context.SaveChangesAsync();

                // GÁN ID & LƯU CHI TIẾT 
                foreach (var c in model.ChiTietPhieuNhaps)
                {
                    c.PhieuNhapId = phieu.PhieuNhapId;
                    _context.ChiTietPhieuNhaps.Add(c);
                }

                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                return Ok(new
                {
                    success = true,
                    message = "Lưu phiếu nhập thành công!",
                    phieuId = phieu.PhieuNhapId,
                    maPhieu = maPhieu
                });
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return BadRequest(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}
