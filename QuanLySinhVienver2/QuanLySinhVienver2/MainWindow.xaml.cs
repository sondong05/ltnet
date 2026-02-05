using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq; // Để dùng hàm FirstOrDefault, Where
using System.Windows;
using System.Windows.Controls;

namespace QuanLySinhVienver2
{
    public partial class MainWindow : Window
    {
        // Khai báo danh sách sinh viên
        ObservableCollection<SinhVien> ds = new ObservableCollection<SinhVien>();

        public MainWindow()
        {
            InitializeComponent();

            // Thêm dữ liệu mẫu để test
            ds.Add(new SinhVien("SV01", "Nguyễn Văn A", new DateTime(2000, 1, 1), "Nam", "Hà Nội"));
            ds.Add(new SinhVien("SV02", "Trần Thị B", new DateTime(2001, 5, 20), "Nữ", "Hải Phòng"));
            ds.Add(new SinhVien("SV03", "Lê Văn C", new DateTime(2000, 12, 10), "Nam", "Đà Nẵng"));

            // Gán nguồn dữ liệu cho DataGrid
            dgDanhSachSV.ItemsSource = ds;
        }

        // --- HÀM XÓA TRẮNG FORM ---
        private void XoaTextBox()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            dpNgaySinh.SelectedDate = null;
            radNam.IsChecked = true;

            // [THAY ĐỔI]: Xóa trắng TextBox địa chỉ thay vì ComboBox
            txtDiaChi.Clear();

            // Xóa các thông báo lỗi
            lbl_eMaSV.Content = "";
            lbl_eTenSV.Content = "";
            lbl_eNgaySinh.Content = "";

            txtMaSV.Focus();
        }

        // --- CHỨC NĂNG THÊM ---
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text)) { lbl_eMaSV.Content = "Chưa nhập Mã SV"; isValid = false; }
            else lbl_eMaSV.Content = "";

            if (string.IsNullOrWhiteSpace(txtTenSV.Text)) { lbl_eTenSV.Content = "Chưa nhập Tên SV"; isValid = false; }
            else lbl_eTenSV.Content = "";

            if (dpNgaySinh.SelectedDate == null) { lbl_eNgaySinh.Content = "Chưa chọn ngày sinh"; isValid = false; }
            else lbl_eNgaySinh.Content = "";

            if (!isValid) return;

            // 2. Lấy dữ liệu
            string gioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";

            // [THAY ĐỔI]: Lấy địa chỉ từ TextBox
            string diaChi = txtDiaChi.Text.Trim();

            // 3. Tạo đối tượng và thêm
            SinhVien svMoi = new SinhVien(txtMaSV.Text.Trim(), txtTenSV.Text.Trim(), dpNgaySinh.SelectedDate, gioiTinh, diaChi);

            if (ds.Contains(svMoi))
            {
                MessageBox.Show("Mã sinh viên này đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ds.Add(svMoi);
                XoaTextBox();
            }
        }

        // --- CHỨC NĂNG CHỌN DÒNG (Hiển thị lên TextBox) ---
        private void SelectSanpham(object sender, SelectionChangedEventArgs e)
        {
            SinhVien sv = dgDanhSachSV.SelectedItem as SinhVien;
            if (sv != null)
            {
                txtMaSV.Text = sv.MaSV;
                txtTenSV.Text = sv.TenSV;
                dpNgaySinh.SelectedDate = sv.NgaySinh;

                if (sv.GioiTinh == "Nam") radNam.IsChecked = true;
                else radNu.IsChecked = true;

                // [THAY ĐỔI]: Gán dữ liệu vào TextBox địa chỉ
                txtDiaChi.Text = sv.DiaChi;
            }
        }

        // --- CHỨC NĂNG XÓA ---
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                MessageBox.Show("Vui lòng nhập hoặc chọn Mã SV cần xóa.");
                return;
            }

            SinhVien svCanXoa = new SinhVien();
            svCanXoa.MaSV = txtMaSV.Text;

            if (ds.Contains(svCanXoa))
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa SV mã {svCanXoa.MaSV}?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    ds.Remove(svCanXoa);
                    XoaTextBox();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã này để xóa.");
            }
        }

        // --- CHỨC NĂNG CẬP NHẬT ---
        private void btnCapnhat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text)) return;

            SinhVien svGoc = ds.FirstOrDefault(s => s.MaSV.Equals(txtMaSV.Text, StringComparison.OrdinalIgnoreCase));

            if (svGoc != null)
            {
                svGoc.TenSV = txtTenSV.Text;
                svGoc.NgaySinh = dpNgaySinh.SelectedDate;
                svGoc.GioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";

                // [THAY ĐỔI]: Cập nhật địa chỉ từ TextBox
                svGoc.DiaChi = txtDiaChi.Text;

                dgDanhSachSV.Items.Refresh();
                MessageBox.Show("Cập nhật thành công!");
                XoaTextBox();
            }
            else
            {
                MessageBox.Show("Không tìm thấy Mã SV để cập nhật.", "Lỗi");
            }
        }

        // --- CÁC NÚT PHỤ & TÌM KIẾM ---
        private void btnXoatextbox_Click(object sender, RoutedEventArgs e)
        {
            XoaTextBox();
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnTimKiem_Click(object sender, RoutedEventArgs e)
        {
            string tuKhoa = txtTimKiem.Text.Trim();
            if (string.IsNullOrEmpty(tuKhoa)) return;

            SinhVien svTimThay = ds.FirstOrDefault(s => s.MaSV.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase));

            if (svTimThay != null)
            {
                dgDanhSachSV.SelectedItem = svTimThay;
                dgDanhSachSV.ScrollIntoView(svTimThay);
                MessageBox.Show($"Đã tìm thấy sinh viên: {svTimThay.TenSV}");
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã: " + tuKhoa);
            }
        }
        
    }
}