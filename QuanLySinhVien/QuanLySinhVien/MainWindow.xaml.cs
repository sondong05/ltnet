using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq; // Cần thư viện này để dùng hàm FirstOrDefault
using System.Windows;
using System.Windows.Controls;

namespace QuanLySinhVien // Đổi lại theo namespace project của bạn
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

        // Hàm xóa trắng các ô nhập liệu
        private void XoaTextBox()
        {
            txtMaSV.Clear();
            txtTenSV.Clear();
            dpNgaySinh.SelectedDate = null;
            radNam.IsChecked = true; // Mặc định chọn Nam
            cboDiaChi.SelectedIndex = 0; // Mặc định chọn địa chỉ đầu tiên

            // Xóa các thông báo lỗi nếu có
            lbl_eMaSV.Content = "";
            lbl_eTenSV.Content = "";
            lbl_eNgaySinh.Content = "";

            txtMaSV.Focus();
        }

        // --- CHỨC NĂNG THÊM ---
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // 1. Validation (Kiểm tra dữ liệu)
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(txtMaSV.Text))
            {
                lbl_eMaSV.Content = "Chưa nhập Mã SV";
                isValid = false;
            }
            else lbl_eMaSV.Content = "";

            if (string.IsNullOrWhiteSpace(txtTenSV.Text))
            {
                lbl_eTenSV.Content = "Chưa nhập Tên SV";
                isValid = false;
            }
            else lbl_eTenSV.Content = "";

            if (dpNgaySinh.SelectedDate == null)
            {
                lbl_eNgaySinh.Content = "Chưa chọn ngày sinh";
                isValid = false;
            }
            else lbl_eNgaySinh.Content = "";

            if (!isValid) return; // Nếu có lỗi thì dừng lại

            // 2. Lấy dữ liệu từ giao diện
            string gioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";
            string diaChi = cboDiaChi.Text; // Hoặc lấy từ ComboBoxItem

            // 3. Tạo đối tượng mới
            SinhVien svMoi = new SinhVien(txtMaSV.Text.Trim(), txtTenSV.Text.Trim(), dpNgaySinh.SelectedDate, gioiTinh, diaChi);

            // 4. Kiểm tra trùng mã (Nhờ đã override Equals, ta dùng Contains được ngay)
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

                cboDiaChi.Text = sv.DiaChi;
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

            // Tạo một đối tượng sinh viên chỉ cần có Mã SV để so sánh
            SinhVien svCanXoa = new SinhVien();
            svCanXoa.MaSV = txtMaSV.Text;

            // Kiểm tra xem có trong danh sách không
            if (ds.Contains(svCanXoa))
            {
                MessageBoxResult result = MessageBox.Show($"Bạn có chắc muốn xóa SV mã {svCanXoa.MaSV}?",
                                                          "Xác nhận",
                                                          MessageBoxButton.YesNo,
                                                          MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    // Hàm Remove sẽ gọi Equals để tìm và xóa đúng đối tượng
                    ds.Remove(svCanXoa);
                    XoaTextBox();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy sinh viên có mã này để xóa.");
            }
        }

        // --- CHỨC NĂNG CẬP NHẬT (Không sửa Mã SV) ---
        private void btnCapnhat_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaSV.Text)) return;

            // Tìm sinh viên trong danh sách dựa trên Mã SV đang nhập
            // Sử dụng LINQ FirstOrDefault để tìm object gốc trong bộ nhớ
            SinhVien svGoc = ds.FirstOrDefault(s => s.MaSV.Equals(txtMaSV.Text, StringComparison.OrdinalIgnoreCase));

            if (svGoc != null)
            {
                // Cập nhật thông tin (TRỪ MÃ SINH VIÊN)
                svGoc.TenSV = txtTenSV.Text;
                svGoc.NgaySinh = dpNgaySinh.SelectedDate;
                svGoc.GioiTinh = (radNam.IsChecked == true) ? "Nam" : "Nữ";
                svGoc.DiaChi = cboDiaChi.Text;

                // Refresh lại DataGrid để cập nhật giao diện
                dgDanhSachSV.Items.Refresh();

                MessageBox.Show("Cập nhật thành công!");
                XoaTextBox();
            }
            else
            {
                MessageBox.Show("Không tìm thấy Mã SV để cập nhật. (Lưu ý: Không được sửa Mã SV)", "Lỗi");
            }
        }

        // Các nút phụ
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

            // 1. Kiểm tra nếu chưa nhập gì
            if (string.IsNullOrEmpty(tuKhoa))
            {
                MessageBox.Show("Vui lòng nhập Mã sinh viên cần tìm!");
                txtTimKiem.Focus();
                return;
            }

            // 2. Tìm kiếm trong danh sách (So sánh không phân biệt hoa thường)
            // Dùng hàm FirstOrDefault để lấy sinh viên đầu tiên khớp mã
            SinhVien svTimThay = ds.FirstOrDefault(s => s.MaSV.Equals(tuKhoa, StringComparison.OrdinalIgnoreCase));

            // 3. Xử lý kết quả
            if (svTimThay != null)
            {
                // Nếu tìm thấy:
                // - Chọn dòng đó trên DataGrid
                dgDanhSachSV.SelectedItem = svTimThay;

                // - Cuộn màn hình đến dòng đó (nếu danh sách dài)
                dgDanhSachSV.ScrollIntoView(svTimThay);

                MessageBox.Show($"Đã tìm thấy sinh viên: {svTimThay.TenSV}");
            }
            else
            {
                // Nếu không tìm thấy
                MessageBox.Show("Không tìm thấy sinh viên có mã: " + tuKhoa);

                // Bỏ chọn trên DataGrid (nếu muốn)
                dgDanhSachSV.SelectedItem = null;
            }
        }

        private void btnCuaso2_Click(object sender, RoutedEventArgs e)
        {
            // 1. Lấy tiêu chí lọc từ ComboBox trên màn hình chính
            // (Giả sử bạn dùng chung ComboBox nhập liệu cboDiaChi để chọn tiêu chí lọc)
            string diaChiCanLoc = cboDiaChi.Text;

            // Kiểm tra nếu chưa chọn địa chỉ (hoặc địa chỉ rỗng)
            if (string.IsNullOrEmpty(diaChiCanLoc))
            {
                MessageBox.Show("Vui lòng chọn địa chỉ trên màn hình chính để lọc!");
                cboDiaChi.Focus();
                return;
            }

            // 2. Thực hiện lọc ngay tại đây
            // Lọc ra những sinh viên có địa chỉ giống với lựa chọn
            List<SinhVien> ketQuaLoc = ds.Where(s => s.DiaChi.Equals(diaChiCanLoc, StringComparison.OrdinalIgnoreCase)).ToList();

            // 3. Kiểm tra kết quả
            if (ketQuaLoc.Count > 0)
            {
                // Nếu có dữ liệu -> Mở màn hình 2 và truyền kết quả sang
                LocSinhVien manHinhLoc = new LocSinhVien(ketQuaLoc);
                manHinhLoc.Title = "Danh sách sinh viên tại: " + diaChiCanLoc; // Đổi tiêu đề cho đẹp
                manHinhLoc.ShowDialog();
            }
            else
            {
                // Nếu không có ai -> Thông báo, không cần mở màn hình mới
                MessageBox.Show("Không tìm thấy sinh viên nào ở địa chỉ: " + diaChiCanLoc);
            }
        }
    }
}