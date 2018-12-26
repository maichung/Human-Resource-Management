using QuanLyNhanSu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Security.Cryptography;


namespace QuanLyNhanSu.ViewModel
{
    class TaiKhoanViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<ThongTinTaikhoan> _ListTaiKhoan;
        public ObservableCollection<ThongTinTaikhoan> ListTaiKhoan { get => _ListTaiKhoan; set { _ListTaiKhoan = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính Binding
        private string _TenTaiKhoan;
        public string TenTaiKhoan { get => _TenTaiKhoan; set { _TenTaiKhoan = value; OnPropertyChanged(); } }

        private string _MatKhau;
        public string MatKhau { get => _MatKhau; set { _MatKhau = value; OnPropertyChanged(); } }
        private string _MatKhauMaHoa;


        private string _NhapLaiMatKhau;
        public string NhapLaiMatKhau { get => _NhapLaiMatKhau; set { _NhapLaiMatKhau = value; OnPropertyChanged(); } }
        private string _NhapLaiMatKhauMaHoa;

        private string _QuyenHan;
        public string QuyenHan { get => _QuyenHan; set { _QuyenHan = value; OnPropertyChanged(); } }

        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }

        private string _SelectedQuyenHan;
        public string SelectedQuyenHan { get => _SelectedQuyenHan; set { _SelectedQuyenHan = value; OnPropertyChanged(); } }

        private ThongTinTaikhoan _SelectedThongTinTaiKhoan;
        public ThongTinTaikhoan SelectedThongTinTaiKhoan { get => _SelectedThongTinTaiKhoan; set { _SelectedThongTinTaiKhoan = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox Itemsource
        private ObservableCollection<string> _ListQuyenHan;
        public ObservableCollection<string> ListQuyenHan { get => _ListQuyenHan; set { _ListQuyenHan = value; OnPropertyChanged(); } }

        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }


 

        #endregion

        #region Thuộc tính khác
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        private string _SearchTaiKhoan;
        public string SearchTaiKhoan { get => _SearchTaiKhoan; set { _SearchTaiKhoan = value; OnPropertyChanged(); } }

        #endregion

        #region Binding Command
        public ICommand ThemTaiKhoanCommand { get; set; }
        public ICommand ThoatCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RepeatPasswordChangedCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }

        public ICommand SearchTaiKhoanCommand { get; set; }
        public ICommand XoaTaiKhoanCommand { get; set; }
        public ICommand SuaTaiKhoanCommand { get; set; }
        public ICommand HienThiTaiKhoanCommand { get; set; }
        #endregion

     
        public TaiKhoanViewModel()
        {
            LoadListTaiKhoan();
            string[] DSQuyenHan = new string[] { "Trưởng bộ phận Hành chính-Nhân sự", "Quản trị hệ thống", "Trưởng các bộ phận khác", "Nhân viên hành chính nhân sự" };
            ListQuyenHan = new ObservableCollection<string>(DSQuyenHan);
            ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN);
            LoadListNhanVien();
            IsEditable = true;

            //Xóa tài khoản command
            XoaTaiKhoanCommand = new RelayCommand<Window>((p) =>
            {

                if ( SelectedThongTinTaiKhoan.TaiKhoan == null)
                {
                     MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    return false;
                }
                return true;
               
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa tài khoản", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {


                    using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                    {
                        try
                        {
                            var tk = DataProvider.Ins.model.TAIKHOAN.Where(x => x.MA_TK == SelectedThongTinTaiKhoan.TaiKhoan.MA_TK).FirstOrDefault();
                            DataProvider.Ins.model.TAIKHOAN.Remove(tk);
                            DataProvider.Ins.model.SaveChanges();
                            transactions.Commit();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            ResetControl();
                            p.Close();

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            transactions.Rollback();
                        }
                        LoadListTaiKhoan();
                    }
                }
                else
                {
                    return;
                }
                    
                
            });

            //Thêm tài khoản command
            ThemTaiKhoanCommand = new RelayCommand<Object>(
                (p) =>
                {
                    LoadListNhanVien();
                    if (ListNhanVien.Count == 0)
                    {
                        MessageBox.Show("Không có nhân viên chưa có tài khoản.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    return true;
                }, (p) =>
                {
                    IsEditable = true;
                    ResetControl();
                   // LoadListNhanVien();

                    TaiKhoanWindow taiKhoanWindow = new TaiKhoanWindow();
                    taiKhoanWindow.ShowDialog();
                });

            //Hiển thị tài khoản command
            HienThiTaiKhoanCommand = new RelayCommand<Window>(
                (p) =>
                {
                    if (SelectedThongTinTaiKhoan == null) return false;
                    return true;
                }, (p) =>
                 {
                     IsEditable = false;
                     TenTaiKhoan = SelectedThongTinTaiKhoan.TaiKhoan.TENDANGNHAP_TK;
                     SelectedQuyenHan = SelectedThongTinTaiKhoan.TaiKhoan.QUYEN_TK;                    
                     ListNhanVien.Add(SelectedThongTinTaiKhoan.NhanVien);
                     SelectedNhanVien = SelectedThongTinTaiKhoan.NhanVien;
                     TaiKhoanWindow taiKhoanWindow = new TaiKhoanWindow();
                     taiKhoanWindow.ShowDialog();
                 });

            //Password changed command
            PasswordChangedCommand = new RelayCommand<PasswordBox>(
                (p) =>
                {
                    return p == null ? false : true;
                }, (p) =>
                 {
                     _MatKhauMaHoa = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(p.Password));
                 });

            RepeatPasswordChangedCommand = new RelayCommand<PasswordBox>(
                  (p) =>
                    {
                        return p == null ? false : true;
                    }, (p) =>
                     {
                         _NhapLaiMatKhauMaHoa = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(p.Password));
                     });

