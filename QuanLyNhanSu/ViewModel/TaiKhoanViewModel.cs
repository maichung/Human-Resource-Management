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
        private ObservableCollection<TAIKHOAN> _ListTaiKhoan;
        public ObservableCollection<TAIKHOAN> ListTaiKhoan { get => _ListTaiKhoan; set { _ListTaiKhoan = value; OnPropertyChanged(); } }

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

        private TAIKHOAN _SelectedTaiKhoan;
        public TAIKHOAN SelectedTaiKhoan { get => _SelectedTaiKhoan; set { _SelectedTaiKhoan = value; OnPropertyChanged(); } }
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

        private bool _sort;
        public bool sort { get => _sort; set { _sort = value; OnPropertyChanged(); } }

        #endregion

        #region Binding Command
        public ICommand TaoMoiCommand { get; set; }
        public ICommand ThoatCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RepeatPasswordChangedCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }

        public ICommand SearchCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        #endregion

        #region Thuộc tính ẩn hiện tab
        public enum ChucNangCaiDat
        {
            TaiKhoan, NgayNghiLe, ThongTinPhanMem
        };
        private int _ChucNangCD;
        public int ChucNangCD { get => _ChucNangCD; set { _ChucNangCD = value; OnPropertyChanged(); } }

        public ICommand TabTaiKhoanCommand { get; set; }
        public ICommand TabNgayNghiLeCommand { get; set; }
        public ICommand TabThongTinPhanMemCommand { get; set; }
        #endregion

        public TaiKhoanViewModel()
        {
            #region Xử lý ẩn hiện tab
            TabTaiKhoanCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangCD = (int)ChucNangCaiDat.TaiKhoan;
            });

            TabNgayNghiLeCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangCD = (int)ChucNangCaiDat.NgayNghiLe;
            });

            TabThongTinPhanMemCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangCD = (int)ChucNangCaiDat.ThongTinPhanMem;
            });
            #endregion

            LoadListTaiKhoan();
            string[] DSQuyenHan = new string[] { "Trưởng bộ phận Hành chính-Nhân sự", "Quản trị hệ thống", "Trưởng các bộ phận khác", "Nhân viên hành chính nhân sự" };
            ListQuyenHan = new ObservableCollection<string>(DSQuyenHan);
            LoadListNhanVien();

            #region Xóa tài khoản command
            //Xóa tài khoản command
            XoaCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedTaiKhoan == null)
                {
                    MessageBox.Show("Vui lòng chọn tài khoản trước khi xoá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if(MainViewModel.TaiKhoan == SelectedTaiKhoan)
                {
                    MessageBox.Show("Tài khoản đang sử dụng không thể xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;

            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Thao tác này không thể hoàn tác! Bạn có chắc chắn xoá nhân viên này không? ", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                    {
                        try
                        {
                            var tk = DataProvider.Ins.model.TAIKHOAN.Where(x => x.MA_TK == SelectedTaiKhoan.MA_TK).FirstOrDefault();
                            DataProvider.Ins.model.TAIKHOAN.Remove(tk);
                            DataProvider.Ins.model.SaveChanges();
                            transactions.Commit();
                          
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
            #endregion

            #region Tạo mới command
            //Thêm tài khoản command
            TaoMoiCommand = new RelayCommand<Object>(
                (p) =>
                {
                    LoadListNhanVien();
                    if (ListNhanVien.Count == 0)
                    {
                        MessageBox.Show("Hiện tại không có nhân viên chưa có tài khoản.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    return true;
                }, (p) =>
                {
                    IsEditable = true;
                    SelectedTaiKhoan = null;
                    ResetControl();
                    LoadListNhanVien();

                    TaiKhoanWindow taiKhoanWindow = new TaiKhoanWindow();
                    taiKhoanWindow.ShowDialog();
                });
            #endregion

            #region  Hiển thị command
            //Hiển thị tài khoản command
            HienThiCommand = new RelayCommand<Window>(
                (p) =>
                {
                    if (SelectedTaiKhoan == null) return false;
                    return true;
                }, (p) =>
                 {
                     IsEditable = false;
                     LoadListNhanVien();
                     TenTaiKhoan = SelectedTaiKhoan.TENDANGNHAP_TK;
                     SelectedQuyenHan = SelectedTaiKhoan.QUYEN_TK;
                     SelectedNhanVien = SelectedTaiKhoan.NHANVIEN;
                     ListNhanVien.Add(SelectedTaiKhoan.NHANVIEN);
                     TaiKhoanWindow taiKhoanWindow = new TaiKhoanWindow();
                     taiKhoanWindow.ShowDialog();
                 });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    p.Close();
                }

            });
            #endregion

            #region Passwordchanged command
            //Password changed command
            PasswordChangedCommand = new RelayCommand<PasswordBox>(
                (p) =>
                {
                    return p == null ? false : true;
                }, (p) =>
                 {
                     _MatKhauMaHoa = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(p.Password));
                 });
            #endregion

            #region RepeatPasswordchanged command
            RepeatPasswordChangedCommand = new RelayCommand<PasswordBox>(
                  (p) =>
                    {
                        return p == null ? false : true;
                    }, (p) =>
                     {
                         _NhapLaiMatKhauMaHoa = LoginViewModel.MD5Hash(LoginViewModel.Base64Encode(p.Password));
                     });
            #endregion

            #region Lưu command
            //Lưu command
            LuuCommand = new RelayCommand<Window>(
                (p) =>
                {
                    if (TenTaiKhoan == null)
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }

                    if (SelectedNhanVien == null
                       || SelectedQuyenHan == null)
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    if (_MatKhauMaHoa != _NhapLaiMatKhauMaHoa
                    )
                    {
                        MessageBox.Show("Nhập lại mật khẩu và mật khẩu chưa được nhập hoặc không trùng khớp");
                        return false;
                    }
                    if (IsEditable == false)
                    {
                        MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return false;
                    }
                    return true;
                }, (p) =>
                 {
                     if (SelectedTaiKhoan == null)
                     {
                         if (KiemTraTenDangNhap(TenTaiKhoan) == false)
                         {
                             MessageBox.Show("Tên đăng nhập đã tồn tại, vui lòng nhập tên đăng nhập khác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                             return;
                         }

                         var TaiKhoanMoi = new TAIKHOAN()
                         {
                             TENDANGNHAP_TK = TenTaiKhoan,
                             MATKHAU_TK = _MatKhauMaHoa,
                             QUYEN_TK = SelectedQuyenHan,
                             MA_NV = SelectedNhanVien.MA_NV,
                         };                         

                         DataProvider.Ins.model.TAIKHOAN.Add(TaiKhoanMoi);
                         DataProvider.Ins.model.SaveChanges();
                         ListTaiKhoan.Add(TaiKhoanMoi);
                         MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                     }
                     else
                     {
                         if (KiemTraTenDangNhap(TenTaiKhoan) == false)
                         {
                             MessageBox.Show("Tên đăng nhập đã tồn tại, vui lòng nhập tên đăng nhập khác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                             return;
                         }

                         var TaiKhoanSua = DataProvider.Ins.model.TAIKHOAN.Where(x => x.MA_TK == SelectedTaiKhoan.MA_TK).SingleOrDefault();
                         TaiKhoanSua.TENDANGNHAP_TK = TenTaiKhoan;
                         TaiKhoanSua.MATKHAU_TK = _MatKhauMaHoa;
                         TaiKhoanSua.QUYEN_TK = SelectedQuyenHan;
                         TaiKhoanSua.MA_NV = SelectedNhanVien.MA_NV;                         

                         DataProvider.Ins.model.SaveChanges();
                         MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                     }
                     LoadListNhanVien();
                     p.Close();
                 });
            #endregion

            #region Sửa command
            //Sửa tài khoản command
            SuaCommand = new RelayCommand<Object>((p) =>
              {
                  if (SelectedTaiKhoan == null)
                  {
                      MessageBoxResult result = MessageBox.Show("Vui lòng chọn tài khoản trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                      return false;
                  }
                  return true;
              }, (p) =>
             {
                 IsEditable = true;
             });
            #endregion

            #region Search command
            //Search tài khoản command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchTaiKhoan))
                {
                    CollectionViewSource.GetDefaultView(ListTaiKhoan).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListTaiKhoan).Filter = (searchTaiKhoan) =>
                    {
                        return (searchTaiKhoan as TAIKHOAN).TENDANGNHAP_TK.IndexOf(SearchTaiKhoan, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            #endregion

            #region Sort commmand
            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTaiKhoan);
                if (sort)
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Ascending));
                }
                else
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Descending));
                }
                sort = !sort;
            });
            #endregion
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
            ListTaiKhoan = new ObservableCollection<TAIKHOAN>(DataProvider.Ins.model.TAIKHOAN);
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
                if (item.TRANGTHAI_NV == true)
                    ListNhanVien.Add(item);
            }

        }

        bool KiemTraTenDangNhap(string tdn)
        {
            var taiKhoan = DataProvider.Ins.model.TAIKHOAN.Where(x => x.TENDANGNHAP_TK == tdn).SingleOrDefault();

            if (taiKhoan != null)
                return false;

            return true;
        }
    }
}
