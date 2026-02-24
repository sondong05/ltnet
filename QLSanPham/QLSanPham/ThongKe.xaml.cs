using QLSanPham.Models;
using System;
using System.Linq;
using System.Windows;

namespace QLSanPham
{
    public partial class ThongKe : Window
    {
        QuanLySanPhamContext db = new QuanLySanPhamContext();

        public ThongKe()
        {
            InitializeComponent();
        }

        // Hàm 2.4: Thống kê khi cửa sổ vừa mở lên
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Nhóm theo Danh mục và tính tổng số lượng
            var thongKe = db.DanhMucs.Select(dm => new
            {
                MaDanhMuc = dm.MaDanhMuc,
                TenDanhMuc = dm.TenDanhMuc,
                SoLuongSanPham = dm.SanPhams.Count(), // Đếm số dòng (mặt hàng)
                TongSoLuong = dm.SanPhams.Sum(sp => (int?)sp.SoLuong) ?? 0 // Tổng số lượng sản phẩm của từng nhóm
            }).ToList();

            dgThongKe.ItemsSource = thongKe;
        }
    }
}