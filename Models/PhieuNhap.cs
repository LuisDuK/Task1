using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLHangHoa.Models
{
    [Table("phieu_nhap")]
    public class PhieuNhap
    {
        [Key]
        [Column("phieu_nhap_id")]
        public int PhieuNhapId { get; set; }

        [Required]
        [Column("ma_phieu_nhap")]
        [StringLength(50)]
        public string MaPhieuNhap { get; set; }

        [Column("ngay_nhap")]
        public DateTime NgayNhap { get; set; } = DateTime.Now;

        [Column("ngay_hoa_don")]
        public DateTime? NgayHoaDon { get; set; }

        [Column("so_hoa_don")]
        [StringLength(50)]
        public string? SoHoaDon { get; set; }

        [Column("ky_hieu_hoa_don")]
        [StringLength(50)]
        public string? KyHieuHoaDon { get; set; }

        [Column("nha_cung_cap_id")]
        public int NhaCungCapId { get; set; }

        [Column("nguoi_nhap")]
        [StringLength(100)]
        public string? NguoiNhap { get; set; }

        [Column("ghi_chu")]
        [StringLength(500)]
        public string? GhiChu { get; set; }

        [Column("tong_tien_hang", TypeName = "decimal(18,2)")]
        public decimal TongTienHang { get; set; }

        [Column("tong_chiet_khau", TypeName = "decimal(18,2)")]
        public decimal TongChietKhau { get; set; }

        [Column("tong_thue", TypeName = "decimal(18,2)")]
        public decimal TongThue { get; set; }

        [Column("tong_cong", TypeName = "decimal(18,2)")]
        public decimal TongCong { get; set; }

        [Column("trang_thai")]
        public byte TrangThai { get; set; } = 1;

        // 🔗 Navigation
        [ForeignKey("NhaCungCapId")]
        public NhaCungCap? NhaCungCap { get; set; }

        public ICollection<ChiTietPhieuNhap>? ChiTietPhieuNhaps { get; set; }
    }
    public class PhieuNhapViewModel
    {
        public string SoHoaDon { get; set; }
        public DateTime? NgayHoaDon { get; set; }
        public string KyHieuHoaDon { get; set; }
        public int NhaCungCapId { get; set; }
        public string GhiChu { get; set; }
        public string NguoiNhap { get; set; }

        public List<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    }
}
