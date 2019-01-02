using QuanLyNhanSu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data.Entity;

namespace QuanLyNhanSu.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public enum ChucNangNhanSu
        {
            TrangChu, NhanVien, PhongBan, NghiPhep, ChamCong, Luong, TuyenDung, ChiPhi, BaoCao, CaiDat
        };
        private int _ChucNangNS;
        public int ChucNangNS { get => _ChucNangNS; set { _ChucNangNS = value; OnPropertyChanged(); } }

        private NHANVIEN _NhanVien;
        public NHANVIEN NhanVien { get => _NhanVien; set { _NhanVien = value; OnPropertyChanged(); } }


        #region Items Source
        private ObservableCollection<ImageSource> _ListAVTNhanVienNP1;
        public ObservableCollection<ImageSource> ListAVTNhanVienNP1 { get => _ListAVTNhanVienNP1; set { _ListAVTNhanVienNP1 = value; OnPropertyChanged(); } }
        private ObservableCollection<ImageSource> _ListAVTNhanVienNP7;
        public ObservableCollection<ImageSource> ListAVTNhanVienNP7 { get => _ListAVTNhanVienNP7; set { _ListAVTNhanVienNP7 = value; OnPropertyChanged(); } }

        private ObservableCollection<ImageSource> _ListAVTNhanVienMoi;
        public ObservableCollection<ImageSource> ListAVTNhanVienMoi { get => _ListAVTNhanVienMoi; set { _ListAVTNhanVienMoi = value; OnPropertyChanged(); } }

        private Dictionary<ImageSource, string> _ListNhanVienSinhNhatThang;
        public Dictionary<ImageSource, string> ListNhanVienSinhNhatThang { get => _ListNhanVienSinhNhatThang; set { _ListNhanVienSinhNhatThang = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private int _SoLuongNghiPhep;
        public int SoLuongNghiPhep { get => _SoLuongNghiPhep; set { _SoLuongNghiPhep = value; OnPropertyChanged(); } }
        private int _SoLuongNhanVienMoi;
        public int SoLuongNhanVienMoi { get => _SoLuongNhanVienMoi; set { _SoLuongNhanVienMoi = value; OnPropertyChanged(); } }
        private int _SoLuongSinhNhatThang;
        public int SoLuongSinhNhatThang { get => _SoLuongSinhNhatThang; set { _SoLuongSinhNhatThang = value; OnPropertyChanged(); } }
        private int _SoLuongTuyenDungTuan;
        public int SoLuongTuyenDungTuan { get => _SoLuongTuyenDungTuan; set { _SoLuongTuyenDungTuan = value; OnPropertyChanged(); } }
        private int _SoLuongTuyenDungThang;
        public int SoLuongTuyenDungThang { get => _SoLuongTuyenDungThang; set { _SoLuongTuyenDungThang = value; OnPropertyChanged(); } }
        private ImageSource _AvatarSource;
        public ImageSource AvatarSource { get => _AvatarSource; set { _AvatarSource = value; OnPropertyChanged(); } }
        #endregion


        public ICommand BtnTrangChuCommand { get; set; }
        public ICommand BtnNhanVienCommand { get; set; }
        public ICommand BtnPhongBanCommand { get; set; }
        public ICommand BtnNghiPhepCommand { get; set; }
        public ICommand BtnChamCongCommand { get; set; }
        public ICommand BtnLuongCommand { get; set; }
        public ICommand BtnTuyenDungCommand { get; set; }
        public ICommand BtnChiPhiCommand { get; set; }
        public ICommand BtnBaoCaoCommand { get; set; }
        public ICommand BtnCaiDatCommand { get; set; }

        #region Đăng xuất
        public ICommand DangXuatCommand { get; set; }
        #endregion

        public MainViewModel()
        {
           
            #region Xử lý ẩn hiện Grid
            BtnTrangChuCommand = new RelayCommand<Object>((p) =>
            {
                  return true;
            }, (p) =>
            {
                   ChucNangNS = (int)ChucNangNhanSu.TrangChu;
            });

            BtnNhanVienCommand = new RelayCommand<Grid>((p) =>
            {
                //if (p == null || p.DataContext == null)
                //    return false;
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.NhanVien;
            });

            BtnPhongBanCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.PhongBan;
            });

            BtnNghiPhepCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.NghiPhep;
            });

            BtnChamCongCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.ChamCong;
            });

            BtnLuongCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.Luong;
            });

            BtnTuyenDungCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.TuyenDung;
            });

            BtnChiPhiCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.ChiPhi;
            });

            BtnBaoCaoCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.BaoCao;
            });

            BtnCaiDatCommand = new RelayCommand<Grid>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.CaiDat;
            });
            #endregion


            // Nghỉ phép
            LoadListAvatarNghiPhep1Ngay();
            LoadListAvatarNghiPhep7Ngay();
            LoadListAVTNhanVienMoi();
            LoadListNhanVienSinhNhatThang();
            LoadSoLuong();

            DangXuatCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn đăng xuất khỏi hệ thống không?", "Đăng xuất", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    p.Close();
                }
            });
        }

        public void LoadListAvatarNghiPhep1Ngay()
        {
            ListAVTNhanVienNP1 = new ObservableCollection<ImageSource>();
            DateTime today = DateTime.Today;

            var listAvatar = from nv in DataProvider.Ins.model.NHANVIEN
                             join np in DataProvider.Ins.model.NGHIPHEP
                             on nv.MA_NV equals np.MA_NV
                             where (DbFunctions.DiffDays(np.NGAYBATDAU_NP, DateTime.Now) == 0)
                             select nv.AVATAR_NV;
            foreach (string s in listAvatar)
            {
                BitmapImage bitmapImage = NhanVienViewModel.GetImage(s);
                ListAVTNhanVienNP1.Add(bitmapImage);
            }
        }

        public void LoadListAvatarNghiPhep7Ngay()
        {
            ListAVTNhanVienNP7 = new ObservableCollection<ImageSource>();

            var listAvatar = from nv in DataProvider.Ins.model.NHANVIEN
                             join np in DataProvider.Ins.model.NGHIPHEP
                             on nv.MA_NV equals np.MA_NV
                             where (DbFunctions.DiffDays(np.NGAYBATDAU_NP, DateTime.Now) <= 7)
                             select nv.AVATAR_NV;
            foreach (string s in listAvatar)
            {
                BitmapImage bitmapImage = NhanVienViewModel.GetImage(s);
                ListAVTNhanVienNP7.Add(bitmapImage);
            }
            SoLuongNghiPhep = ListAVTNhanVienNP7.Count();
        }

        public void LoadListAVTNhanVienMoi()
        {
            ListAVTNhanVienMoi = new ObservableCollection<ImageSource>();

            var listAvatar = from nv in DataProvider.Ins.model.NHANVIEN
                             where (DbFunctions.DiffDays(nv.NGAYVAOLAM_NV, DateTime.Now) <= 30)
                             select nv.AVATAR_NV;
            foreach (string s in listAvatar)
            {
                BitmapImage bitmapImage = NhanVienViewModel.GetImage(s);
                ListAVTNhanVienMoi.Add(bitmapImage);
            }
            SoLuongNhanVienMoi = ListAVTNhanVienMoi.Count();
        }

        public void LoadListNhanVienSinhNhatThang()
        {
            ListNhanVienSinhNhatThang = new Dictionary<ImageSource, string>();

            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                               where (DbFunctions.DiffMonths(nv.NGAYSINH_NV, DateTime.Now) == 0)
                               select nv;

            foreach (NHANVIEN nv in listNhanVien)
            {
                string s = (nv.NGAYSINH_NV ?? DateTime.Now).ToString("dd/MM/yyyy");
                ImageSource img = NhanVienViewModel.GetImage(nv.AVATAR_NV);
                ListNhanVienSinhNhatThang.Add(img, s);
            }
            SoLuongSinhNhatThang = ListNhanVienSinhNhatThang.Count();
        }

        public void LoadSoLuong()
        {
            SoLuongTuyenDungTuan = (from td in DataProvider.Ins.model.HOSOUNGTUYEN
                                    where (DbFunctions.DiffDays(DateTime.Now, td.NGAYNOP_HSUT) <= 7)
                                    select td.MA_UV).Count();
            SoLuongTuyenDungThang = (from td in DataProvider.Ins.model.HOSOUNGTUYEN
                                    where ((td.NGAYNOP_HSUT ?? DateTime.Now).Month == DateTime.Now.Month)
                                    select td.MA_UV).Count();
        }
    }
}
