using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class HangHoa
{
    public long Id { get; set; }

    public long MaNhom { get; set; }

    public string MaHang { get; set; } = null!;

    public string TenHang { get; set; } = null!;

    public long DvtNhapId { get; set; }

    public long DvtXuatId { get; set; }

    public decimal SoLuongQuyDoi { get; set; }

    public string? MaDuongDung { get; set; }

    public long? NuocId { get; set; }

    public long? HangSxId { get; set; }

    public string? HamLuong { get; set; }

    public string? HoatChatText { get; set; }

    public string? DangBaoChe { get; set; }

    public string? QuyCachDongGoi { get; set; }

    public string? SoDangKy { get; set; }

    public string MaAnhXa { get; set; } = null!;

    public string? MaBarcode { get; set; }

    public string? MaPpCheBien { get; set; }

    public long NhomChiPhiId { get; set; }

    public long NguonChiTraId { get; set; }

    public bool Bhyt { get; set; }

    public decimal? TiLeBhyt { get; set; }

    public decimal? TiLeThanhToan { get; set; }

    public string? ThongTinThau { get; set; }

    public long? MaNhaThau { get; set; }

    public decimal? GiaThau { get; set; }

    public decimal? SlMin { get; set; }

    public decimal? SlMax { get; set; }

    public int SoNgayDung { get; set; }
    public byte TrangThai { get; set; }

    public virtual DuongDung? DuongDung { get; set; }

    public virtual DonViTinh DvtNhap { get; set; } = null!;

    public virtual DonViTinh DvtXuat { get; set; } = null!;

    public virtual HangSanXuat? HangSx { get; set; }

    public virtual NhomHangHoa NhomHangHoa { get; set; } = null!;

    public virtual NguonChiTra NguonChiTra { get; set; } = null!;

    public virtual NhomChiPhi NhomChiPhi { get; set; } = null!;

    public virtual NuocSanXuat? Nuoc { get; set; }

    public virtual NhaThau? NhaThau { get; set; }
}