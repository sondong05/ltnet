using System;

namespace QuanLySanPham
{
    public class Sanpham
    {
        public string masp { get; set; }
        public string tensp { get; set; }
        public int soluong { get; set; }
        public double giatien { get; set; }

        public double thanhtien
        {
            get { return soluong * giatien; }
        }

        public Sanpham() { }

        public Sanpham(string ma, string ten, int sl, double gia)
        {
            this.masp = ma;
            this.tensp = ten;
            this.soluong = sl;
            this.giatien = gia;
        }

        // Đảm bảo dòng này và các dòng dưới KHÔNG có chữ lạ
        public override bool Equals(object obj)
        {
            if (obj is Sanpham)
            {
                return this.masp.Equals(((Sanpham)obj).masp, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return masp.GetHashCode();
        }
    }
}