using System;
using System.Linq;
using System.Windows;
using QLSinhVien.Models; // Đảm bảo using đúng thư mục Models của bạn

namespace QLSinhVien
{
    public partial class MainWindow : Window
    {
        QuanLySinhVienContext db = new QuanLySinhVienContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadComboBox();
        }

        private void LoadData()
        {
            // Join 2 bảng để lấy được Tên Địa Chỉ hiển thị lên DataGrid
            var query = db.SinhViens.Select(sv => new
            {
                sv.MaSV,
                sv.TenSV,
                sv.NgaySinh,
                sv.GioiTinh,
                TenDiaChi = sv.DiaChi.TenDiaChi
            }).ToList();

            dgDanhSachSV.ItemsSource = query;
        }

        private void LoadComboBox()
        {
            cbDiaChi.ItemsSource = db.DiaChis.ToList();
            cbDiaChi.DisplayMemberPath = "TenDiaChi";
            cbDiaChi.SelectedValuePath = "MaDiaChi";
            if (cbDiaChi.Items.Count > 0) cbDiaChi.SelectedIndex = 0;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên!");
                return;
            }

            if (dpNgaySinh.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày sinh!");
                return;
            }

            // Lấy giới tính từ StackPanel chứa RadioButton
            string gioiTinh = rbNam.IsChecked == true ? "Nam" : "Nữ";

            SinhVien svNew = new SinhVien
            {
                TenSV = txtTenSV.Text,
                NgaySinh = dpNgaySinh.SelectedDate.Value,
                GioiTinh = gioiTinh,
                MaDiaChi = (int)cbDiaChi.SelectedValue
            };

            db.SinhViens.Add(svNew);
            db.SaveChanges();

            LoadData();
            MessageBox.Show("Thêm sinh viên thành công!");
        }

        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            ThongKe window = new ThongKe();
            window.ShowDialog();
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}