            //Lưu command
            LuuCommand = new RelayCommand<Window>(
                (p) =>
                {

                    if (TenTaiKhoan == "")
                    {
                        MessageBox.Show("Chưa nhập tên tài khoản");
                        return false;
                    }

                    if (SelectedNhanVien == null
                       || SelectedQuyenHan == null)
                    {
                        MessageBox.Show("Vui lòng chọn nhân viên và quyền hạn cho tài khoản.");
                        return false;
                    }
                    if (_MatKhauMaHoa != _NhapLaiMatKhauMaHoa
                    )
                    {
                        MessageBox.Show("Nhập lại mật khẩu và mật khẩu chưa được nhập hoặc không trùng khớp");
                        return false;
                    }                                 
                    return true;
                }, (p) =>
                 {
                     if (SelectedThongTinTaiKhoan == null)
                     {
                         var TaiKhoanMoi = new TAIKHOAN()
                         {
                             TENDANGNHAP_TK = TenTaiKhoan,
                             MATKHAU_TK = _MatKhauMaHoa,
                             QUYEN_TK = SelectedQuyenHan,
                             MA_NV = SelectedNhanVien.MA_NV,
                         };
                         DataProvider.Ins.model.TAIKHOAN.Add(TaiKhoanMoi);
                         DataProvider.Ins.model.SaveChanges();
                         MessageBox.Show("Thêm tài khoản mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                     }
                     else
                     {
                         var TaiKhoanSua = DataProvider.Ins.model.TAIKHOAN.Where(x => x.MA_TK == SelectedThongTinTaiKhoan.TaiKhoan.MA_TK).SingleOrDefault();
                         TaiKhoanSua.TENDANGNHAP_TK = TenTaiKhoan;
                         TaiKhoanSua.MATKHAU_TK = _MatKhauMaHoa;
                         TaiKhoanSua.QUYEN_TK = SelectedQuyenHan;
                         TaiKhoanSua.MA_NV = SelectedNhanVien.MA_NV;
                         DataProvider.Ins.model.SaveChanges();
                         MessageBox.Show("Cập nhật tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                     }
                     LoadListTaiKhoan();
                     LoadListNhanVien();
                     p.Close();
                 });

            //Sửa tài khoản command
            SuaTaiKhoanCommand = new RelayCommand<Object>((p) =>
              {
                  if (SelectedThongTinTaiKhoan==null)
                  {
                      MessageBox.Show("Vui lòng thêm tài khoản trước khi chỉnh sửa", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                      return false;
                  }
                  return true;
              }, (p) =>
             {
                 IsEditable = true;
             });

            //Search tài khoản command
            SearchTaiKhoanCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchTaiKhoan))
                {
                    CollectionViewSource.GetDefaultView(ListTaiKhoan).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListTaiKhoan).Filter = (searchTaiKhoan) =>
                    {
                        return (searchTaiKhoan as ThongTinTaikhoan).TaiKhoan.TENDANGNHAP_TK.IndexOf(SearchTaiKhoan, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
        }
        void ResetControl()
        {
            TenTaiKhoan = null;
            MatKhau = null;
            NhapLaiMatKhau = null;
            SelectedNhanVien = null;
            SelectedQuyenHan = null;
        }

        void LoadListTaiKhoan()
        {
            ListTaiKhoan = new ObservableCollection<ThongTinTaikhoan>();
            var query = from nv in DataProvider.Ins.model.NHANVIEN
                        join tk in DataProvider.Ins.model.TAIKHOAN
                        on nv.MA_NV equals tk.MA_NV
                        select new ThongTinTaikhoan()
                        {
                            NhanVien = nv,
                            TaiKhoan = tk
                        };
            foreach (ThongTinTaikhoan item in query)
            {
                ListTaiKhoan.Add(item);
            }

        }
        void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>();
       
            var query1 = from nv in DataProvider.Ins.model.NHANVIEN
                         where
                               (from tk in DataProvider.Ins.model.TAIKHOAN
                                where
                                    nv.MA_NV == tk.MA_NV
                                select
                                    tk
                                ).FirstOrDefault() == null
                         select nv;

            foreach (NHANVIEN item in query1)
            {               
                ListNhanVien.Add(item);
            }

        }


    }
}
