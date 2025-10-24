using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class NhaThau
{
    public long Id { get; set; }

    public string MaNhaThau { get; set; } = null!;

    public string TenNhaThau { get; set; } = null!;

    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public string? NguoiDaiDien { get; set; }
    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
