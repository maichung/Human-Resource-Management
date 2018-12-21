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
    public class NghiPhepViewModel : BaseViewModel
    {
        public enum ChucNangNghiPhep
        {
            NgayNghiPhep, KhoanNghiPhep, LoaiNghiPhep
        };
        private int _ChucNangNP;
        public int ChucNangNP { get => _ChucNangNP; set { _ChucNangNP = value; OnPropertyChanged(); } }

        public ICommand TabNgayNghiPhepCommand { get; set; }
        public ICommand TabKhoanNghiPhepCommand { get; set; }
        public ICommand TabLoaiNghiPhepCommand { get; set; }

        public NghiPhepViewModel()
        {

            #region Xử lý ẩn hiện Grid
            TabNgayNghiPhepCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNP = (int)ChucNangNghiPhep.NgayNghiPhep;
            });

            TabKhoanNghiPhepCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNP = (int)ChucNangNghiPhep.KhoanNghiPhep;
            });

            TabLoaiNghiPhepCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNP = (int)ChucNangNghiPhep.LoaiNghiPhep;
            });
            #endregion

        }
    }
}
