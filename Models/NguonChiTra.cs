using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class NguonChiTra
{
    public long Id { get; set; }

    public string MaNguon { get; set; } = null!;

    public string TenNguon { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
