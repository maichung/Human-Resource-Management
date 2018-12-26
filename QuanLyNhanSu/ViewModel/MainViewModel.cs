using QuanLyNhanSu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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

        private ImageSource _AvatarSource;
        public ImageSource AvatarSource { get => _AvatarSource; set { _AvatarSource = value; OnPropertyChanged(); } }

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

        }
    }
}
