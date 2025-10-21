using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class NuocSanXuat
{
    public long Id { get; set; }

    public string MaNuoc { get; set; } = null!;

    public string TenNuoc { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();

    public virtual ICollection<HangSanXuat> HangSanXuats { get; set; } = new List<HangSanXuat>();
}
