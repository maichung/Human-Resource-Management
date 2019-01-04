﻿using QuanLyNhanSu.Model;
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
        #region Thuộc tính quyền tài khoản
        public enum QuyenTaiKhoan
        {
            TruongBoPhan_HCNS, NhanVien_HCNS, TruongBoPhanKhac, QuanTriHeThong
        };
        private int _QuyenTK;
        public int QuyenTK { get => _QuyenTK; set { _QuyenTK = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính ẩn hiện grid
        public enum ChucNangNhanSu
        {
            TrangChu, NhanVien, PhongBan, NghiPhep, ChamCong, Luong, TuyenDung, ChiPhi, BaoCao, CaiDat
        };
        private int _ChucNangNS;
        public int ChucNangNS { get => _ChucNangNS; set { _ChucNangNS = value; OnPropertyChanged(); } }
        private bool _HienNgayLe;
        public bool HienNgayLe { get => _HienNgayLe; set { _HienNgayLe = value; OnPropertyChanged(); } }
        #endregion 

        #region Items Source
        private ObservableCollection<ThongTinNhanVien> _ListNhanVienNP1;
        public ObservableCollection<ThongTinNhanVien> ListNhanVienNP1 { get => _ListNhanVienNP1; set { _ListNhanVienNP1 = value; OnPropertyChanged(); } }

        private ObservableCollection<ThongTinNhanVien> _ListNhanVienNP7;
        public ObservableCollection<ThongTinNhanVien> ListNhanVienNP7 { get => _ListNhanVienNP7; set { _ListNhanVienNP7 = value; OnPropertyChanged(); } }

        private ObservableCollection<ThongTinNhanVien> _ListNhanVienMoi;
        public ObservableCollection<ThongTinNhanVien> ListNhanVienMoi { get => _ListNhanVienMoi; set { _ListNhanVienMoi = value; OnPropertyChanged(); } }

        private ObservableCollection<ThongTinNhanVien> _ListNhanVienSinhNhatThang;
        public ObservableCollection<ThongTinNhanVien> ListNhanVienSinhNhatThang { get => _ListNhanVienSinhNhatThang; set { _ListNhanVienSinhNhatThang = value; OnPropertyChanged(); } }
        
        #endregion

        static public TAIKHOAN TaiKhoan { get; set; }

        #region Thuộc tính binding
        private TAIKHOAN _TaiKhoanHienThi;
        public TAIKHOAN TaiKhoanHienThi { get => _TaiKhoanHienThi; set { _TaiKhoanHienThi = value; OnPropertyChanged(); } }
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

        private ThongTinNgayNghiLe _NgayNghiKeTiep1;
        public ThongTinNgayNghiLe NgayNghiKeTiep1 { get => _NgayNghiKeTiep1; set { _NgayNghiKeTiep1 = value; OnPropertyChanged(); } }
        private ThongTinNgayNghiLe _NgayNghiKeTiep2;
        public ThongTinNgayNghiLe NgayNghiKeTiep2 { get => _NgayNghiKeTiep2; set { _NgayNghiKeTiep2 = value; OnPropertyChanged(); } }
        private ImageSource _AvatarSource;
        public ImageSource AvatarSource { get => _AvatarSource; set { _AvatarSource = value; OnPropertyChanged(); } }

        #endregion

        #region Command cài đặt quyền
        public ICommand SetupQuyenCommand { get; set; }
        #endregion

        #region Command ẩn hiện grid
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
        #endregion

        #region Đăng xuất
        public ICommand DangXuatCommand { get; set; }
        #endregion

        public MainViewModel()
        {
            SetupQuyenCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                SetupQuyenTaiKhoan();
            });
            
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
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                    

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.NhanVien;
            });

            BtnPhongBanCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.PhongBan;
            });

            BtnNghiPhepCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.NghiPhep;
            });

            BtnChamCongCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.ChamCong;
            });

            BtnLuongCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK != (int)QuyenTaiKhoan.TruongBoPhan_HCNS)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.Luong;
            });

            BtnTuyenDungCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.TuyenDung;
            });

            BtnChiPhiCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.ChiPhi;
            });

            BtnBaoCaoCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.QuanTriHeThong)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.BaoCao;
            });

            BtnCaiDatCommand = new RelayCommand<Grid>((p) =>
            {
                if (QuyenTK == (int)QuyenTaiKhoan.TruongBoPhanKhac || QuyenTK == (int)QuyenTaiKhoan.NhanVien_HCNS)
                {
                    MessageBoxResult result = MessageBox.Show("Bạn không đủ quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                ChucNangNS = (int)ChucNangNhanSu.CaiDat;
            });
            #endregion

            #region Load thẻ
            LoadListNghiPhep1Ngay();
            LoadListNghiPhep7Ngay();
            LoadListNhanVienMoi();
            LoadListNhanVienSinhNhatThang();
            LoadSoLuongTuyenDung();
            LoadNgayLeKeTiep();
            #endregion

            #region Đăng xuất command
            DangXuatCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Bạn có chắc chắn đăng xuất khỏi hệ thống không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    p.Hide();
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.ShowDialog();
                    p.Close();
                }
            });
            #endregion
        }

        public void SetupQuyenTaiKhoan()
        {
            if(TaiKhoan.QUYEN_TK == "Trưởng bộ phận Hành chính-Nhân sự")
            {
                QuyenTK = (int)QuyenTaiKhoan.TruongBoPhan_HCNS;
            }
            else if (TaiKhoan.QUYEN_TK == "Nhân viên hành chính nhân sự")
            {
                QuyenTK = (int)QuyenTaiKhoan.NhanVien_HCNS;
            }
            else if (TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                QuyenTK = (int)QuyenTaiKhoan.TruongBoPhanKhac;
            }
            else if (TaiKhoan.QUYEN_TK == "Quản trị hệ thống")
            {
                QuyenTK = (int)QuyenTaiKhoan.QuanTriHeThong;
            }
        }

        #region Các hàm hỗ trợ
        public void LoadListAvatarNghiPhep1Ngay()
        {
            DateTime today = DateTime.Today;

            ListNhanVienNP1 = new ObservableCollection<ThongTinNhanVien>();
            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                             join np in DataProvider.Ins.model.NGHIPHEP
                             on nv.MA_NV equals np.MA_NV
                             where (DbFunctions.DiffDays(np.NGAYBATDAU_NP, DateTime.Now) == 0)
                             select nv;
            foreach (NHANVIEN nv in listNhanVien)
            {
                ListNhanVienNP1.Add(new ThongTinNhanVien
                {
                    NhanVien = nv,
                    NgaySinh = (nv.NGAYSINH_NV ?? DateTime.Now).ToString("dd/MM/yyyy"),
                    Avatar = NhanVienViewModel.GetImage(nv.AVATAR_NV)
                });
            }
        }

        public void LoadListNghiPhep7Ngay()
        {
            ListNhanVienNP7 = new ObservableCollection<ThongTinNhanVien>();
            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                             join np in DataProvider.Ins.model.NGHIPHEP
                             on nv.MA_NV equals np.MA_NV
                             where ((DbFunctions.DiffDays(DateTime.Now, np.NGAYBATDAU_NP) <= 7) && (DbFunctions.DiffDays(DateTime.Now, np.NGAYBATDAU_NP) >= 0))
                             select nv;
            foreach (NHANVIEN nv in listNhanVien)
            {
                ListNhanVienNP7.Add(new ThongTinNhanVien
                {
                    NhanVien = nv,
                    NgaySinh = (nv.NGAYSINH_NV ?? DateTime.Now).ToString("dd/MM/yyyy"),
                    Avatar = NhanVienViewModel.GetImage(nv.AVATAR_NV)
                });
            }

            SoLuongNghiPhep = ListNhanVienNP7.Count();
        }

        public void LoadListNhanVienMoi()
        {
            ListNhanVienMoi = new ObservableCollection<ThongTinNhanVien>();
            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                             where (DbFunctions.DiffDays(nv.NGAYVAOLAM_NV, DateTime.Now) <= 30)
                             select nv;
            foreach (NHANVIEN nv in listNhanVien)
            {
                ListNhanVienMoi.Add(new ThongTinNhanVien
                {
                    NhanVien = nv,
                    NgaySinh = (nv.NGAYSINH_NV ?? DateTime.Now).ToString("dd/MM/yyyy"),
                    Avatar = NhanVienViewModel.GetImage(nv.AVATAR_NV)
                });
            }

            SoLuongNhanVienMoi = ListNhanVienMoi.Count();
        }

        public void LoadListNhanVienSinhNhatThang()
        {
            ListNhanVienSinhNhatThang = new ObservableCollection<ThongTinNhanVien>();

            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                               where (DbFunctions.DiffMonths(nv.NGAYSINH_NV, DateTime.Now) == 0)
                               select nv;

            foreach (NHANVIEN nv in listNhanVien)
            {
                ListNhanVienSinhNhatThang.Add(new ThongTinNhanVien
                {
                    NhanVien = nv,
                    NgaySinh = (nv.NGAYSINH_NV ?? DateTime.Now).ToString("dd/MM/yyyy"),
                    Avatar = NhanVienViewModel.GetImage(nv.AVATAR_NV)
                });
            }
            SoLuongSinhNhatThang = ListNhanVienSinhNhatThang.Count();
        }

        public void LoadSoLuongTuyenDung()
        {
            SoLuongTuyenDungTuan = (from td in DataProvider.Ins.model.HOSOUNGTUYEN
                                    where (DbFunctions.DiffDays(DateTime.Now, td.NGAYNOP_HSUT) <= 7)
                                    select td.MA_UV).Count();
            SoLuongTuyenDungThang = (from td in DataProvider.Ins.model.HOSOUNGTUYEN
                                    where ((td.NGAYNOP_HSUT ?? DateTime.Now).Month == DateTime.Now.Month)
                                    select td.MA_UV).Count();
        }

        public void LoadNgayLeKeTiep()
        {
            var listNgayNghiLe = from nnl in DataProvider.Ins.model.NGAYNGHILE
                                 where (nnl.NGAY_NNL > DateTime.Now)
                                 select nnl;
            List<NGAYNGHILE> sortedListNgayNghiLe = listNgayNghiLe.OrderBy(nnl => nnl.NGAY_NNL).ToList();

            if (sortedListNgayNghiLe.Count >= 1)
            {
                NgayNghiKeTiep1 = new ThongTinNgayNghiLe
                {
                    Ten = sortedListNgayNghiLe[0].TEN_NNL,
                    Ngay = (sortedListNgayNghiLe[0].NGAY_NNL ?? DateTime.Now).Day,
                    Thang = MonthNumberToString((sortedListNgayNghiLe[0].NGAY_NNL ?? DateTime.Now).Month)
                };
            }

            if (sortedListNgayNghiLe.Count >= 2)
            {
                NgayNghiKeTiep2 = new ThongTinNgayNghiLe
                {
                    Ten = sortedListNgayNghiLe[1].TEN_NNL,
                    Ngay = (sortedListNgayNghiLe[1].NGAY_NNL ?? DateTime.Now).Day,
                    Thang = MonthNumberToString((sortedListNgayNghiLe[0].NGAY_NNL ?? DateTime.Now).Month)
                };
            }

            if (sortedListNgayNghiLe.Count == 0)
                HienNgayLe = false;
            else
                HienNgayLe = true;
        }

        public string MonthNumberToString(int m)
        {
            switch (m)
            {
                case 1:
                    return "JAN";
                case 2:
                    return "FEB";
                case 3:
                    return "MAR";
                case 4:
                    return "APR";
                case 5:
                    return "MAY";
                case 6:
                    return "JUN";
                case 7:
                    return "JUL";
                case 8:
                    return "AUG";
                case 9:
                    return "SEP";
                case 10:
                    return "OCT";
                case 11:
                    return "NOV";
                case 12:
                    return "DEC";
                default:
                    return String.Empty;
            }
        }
        #endregion
    }
}
