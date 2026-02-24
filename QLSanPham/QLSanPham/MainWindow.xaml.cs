using QLSanPham.Models;
using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QLSanPham
{
    public partial class MainWindow : Window
    {
        // Khởi tạo DbContext
        QuanLySanPhamContext db = new QuanLySanPhamContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Sự kiện load cửa sổ
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
            LoadComboBox();
        }

        // Hàm 2.1: Hiển thị danh sách sản phẩm
        private void LoadData()
        {
            var query = db.SanPhams.Select(sp => new
            {
                sp.MaSanPham,
                sp.TenSanPham,
                sp.SoLuong,
                sp.DonGia,
                ThanhTien = sp.SoLuong * sp.DonGia // Tính thành tiền
            })
            .OrderBy(sp => sp.DonGia) // Sắp xếp theo đơn giá tăng dần
            .ToList();

            dgDanhsachSP.ItemsSource = query;
        }

        // Hàm 2.3: Hiển thị ComboBox danh mục
        private void LoadComboBox()
        {
            cbDanhMuc.ItemsSource = db.DanhMucs.ToList();
            cbDanhMuc.DisplayMemberPath = "TenDanhMuc"; // Hiển thị tên
            cbDanhMuc.SelectedValuePath = "MaDanhMuc";  // Lấy giá trị mã
            if (cbDanhMuc.Items.Count > 0)
                cbDanhMuc.SelectedIndex = 0;
        }

        // Hàm 2.2: Xử lý sự kiện nhấn nút Thêm
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra số lượng là số nguyên và > 5
            if (!int.TryParse(txtsoluong.Text, out int soLuong) || soLuong <= 5)
            {
                MessageBox.Show("Số lượng phải là số nguyên và lớn hơn 5!", "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Kiểm tra đơn giá
            if (!decimal.TryParse(txtdongia.Text, out decimal donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ!", "Thông báo lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Tạo đối tượng Sản phẩm mới
            SanPham spNew = new SanPham
            {
                TenSanPham = txttensp.Text,
                SoLuong = soLuong,
                DonGia = donGia,
                MaDanhMuc = (int)cbDanhMuc.SelectedValue
            };

            db.SanPhams.Add(spNew);
            db.SaveChanges(); // Lưu vào CSDL

            LoadData(); // Cập nhật lại DataGrid
            MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo");
        }

        // Xử lý sự kiện nhấn nút Thống kê
        private void btnThongKe_Click(object sender, RoutedEventArgs e)
        {
            // Mở cửa sổ thống kê
            ThongKe window = new ThongKe();
            window.ShowDialog();
        }

        // Xử lý sự kiện nhấn nút Đóng
        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Xử lý khi click vào dòng trong DataGrid (Để trống hoặc fill ngược dữ liệu tùy ý)
        private void SelectSanpham(object sender, SelectionChangedEventArgs e)
        {
            // Tùy chọn: Nếu bạn muốn click vào dòng hiển thị lại lên Textbox thì code ở đây
        }
    }
}