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

namespace QuanLyNhanSu.ViewModel
{
    public class LuongViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<BANGLUONG> _ListBangLuong;
        public ObservableCollection<BANGLUONG> ListBangLuong { get => _ListBangLuong; set { _ListBangLuong = value; OnPropertyChanged(); } }

        private ObservableCollection<ThongTinBangLuong> _ListTTBangLuong;
        public ObservableCollection<ThongTinBangLuong> ListTTBangLuong { get => _ListTTBangLuong; set { _ListTTBangLuong = value; OnPropertyChanged(); } }
        #endregion

        #region  Combobox item source
        private ObservableCollection<int> _ListNam;
        public ObservableCollection<int> ListNam { get => _ListNam; set { _ListNam = value; OnPropertyChanged(); } }

        private int _SelectedNam;
        public int SelectedNam { get => _SelectedNam; set { _SelectedNam = value; OnPropertyChanged(); } }

        private ObservableCollection<int> _ListThang;
        public ObservableCollection<int> ListThang { get => _ListThang; set { _ListThang = value; OnPropertyChanged(); } }

        private int _SelectedThang;
        public int SelectedThang { get => _SelectedThang; set { _SelectedThang = value; OnPropertyChanged(); } }

        private ObservableCollection<PHONGBAN> _ListPhongBan;
        public ObservableCollection<PHONGBAN> ListPhongBan { get => _ListPhongBan; set { _ListPhongBan = value; OnPropertyChanged(); } }

