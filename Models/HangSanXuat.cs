using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class HangSanXuat
{
    public long Id { get; set; }

    public string TenHang { get; set; } = null!;

    public long? NuocId { get; set; }

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();

    public virtual NuocSanXuat? Nuoc { get; set; }
}
