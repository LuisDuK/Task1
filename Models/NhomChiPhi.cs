using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class NhomChiPhi
{
    public long Id { get; set; }

    public string MaNhom { get; set; } = null!;

    public string TenNhom { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
