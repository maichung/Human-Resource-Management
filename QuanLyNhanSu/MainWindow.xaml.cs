using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyNhanSu.UserControls;

namespace QuanLyNhanSu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnTrangChu_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in gridNoiDung.Children)
            {
                if (element is UserControl)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }

            TrangChuUC trangChuUC = new TrangChuUC();
            gridNoiDung.Children.Add(trangChuUC);
        }

        private void BtnNhanVien_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in gridNoiDung.Children)
            {
                if (element is UserControl)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }

            NhanVienUC nhanVienUC = new NhanVienUC();
            gridNoiDung.Children.Add(nhanVienUC);
        }

        private void BtnPhongBan_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in gridNoiDung.Children)
            {
                if (element is UserControl)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }

            PhongBanUC phongBanUC = new PhongBanUC();
            gridNoiDung.Children.Add(phongBanUC);
        }

        private void BtnTuyenDung_Click(object sender, RoutedEventArgs e)
        {
            foreach (UIElement element in gridNoiDung.Children)
            {
                if (element is UserControl)
                {
                    element.Visibility = Visibility.Hidden;
                }
            }

            UngVienUC ungVienUC = new UngVienUC();
            gridNoiDung.Children.Add(ungVienUC);
        }
    }
}
