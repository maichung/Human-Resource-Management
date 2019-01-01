using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyNhanSu.ViewModel
{
    public class LuongViewModel : BaseViewModel
    {
        #region Thuộc tính ẩn hiện tab
        public enum ChucNangLuong
        {
            BangLuong, KhoanLuong, LoaiLuong
        };
        private int _ChucNangBL;
        public int ChucNangBL { get => _ChucNangBL; set { _ChucNangBL = value; OnPropertyChanged(); } }

        public ICommand TabBangLuongCommand { get; set; }
        public ICommand TabKhoanLuongCommand { get; set; }
        public ICommand TabLoaiLuongCommand { get; set; }
        #endregion

        public LuongViewModel()
        {
            #region Xử lý ẩn hiện tab
            TabBangLuongCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangBL = (int)ChucNangLuong.BangLuong;
            });

            TabKhoanLuongCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangBL = (int)ChucNangLuong.KhoanLuong;
            });

            TabLoaiLuongCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangBL = (int)ChucNangLuong.LoaiLuong;
            });
            #endregion
        }
    }
}
