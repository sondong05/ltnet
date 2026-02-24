using System.Collections.Generic; // Dùng List
using System.Windows;

namespace QuanLySinhVien
{
    public partial class LocSinhVien : Window
    {
        // Constructor nhận vào danh sách ĐÃ LỌC SẴN từ bên ngoài
        public LocSinhVien(List<SinhVien> danhSachKetQua)
        {
            InitializeComponent();

            // Hiển thị luôn lên lưới
            dgKetQua.ItemsSource = danhSachKetQua;
        }
    }
}