        private PHONGBAN _SelectedPhongBan;
        public PHONGBAN SelectedPhongBan { get => _SelectedPhongBan; set { _SelectedPhongBan = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private BANGLUONG _SelectedBangLuong;
        public BANGLUONG SelectedBangLuong { get => _SelectedBangLuong; set { _SelectedBangLuong = value; OnPropertyChanged(); } }

        private int _SoNgayNghiCoLuong;
        public int SoNgayNghiCoLuong { get => _SoNgayNghiCoLuong; set { _SoNgayNghiCoLuong = value; OnPropertyChanged(); } }

        private int _SoNgayNghiKhongLuong;
        public int SoNgayNghiKhongLuong { get => _SoNgayNghiKhongLuong; set { _SoNgayNghiKhongLuong = value; OnPropertyChanged(); } }

        private int _SoNgayLamChinhThuc;
        public int SoNgayLamChinhThuc { get => _SoNgayLamChinhThuc; set { _SoNgayLamChinhThuc = value; OnPropertyChanged(); } }

        private int _SoGioLamTangCa;
        public int SoGioLamTangCa { get => _SoGioLamTangCa; set { _SoGioLamTangCa = value; OnPropertyChanged(); } }

        private decimal _TongLuong;
        public decimal TongLuong { get => _TongLuong; set { _TongLuong = value; OnPropertyChanged(); } }

        #endregion

        #region Binding Command
        public ICommand ChangeCmbCommand { get; set; }
        public ICommand TinhLuongCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        #endregion

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
            #region Khởi tạo
            LoadListPhongBan();

            int[] DSNam = new int[] { 2019, 2020, 2021, 2022, 2023 };
            ListNam = new ObservableCollection<int>(DSNam);

            int[] DSThang = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            ListThang = new ObservableCollection<int>(DSThang);

            #endregion

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

            #region Change Combobox Command 
            ChangeCmbCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedThang == 0 || SelectedNam == 0 || SelectedPhongBan == null)
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                ListBangLuong = new ObservableCollection<BANGLUONG>();
                var lstBL = DataProvider.Ins.model.BANGLUONG.Where(x => x.NHANVIEN.MA_PB == SelectedPhongBan.MA_PB
                                                                     && IsInMonth(x.THANG_BL.Value, SelectedNam, SelectedThang) == true);
                foreach (BANGLUONG item in lstBL)
                {
                    ListBangLuong.Add(item);
                }
            });
            #endregion

            #region Tính lương Command 
            TinhLuongCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedThang == 0 || SelectedNam == 0 || SelectedPhongBan == null)
                {
                    MessageBox.Show("Vui lòng chọn đủ thời gian và phòng ban cần tính lương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                var lstBL = DataProvider.Ins.model.BANGLUONG.Where(x => x.NHANVIEN.MA_PB == SelectedPhongBan.MA_PB
                                                                     && IsInMonth(x.THANG_BL.Value, SelectedNam, SelectedThang) == true);
                if (lstBL != null)
                {
                    MessageBox.Show("Thời gian và phòng ban đã chọn đã được tính lương, vui lòng chọn thời gian hoặc phòng ban khác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                int SoNgayLamViecChung = TinhSoNgayLamViecChung(SelectedNam, SelectedThang);
                var listNhanVien = DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true && x.MA_PB == SelectedPhongBan.MA_PB);

                foreach (NHANVIEN nv in listNhanVien)
                {
                    TongLuong = 0;
                    SoNgayNghiCoLuong = TinhSoNgayNghiPhepCoLuong(nv.MA_NV, SelectedNam, SelectedThang);
                    SoNgayLamChinhThuc = TinhSoNgayLamChinhThuc(nv.MA_NV, SelectedNam, SelectedThang);
                    SoGioLamTangCa = TinhSoGioLamTangCa(nv.MA_NV, SelectedNam, SelectedThang);

                    // Tính các khoản lương theo chấm công
                    var lstKhoanLuong = DataProvider.Ins.model.KHOANLUONG.Where(x => x.MA_NV == nv.MA_NV);
                    foreach (KHOANLUONG kl in lstKhoanLuong)
                    {
                        if (kl.MA_LL == 1)
                        {
                            TongLuong += (decimal)kl.SOTIEN_KL / SoNgayLamViecChung * SoNgayLamChinhThuc;
                            // Tính lương cho các ngày nghỉ phép có lương
                            TongLuong += (decimal)kl.SOTIEN_KL / SoNgayLamViecChung * SoNgayNghiCoLuong;
                        }
                        else if (kl.MA_LL == 2)
                        {
                            TongLuong += (decimal)kl.SOTIEN_KL * SoGioLamTangCa;
                        }
                        else
                        {
                            TongLuong += (decimal)kl.SOTIEN_KL;
                        }
                    }

                    var BangLuongMoi = new BANGLUONG()
                    {
                        MA_NV = nv.MA_NV,
                        THANG_BL = new DateTime(SelectedNam, SelectedThang, 1),
                        TONGLUONG_BL = TongLuong
                    };

                    DataProvider.Ins.model.BANGLUONG.Add(BangLuongMoi);
                    ListBangLuong.Add(BangLuongMoi);
                    MessageBoxResult result = MessageBox.Show("Tính lương thành công! Bạn có muốn lưu bảng lương không? Bảng lương khi được lưu sẽ không thể xoá hoặc chỉnh sửa.", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        DataProvider.Ins.model.SaveChanges();
                        MessageBox.Show("Lưu bảng lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }

            });
            #endregion
        }

        #region Các hàm hỗ trợ
        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
        }

        // Tính tổng số ngày làm việc trong tháng của công ty (không tính các ngày thứ 7 và CN)
        public int TinhSoNgayLamViecChung(int year, int month)
        {
            int result = 0;
            int SoNgayTrongThang = DateTime.DaysInMonth(year, month);

            for (int i = 1; i <= SoNgayTrongThang; i++)
            {
                DateTime dt = new DateTime(year, month, i);
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday)
                {
                    result++;
                }
            }

            return result;
        }

        public bool IsInMonth(DateTime dt, int year, int month)
        {
            if (dt.Month == month && dt.Year == year)
            {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public int TinhSoNgayNghiPhepCoLuong(int manv, int year, int month)
        {
            int result = 0;
            int SoNgayTrongThang = DateTime.DaysInMonth(year, month);

            DateTime firstDay = new DateTime(year, month, 1);
            DateTime lastDay = new DateTime(year, month, SoNgayTrongThang);

            var lstCoLuong = DataProvider.Ins.model.NGHIPHEP.Where(x => x.KHOANNGHIPHEP.LOAINGHIPHEP.COLUONG_LNP == true && x.MA_NV == manv && (IsInMonth(x.NGAYBATDAU_NP.Value, year, month) == true || IsInMonth(x.NGAYKETTHUC_NP.Value, year, month) == true));
            foreach (var item in lstCoLuong)
            {
                if (item.NGAYBATDAU_NP < firstDay)
                {
                    result += (int)(item.NGAYKETTHUC_NP.Value - firstDay).TotalDays + 1;
                }
                else if (item.NGAYKETTHUC_NP > lastDay)
                {
                    result += (int)(lastDay - item.NGAYBATDAU_NP.Value).TotalDays + 1;
                }
                else
                {
                    result += (int)(item.NGAYKETTHUC_NP.Value - item.NGAYBATDAU_NP.Value).TotalDays + 1;
                }
            }
            return result;
        }

        public int TinhSoNgayNghiPhepKhongLuong(int manv, int year, int month)
        {
            int result = 0;
            int SoNgayTrongThang = DateTime.DaysInMonth(year, month);

            DateTime firstDay = new DateTime(year, month, 1);
            DateTime lastDay = new DateTime(year, month, SoNgayTrongThang);

            var lstKhongLuong = DataProvider.Ins.model.NGHIPHEP.Where(x => x.KHOANNGHIPHEP.LOAINGHIPHEP.COLUONG_LNP == false && x.MA_NV == manv && (IsInMonth(x.NGAYBATDAU_NP.Value, year, month) == true || IsInMonth(x.NGAYKETTHUC_NP.Value, year, month) == true));
            foreach (var item in lstKhongLuong)
            {
                if (item.NGAYBATDAU_NP < firstDay)
                {
                    result += (int)(item.NGAYKETTHUC_NP.Value - firstDay).TotalDays + 1;
                    continue;
                }
                else if (item.NGAYKETTHUC_NP > lastDay)
                {
                    result += (int)(lastDay - item.NGAYBATDAU_NP.Value).TotalDays + 1;
                    continue;
                }
                else
                {
                    result += (int)(item.NGAYKETTHUC_NP.Value - item.NGAYBATDAU_NP.Value).TotalDays + 1;
                }
            }
            return result;
        }

        public int TinhSoNgayLamChinhThuc(int manv, int year, int month)
        {
            return DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_LCC == 1 && x.MA_NV == manv && IsInMonth(x.THOIGIANBATDAU_CCN.Value, year, month) == true).Count();
        }

        public int TinhSoGioLamTangCa(int manv, int year, int month)
        {
            int result = 0;
            var lstTangCa = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_LCC == 2 && x.MA_NV == manv && IsInMonth(x.THOIGIANBATDAU_CCN.Value, year, month) == true);
            foreach (var item in lstTangCa)
            {
                result += (int)(item.THOIGIANKETTHUC_CCN.Value - item.THOIGIANBATDAU_CCN.Value).TotalHours;
            }
            return result;
        }

        #endregion
    }
}
