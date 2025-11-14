using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLHangHoa.Models;
using ExcelGenerator;
using ClosedXML.Excel;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
       .Where(p => p.TrangThai == 1)
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
        /*   public async Task<IActionResult> SavePhieuNhap([FromBody] PhieuNhapViewModel model)
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
           } */
        [HttpPost]
        public async Task<IActionResult> SavePhieuNhap([FromBody] PhieuNhapViewModel model)
        {
            if (model == null || model.ChiTietPhieuNhaps == null || model.ChiTietPhieuNhaps.Count == 0)
                return BadRequest("Dữ liệu phiếu nhập không hợp lệ.");

            using var tran = await _context.Database.BeginTransactionAsync();

            try
            {
                // TÍNH TỔNG
                decimal tongHang = 0, tongCk = 0, tongVat = 0, tongCong = 0;

                foreach (var c in model.ChiTietPhieuNhaps)
                {
                    var tienHang = c.SoLuong * c.DonGiaNhap;

                    // Ở entity ChiTietPhieuNhap bạn đang dùng: ChietKhau là %
                    var ckPhanTram = c.ChietKhau ?? 0;
                    var tienSauCk = tienHang * (1 - ckPhanTram / 100);

                    decimal tienVat = 0;
                    if (c.Vat.HasValue)
                    {
                        if (c.Vat <= 100)
                            tienVat = tienSauCk * (c.Vat.Value / 100);   // VAT là %
                        else
                            tienVat = c.Vat.Value;                       // VAT là số tiền
                    }

                    var thanhTien = tienSauCk + tienVat;

                    tongHang += tienHang;
                    tongCk += (tienHang - tienSauCk);
                    tongVat += tienVat;
                    tongCong += thanhTien;

                }

                PhieuNhap phieu;

                // 🌱 CASE 1: TẠO MỚI
                if (model.PhieuNhapId == null || model.PhieuNhapId == 0)
                {
                    string maPhieu = $"PN-{DateTime.Now:yyMMddHHmmss}";

                    phieu = new PhieuNhap
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
                }
                else
                {
                    // ✏️ CASE 2: CẬP NHẬT
                    phieu = await _context.PhieuNhaps
                        .Include(p => p.ChiTietPhieuNhaps)
                        .FirstOrDefaultAsync(p => p.PhieuNhapId == model.PhieuNhapId.Value);

                    if (phieu == null)
                        return NotFound(new { message = "Phiếu nhập không tồn tại" });

                    phieu.NgayHoaDon = model.NgayHoaDon;
                    phieu.SoHoaDon = model.SoHoaDon;
                    phieu.KyHieuHoaDon = model.KyHieuHoaDon;
                    phieu.NhaCungCapId = model.NhaCungCapId;
                    phieu.NguoiNhap = model.NguoiNhap;
                    phieu.GhiChu = model.GhiChu;
                    phieu.TongTienHang = tongHang;
                    phieu.TongChietKhau = tongCk;
                    phieu.TongThue = tongVat;
                    phieu.TongCong = tongCong;

                    // Xóa chi tiết cũ
                    _context.ChiTietPhieuNhaps.RemoveRange(phieu.ChiTietPhieuNhaps);
                    await _context.SaveChangesAsync();
                }

                // LƯU CHI TIẾT MỚI (dùng model.ChiTietPhieuNhaps như DTO)
                foreach (var c in model.ChiTietPhieuNhaps)
                {
                    var ct = new ChiTietPhieuNhap
                    {
                        PhieuNhapId = phieu.PhieuNhapId,
                        HangHoaId = c.HangHoaId,
                        SoLuong = c.SoLuong,
                        SoLuongQuyDoi = c.SoLuongQuyDoi,
                        DonGiaNhap = c.DonGiaNhap,
                        ChietKhau = c.ChietKhau,
                        Vat = c.Vat,
                        NgaySanXuat = c.NgaySanXuat,
                        HanSuDung = c.HanSuDung,
                        SoLo = c.SoLo,
                        GhiChu = c.GhiChu,
                    };

                    _context.ChiTietPhieuNhaps.Add(ct);
                }

                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                return Ok(new
                {
                    success = true,
                    message = (model.PhieuNhapId == null || model.PhieuNhapId == 0)
                        ? "Lưu phiếu nhập thành công!"
                        : "Cập nhật phiếu nhập thành công!",
                    phieuId = phieu.PhieuNhapId,
                    maPhieu = phieu.MaPhieuNhap
                });
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return BadRequest(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPhieuNhap(int id)
        {
            var phieu = await _context.PhieuNhaps
                .Include(p => p.ChiTietPhieuNhaps)
                    .ThenInclude(c => c.HangHoa) .ThenInclude(h => h.DvtNhap)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.PhieuNhapId == id);

            if (phieu == null)
                return NotFound(new { message = "Phiếu nhập không tồn tại" });

            var result = new
            {
                PhieuNhapId = phieu.PhieuNhapId,
                MaPhieuNhap = phieu.MaPhieuNhap,
                NhaCungCapId = phieu.NhaCungCapId,
                SoHoaDon = phieu.SoHoaDon,
                NgayHoaDon = phieu.NgayHoaDon,
                KyHieuHoaDon = phieu.KyHieuHoaDon,
                GhiChu = phieu.GhiChu,
                ChiTiet = phieu.ChiTietPhieuNhaps.Select(c => new
                {
                    c.HangHoaId,
                    c.SoLuong,
                    c.SoLuongQuyDoi,
                    c.DonGiaNhap,
                    ChietKhauPhanTram = c.ChietKhau ?? 0,   
                   
                    Vat = c.Vat ?? 0,
                    c.NgaySanXuat,
                    c.HanSuDung,
                    c.SoLo,
                    GhiChu = c.GhiChu,

                    DonViTinhNhap = c.HangHoa?.DvtNhap?.TenDvt ?? "",
                }).ToList()
            };

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> ExportExcel([FromBody] ExportRequest request)
        {
            try
            {
                var query = _context.PhieuNhaps
                    .Include(p => p.ChiTietPhieuNhaps)
                        .ThenInclude(c => c.HangHoa)
                    .Include(p => p.NhaCungCap)
                    .AsNoTracking()
                    .AsQueryable();

                // Filter theo Id nếu có
                if (request.Id.HasValue)
                    query = query.Where(p => p.PhieuNhapId == request.Id.Value);

                var data = await query
                    .OrderByDescending(p => p.PhieuNhapId)
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var ws = workbook.Worksheets.Add("Phiếu nhập");

                int row = 1;
                int stt = 1;

                foreach (var p in data)
                {
                    // Header phiếu nhập
                    ws.Cell(row, 1).Value = $"Phiếu nhập: {p.MaPhieuNhap}";
                    ws.Cell(row, 2).Value = $"Ngày nhập: {p.NgayNhap:dd/MM/yyyy}";
                    ws.Cell(row, 3).Value = $"Nhà cung cấp: {p.NhaCungCap?.TenNhaCungCap ?? ""}";
                    ws.Cell(row, 4).Value = $"Tổng tiền hàng: {p.TongTienHang:N0}";
                    ws.Cell(row, 5).Value = $"Tổng chiết khấu: {p.TongChietKhau:N0}";
                    ws.Cell(row, 6).Value = $"Tổng thuế: {p.TongThue:N0}";
                    ws.Cell(row, 7).Value = $"Tổng cộng: {p.TongCong:N0}";
                    ws.Range(row, 1, row, 7).Style.Font.Bold = true;
                    row++;

                    // Header chi tiết
                    string[] headers = {
                "STT", "Tên hàng", "Mã hàng", "Số lượng", "SL Quy đổi",
                "Đơn giá", "Chiết khấu", "VAT", "Thành tiền", "Số lô",
                "NSX", "HSD", "Ghi chú"
            };
                    for (int i = 0; i < headers.Length; i++)
                        ws.Cell(row, i + 1).Value = headers[i];

                    ws.Range(row, 1, row, headers.Length).Style.Font.Bold = true;
                    ws.Range(row, 1, row, headers.Length).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    row++;

                    // Chi tiết
                    if (p.ChiTietPhieuNhaps != null && p.ChiTietPhieuNhaps.Count > 0)
                    {
                        int sttChiTiet = 1;
                        foreach (var c in p.ChiTietPhieuNhaps)
                        {
                            ws.Cell(row, 1).Value = sttChiTiet++;
                            ws.Cell(row, 2).Value = c.HangHoa?.TenHang ?? "";
                            ws.Cell(row, 3).Value = c.HangHoa?.MaHang ?? "";
                            ws.Cell(row, 4).Value = c.SoLuong;
                            ws.Cell(row, 5).Value = c.SoLuongQuyDoi ?? 0;
                            ws.Cell(row, 6).Value = c.DonGiaNhap;
                            ws.Cell(row, 7).Value = c.ChietKhau ?? 0;
                            ws.Cell(row, 8).Value = c.Vat ?? 0;
                            ws.Cell(row, 9).Value = c.ThanhTien ?? 0;
                            ws.Cell(row, 10).Value = c.SoLo ?? "";
                            ws.Cell(row, 11).Value = c.NgaySanXuat?.ToString("dd/MM/yyyy") ?? "";
                            ws.Cell(row, 12).Value = c.HanSuDung?.ToString("dd/MM/yyyy") ?? "";
                            ws.Cell(row, 13).Value = c.GhiChu ?? "";
                            row++;
                        }
                    }

                    row++; // dòng trống giữa các phiếu
                }

                // Auto fit columns
                ws.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);
                var content = stream.ToArray();
                var fileName = $"PhieuNhap_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(content,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ExportPdf([FromBody] ExportRequest request)
        {
            try
            {
                var phieu = await _context.PhieuNhaps
                    .Include(p => p.ChiTietPhieuNhaps)
                        .ThenInclude(c => c.HangHoa)
                    .Include(p => p.NhaCungCap)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.PhieuNhapId == request.Id);

                if (phieu == null)
                    return NotFound("Phiếu nhập không tồn tại");

                var pdf = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(20);

                        // Header thông tin phiếu nhập
                        page.Header().Column(header =>
                        {
                            header.Item().Text($"Phiếu nhập: {phieu.MaPhieuNhap}").FontSize(16).Bold();
                            header.Item().Text($"Ngày nhập: {phieu.NgayNhap:dd/MM/yyyy}");
                            header.Item().Text($"Nhà cung cấp: {phieu.NhaCungCap?.TenNhaCungCap ?? ""}");
                            header.Item().Text($"Tổng tiền hàng: {phieu.TongTienHang:N0}");
                            header.Item().Text($"Tổng chiết khấu: {phieu.TongChietKhau:N0}");
                            header.Item().Text($"Tổng thuế: {phieu.TongThue:N0}");
                            header.Item().Text($"Tổng cộng: {phieu.TongCong:N0}");
                        });

                        page.Content().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30); // STT
                                columns.RelativeColumn();   // Tên hàng
                                columns.RelativeColumn();   // Mã hàng
                                columns.RelativeColumn();   // Số lượng
                                columns.RelativeColumn();   // SL Quy đổi
                                columns.RelativeColumn();   // Đơn giá
                                columns.RelativeColumn();   // Chiết khấu
                                columns.RelativeColumn();   // VAT
                                columns.RelativeColumn();   // Thành tiền
                                columns.RelativeColumn();   // Số lô
                                columns.RelativeColumn();   // NSX
                                columns.RelativeColumn();   // HSD
                                columns.RelativeColumn();   // Ghi chú
                            });

                            // Header chi tiết
                            table.Header(header =>
                            {
                                header.Cell().Text("STT").Bold();
                                header.Cell().Text("Tên hàng").Bold();
                                header.Cell().Text("Mã hàng").Bold();
                                header.Cell().Text("Số lượng").Bold();
                                header.Cell().Text("SL Quy đổi").Bold();
                                header.Cell().Text("Đơn giá").Bold();
                                header.Cell().Text("Chiết khấu").Bold();
                                header.Cell().Text("VAT").Bold();
                                header.Cell().Text("Thành tiền").Bold();
                                header.Cell().Text("Số lô").Bold();
                                header.Cell().Text("NSX").Bold();
                                header.Cell().Text("HSD").Bold();
                                header.Cell().Text("Ghi chú").Bold();
                            });

                            int stt = 1;
                            foreach (var c in phieu.ChiTietPhieuNhaps ?? new List<ChiTietPhieuNhap>())
                            {
                                table.Cell().Text(stt++.ToString());
                                table.Cell().Text(c.HangHoa?.TenHang ?? "");
                                table.Cell().Text(c.HangHoa?.MaHang ?? "");
                                table.Cell().Text(c.SoLuong.ToString("N3"));
                                table.Cell().Text((c.SoLuongQuyDoi ?? 0).ToString("N3"));
                                table.Cell().Text(c.DonGiaNhap.ToString("N0"));
                                table.Cell().Text((c.ChietKhau ?? 0).ToString("N2") + "%");
                                table.Cell().Text((c.Vat ?? 0).ToString("N2") + "%");
                                table.Cell().Text((c.ThanhTien ?? 0).ToString("N0"));
                                table.Cell().Text(c.SoLo ?? "");
                                table.Cell().Text(c.NgaySanXuat?.ToString("dd/MM/yyyy") ?? "");
                                table.Cell().Text(c.HanSuDung?.ToString("dd/MM/yyyy") ?? "");
                                table.Cell().Text(c.GhiChu ?? "");
                            }
                        });

                        page.Footer()
                            .AlignCenter()
                            .Text($"Ngày xuất: {DateTime.Now:dd/MM/yyyy}");
                    });
                }).GeneratePdf();

                return File(pdf, "application/pdf", $"PhieuNhap_{phieu.MaPhieuNhap ?? "unknown"}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeletePhieuNhap([FromBody] DeleteRequest request)
        {
            var phieu = await _context.PhieuNhaps.FirstOrDefaultAsync(p => p.PhieuNhapId == request.Id);
            if (phieu == null) return NotFound(new { message = "Không tìm thấy phiếu nhập" });

            phieu.TrangThai = 0;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Đã xóa phiếu nhập" });
        }

    }
    public class ExportRequest
    {
        public long? Id { get; set; }          // Có thể filter theo Id phiếu

    }
    public class DeleteRequest
    {
        public int Id { get; set; }
    }
}
