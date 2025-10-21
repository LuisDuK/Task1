using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class DuongDung
{
    public long Id { get; set; }

    public string MaDuong { get; set; } = null!;

    public string TenDuong { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoas { get; set; } = new List<HangHoa>();
}
