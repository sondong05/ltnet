using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVienver2
{
    public class SinhVien
    {
        // Các thuộc tính
        public string MaSV { get; set; }
        public string TenSV { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }

        // Constructor không tham số
        public SinhVien() { }

        // Constructor có tham số
        public SinhVien(string ma, string ten, DateTime? ngaySinh, string gioiTinh, string diaChi)
        {
            this.MaSV = ma;
            this.TenSV = ten;
            this.NgaySinh = ngaySinh;
            this.GioiTinh = gioiTinh;
            this.DiaChi = diaChi;
        }

        // Override Equals để so sánh 2 sinh viên dựa trên Mã SV
        // Giúp các hàm ds.Contains() hoặc ds.Remove() hoạt động chính xác
        public override bool Equals(object obj)
        {
            if (obj is SinhVien)
            {
                SinhVien other = (SinhVien)obj;
                // So sánh không phân biệt hoa thường
                return this.MaSV.Equals(other.MaSV, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Khi override Equals nên override cả GetHashCode để tránh warning
        public override int GetHashCode()
        {
            return MaSV.GetHashCode();
        }
    }
}
