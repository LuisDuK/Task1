using System;
using System.Collections.Generic;

namespace QLHangHoa.Models;

public partial class DonViTinh
{
    public long Id { get; set; }

    public string MaDvt { get; set; } = null!;

    public string TenDvt { get; set; } = null!;

    public virtual ICollection<HangHoa> HangHoaDvtNhaps { get; set; } = new List<HangHoa>();

    public virtual ICollection<HangHoa> HangHoaDvtXuats { get; set; } = new List<HangHoa>();
}
