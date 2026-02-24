using System.Linq;
using System.Windows;
using QLSinhVien.Models;

namespace QLSinhVien
{
    public partial class ThongKe : Window
    {
        QuanLySinhVienContext db = new QuanLySinhVienContext();

        public ThongKe()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Thống kê đếm số lượng sinh viên theo từng địa chỉ
            var thongKe = db.DiaChis.Select(dc => new
            {
                MaDiaChi = dc.MaDiaChi,
                TenDiaChi = dc.TenDiaChi,
                SoNguoi = dc.SinhViens.Count() // Đếm số sinh viên thuộc địa chỉ này
            }).ToList();

            dgThongKe.ItemsSource = thongKe;
        }
    }
}