using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace QLHangHoa.Models;

public partial class NhaCungCap
{
    [Key]
    [Column("nha_cung_cap_id")]
    public int NhaCungCapId { get; set; }

    [Required]
    [Column("ten_nha_cung_cap")]
    [StringLength(255)]
    public string TenNhaCungCap { get; set; }

    [Column("dia_chi")]
    [StringLength(255)]
    public string? DiaChi { get; set; }

    [Column("so_dien_thoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("ma_so_thue")]
    [StringLength(50)]
    public string? MaSoThue { get; set; }

    [Column("trang_thai")]
    public bool TrangThai { get; set; } = true;

    // 🔗 Navigation
    public ICollection<PhieuNhap>? PhieuNhaps { get; set; }
}
