using System.Collections.Generic;
using System.Windows;

namespace QuanLySanPham
{
    public partial class LocSanPham : Window
    {
        
        public LocSanPham(List<Sanpham> li)
        {
            InitializeComponent();
            dgDanhsachSP.ItemsSource = li;
        }
    }
}