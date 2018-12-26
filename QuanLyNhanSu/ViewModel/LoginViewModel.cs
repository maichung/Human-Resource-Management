using QuanLyNhanSu.Model;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;

namespace QuanLyNhanSu.ViewModel
{
    class LoginViewModel:BaseViewModel
    {
        #region Thuộc tính Binding
        private string _TenDangNhap;
        public string TenDangNhap { get => _TenDangNhap; set { _TenDangNhap = value; OnPropertyChanged(); } }
        
        public string _MatKhau;
        public string MatKhau { get => _MatKhau; set { _MatKhau = value; OnPropertyChanged(); } }
        #endregion

        #region  Thuộc tính khác
        public bool ktDangNhap { get; set; }
        public NHANVIEN NVDangNhap { get; set; }

        public ICommand DangNhapCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        #endregion

        public LoginViewModel()
        {
            ktDangNhap = false;
            TenDangNhap = "";
            MatKhau = "";
            DangNhapCommand = new RelayCommand<Window>((p) =>
            { return p == null ? false : true; },
            (p) => { DangNhap(p); });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => 
            { return p == null ? false : true; }, (p) =>
                { MatKhau = p.Password; });
        }
        void DangNhap(Window p)
        {
            string matKhauMaHoa = MD5Hash(Base64Encode(MatKhau));
            var taiKhoan = DataProvider.Ins.model.TAIKHOAN.Where(x => x.TENDANGNHAP_TK == TenDangNhap && x.MATKHAU_TK == matKhauMaHoa);
            
            if (taiKhoan.Count() > 0)
            {
              
                ktDangNhap = true;
                int maNV = taiKhoan.SingleOrDefault().MA_NV;
                NVDangNhap = DataProvider.Ins.model.NHANVIEN.Where(x => x.MA_NV == maNV).SingleOrDefault();
            }
            else
            {
                ktDangNhap = false;
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            if (ktDangNhap)
            {
                p.Hide();
                MainWindow mainWindow = new MainWindow();
                if (mainWindow.DataContext == null)
                    return;
                var mainVM = mainWindow.DataContext as MainViewModel;
                mainVM.ChucNangNS = (int)MainViewModel.ChucNangNhanSu.TrangChu;

                mainVM.NhanVien = NVDangNhap;
                mainWindow.ShowDialog();
                p.Close();
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
