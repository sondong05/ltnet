using System;
using System.Collections.Generic;

namespace QLSanPham.Models;

public partial class SanPham
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public int? MaDanhMuc { get; set; }

    public virtual DanhMuc? MaDanhMucNavigation { get; set; }
}
