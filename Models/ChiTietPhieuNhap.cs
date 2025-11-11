using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLHangHoa.Models
{
    [Table("chi_tiet_phieu_nhap")]
    public class ChiTietPhieuNhap
    {
        [Key]
        [Column("chi_tiet_id")]
        public int ChiTietId { get; set; }

        [Column("phieu_nhap_id")]
        public int PhieuNhapId { get; set; }

        [Column("hang_hoa_id")]
        public int HangHoaId { get; set; }

        [Required]
        [Column("so_luong", TypeName = "decimal(18,3)")]
        public decimal SoLuong { get; set; }

        [Column("so_luong_quy_doi", TypeName = "decimal(18,3)")]
        public decimal? SoLuongQuyDoi { get; set; }

        [Required]
        [Column("don_gia_nhap", TypeName = "decimal(18,2)")]
        public decimal DonGiaNhap { get; set; }

        [Column("chiet_khau", TypeName = "decimal(5,2)")]
        public decimal? ChietKhau { get; set; } = 0m;

        [Column("vat", TypeName = "decimal(5,2)")]
        public decimal? Vat { get; set; } = 0m;

        [Column("ngay_san_xuat")]
        public DateTime? NgaySanXuat { get; set; }

        [Column("han_su_dung")]
        public DateTime? HanSuDung { get; set; }

        [Column("so_lo")]
        [StringLength(100)]
        public string? SoLo { get; set; }

        [Column("trang_thai")]
        public byte TrangThai { get; set; } = 1;
        [Column("ghi_chu")]
        [StringLength(255)]
        public string? GhiChu { get; set; }

        // 🔗 Quan hệ
        [ForeignKey("PhieuNhapId")]
        public PhieuNhap? PhieuNhap { get; set; }

        [NotMapped]
        public decimal? ThanhTien
        {
            get
            {
                var ck = (ChietKhau ?? 0) / 100;
                var vat = (Vat ?? 0) / 100;
                return SoLuong * DonGiaNhap * (1 - ck) * (1 + vat);
            }
        }
    }

}
