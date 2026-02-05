using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace QuanLySanPham
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Sanpham> ds = new ObservableCollection<Sanpham>();

        public MainWindow()
        {
            InitializeComponent();
            ds.Add(new Sanpham("s1", "Màn hình", 10, 180));
            ds.Add(new Sanpham("s2", "Bàn phím", 40, 600));
            ds.Add(new Sanpham("s3", "Camera", 20, 130));
            ds.Add(new Sanpham("s4", "Headphone", 30, 200));
            ds.Add(new Sanpham("s5", "Chuột", 30, 900));

            dgDanhsachSP.ItemsSource = ds;
        }

        private void btnDong_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void XoaTextBox()
        {
            txtmasp.Clear();
            txttensp.Clear();
            txtsoluong.Clear();
            txtgiatien.Clear();
            txtthanhtien.Clear();

            lbl_eMasp.Content = "";
            lbl_eTensp.Content = "";
            lbl_eSoluong.Content = "";
            lbl_eGiatien.Content = "";

            txtmasp.Focus();
        }

        private void btnXoatextbox_Click(object sender, RoutedEventArgs e)
        {
            XoaTextBox();
        }

        // CHỨC NĂNG THÊM
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(txtmasp.Text)) { lbl_eMasp.Content = "Phải nhập mã SP"; isValid = false; }
            else lbl_eMasp.Content = "";

            if (string.IsNullOrEmpty(txttensp.Text)) { lbl_eTensp.Content = "Phải nhập tên SP"; isValid = false; }
            else lbl_eTensp.Content = "";

            if (!int.TryParse(txtsoluong.Text, out int sl) || sl < 10 || sl > 100)
            { lbl_eSoluong.Content = "Nhập SL từ 10-100"; isValid = false; }
            else lbl_eSoluong.Content = "";

            if (!double.TryParse(txtgiatien.Text, out double gt) || gt < 100 || gt > 1000)
            { lbl_eGiatien.Content = "Nhập giá 100-1000"; isValid = false; }
            else lbl_eGiatien.Content = "";

            if (isValid)
            {
                Sanpham s = new Sanpham(txtmasp.Text, txttensp.Text, int.Parse(txtsoluong.Text), double.Parse(txtgiatien.Text));

                if (!ds.Contains(s))
                {
                    ds.Add(s);
                    XoaTextBox();
                }
                else
                {
                    MessageBox.Show("Trùng mã sản phẩm!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // CHỨC NĂNG XÓA
        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            Sanpham s = new Sanpham();
            s.masp = txtmasp.Text;

            if (ds.Contains(s))
            {
                var r = MessageBox.Show("Bạn có chắc xóa không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (r == MessageBoxResult.Yes)
                {
                    ds.Remove(s); // Remove hoạt động nhờ hàm Equals trong class Sanpham
                    XoaTextBox();
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy mã SP để xóa.");
            }
        }

        // CHỨC NĂNG CẬP NHẬT
        private void btnCapnhat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sanpham s = new Sanpham(txtmasp.Text, txttensp.Text, int.Parse(txtsoluong.Text), double.Parse(txtgiatien.Text));

                int vitri = ds.IndexOf(s); // Tìm vị trí dựa trên Mã SP
                if (vitri != -1)
                {
                    ds[vitri] = s; // Ghi đè thông tin mới
                    dgDanhsachSP.Items.Refresh(); // Cập nhật lại giao diện
                    MessageBox.Show("Cập nhật thành công!");
                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã SP này để cập nhật.");
                }
            }
            catch
            {
                MessageBox.Show("Dữ liệu nhập không hợp lệ.");
            }
        }

        // SỰ KIỆN CHỌN DÒNG
        private void SelectSanpham(object sender, SelectionChangedEventArgs e)
        {
            Sanpham s = dgDanhsachSP.SelectedItem as Sanpham;
            if (s != null)
            {
                txtmasp.Text = s.masp;
                txttensp.Text = s.tensp;
                txtsoluong.Text = s.soluong.ToString();
                txtgiatien.Text = s.giatien.ToString();
                txtthanhtien.Text = s.thanhtien.ToString("N0");
            }
        }

        // CHỨC NĂNG LỌC (Mở cửa sổ 2)
        private void btnCuaso2_Click(object sender, RoutedEventArgs e)
        {
            List<Sanpham> li = ds.Where(s => s.giatien >= 500).ToList();

            LocSanPham cs = new LocSanPham(li);
            cs.ShowDialog();
        }
    }
}