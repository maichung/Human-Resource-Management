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

        private ObservableCollection<KHOANLUONG> _ListKhoanLuong;
        public ObservableCollection<KHOANLUONG> ListKhoanLuong { get => _ListKhoanLuong; set { _ListKhoanLuong = value; OnPropertyChanged(); } }
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

        private int _SoNgayLamViecChung;
        public int SoNgayLamViecChung { get => _SoNgayLamViecChung; set { _SoNgayLamViecChung = value; OnPropertyChanged(); } }

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

        public ICommand InCommand { get; set; }
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

            SelectedNam = DateTime.Now.Year;
            SelectedThang = DateTime.Now.Month;
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
                                                                     && x.THANG_BL.Value.Month == SelectedThang && x.THANG_BL.Value.Year == SelectedNam);
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
                                                                     && x.THANG_BL.Value.Month == SelectedThang && x.THANG_BL.Value.Year == SelectedNam);
                if (lstBL.Count() > 0)
                {
                    MessageBox.Show("Thời gian và phòng ban đã chọn đã được tính lương, vui lòng chọn thời gian hoặc phòng ban khác!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                SoNgayLamViecChung = TinhSoNgayLamViecChung(SelectedNam, SelectedThang);
                var listNhanVien = DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true && x.MA_PB == SelectedPhongBan.MA_PB);
                if (listNhanVien.Count() == 0)
                {
                    MessageBox.Show("Không có nhân viên trong phòng ban đã chọn, vui lòng thêm nhân viên trước khi tính lương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
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
                }

                MessageBoxResult result = MessageBox.Show("Tính lương thành công! Bạn có muốn lưu bảng lương không? Bảng lương khi được lưu sẽ không thể xoá hoặc chỉnh sửa!", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Lưu bảng lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var changedEntries = DataProvider.Ins.model.ChangeTracker.Entries()
               .Where(x => x.State != System.Data.Entity.EntityState.Unchanged).ToList();

                    foreach (var entry in changedEntries)
                    {
                        switch (entry.State)
                        {
                            case System.Data.Entity.EntityState.Modified:
                                entry.CurrentValues.SetValues(entry.OriginalValues);
                                entry.State = System.Data.Entity.EntityState.Unchanged;
                                break;
                            case System.Data.Entity.EntityState.Added:
                                entry.State = System.Data.Entity.EntityState.Detached;
                                break;
                            case System.Data.Entity.EntityState.Deleted:
                                entry.State = System.Data.Entity.EntityState.Unchanged;
                                break;
                        }
                    }
                }
            });
            #endregion

            #region Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedBangLuong == null)
                {
                    return false;
                }
                return true;
            }, (p) =>
            {
                ResetControls();

                LoadListKhoanLuong(SelectedBangLuong.MA_NV);
                SoNgayLamViecChung = TinhSoNgayLamViecChung(SelectedNam, SelectedThang);
                SoNgayNghiCoLuong = TinhSoNgayNghiPhepCoLuong(SelectedBangLuong.MA_NV, SelectedNam, SelectedThang);
                SoNgayNghiKhongLuong = TinhSoNgayNghiPhepKhongLuong(SelectedBangLuong.MA_NV, SelectedNam, SelectedThang);
                SoNgayLamChinhThuc = TinhSoNgayLamChinhThuc(SelectedBangLuong.MA_NV, SelectedNam, SelectedThang);
                SoGioLamTangCa = TinhSoGioLamTangCa(SelectedBangLuong.MA_NV, SelectedNam, SelectedThang);
                TongLuong = (decimal)SelectedBangLuong.TONGLUONG_BL;

                ChiTietLuongWindow chiTietLuongWindow = new ChiTietLuongWindow();
                chiTietLuongWindow.ShowDialog();
            });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                p.Close();
            });
            #endregion

            #region In Command
            InCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                InChiTietLuong("Chi tiết lương","Chi tiết lương");
                p.Close();
            });
            #endregion
        }

        #region Các hàm hỗ trợ
        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
        }

        public void LoadListKhoanLuong(int manv)
        {
            ListKhoanLuong = new ObservableCollection<KHOANLUONG>();
            var listKhoanLuong = DataProvider.Ins.model.KHOANLUONG.Where(x => x.MA_NV == manv);
            foreach (KHOANLUONG item in listKhoanLuong)
            {
                ListKhoanLuong.Add(item);
            }
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

        public int TinhSoNgayNghiPhepCoLuong(int manv, int year, int month)
        {
            int result = 0;
            int SoNgayTrongThang = DateTime.DaysInMonth(year, month);

            DateTime firstDay = new DateTime(year, month, 1);
            DateTime lastDay = new DateTime(year, month, SoNgayTrongThang);

            var lstCoLuong = DataProvider.Ins.model.NGHIPHEP.Where(x => x.KHOANNGHIPHEP.LOAINGHIPHEP.COLUONG_LNP == true && x.MA_NV == manv
                                                                    && ((x.NGAYBATDAU_NP.Value.Month == month && x.NGAYBATDAU_NP.Value.Year == year) || (x.NGAYKETTHUC_NP.Value.Month == month && x.NGAYKETTHUC_NP.Value.Year == year)));
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

            var lstKhongLuong = DataProvider.Ins.model.NGHIPHEP.Where(x => x.KHOANNGHIPHEP.LOAINGHIPHEP.COLUONG_LNP == false && x.MA_NV == manv
                                                                    && ((x.NGAYBATDAU_NP.Value.Month == month && x.NGAYBATDAU_NP.Value.Year == year) || (x.NGAYKETTHUC_NP.Value.Month == month && x.NGAYKETTHUC_NP.Value.Year == year)));
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
            return DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_LCC == 1 && x.MA_NV == manv && x.THOIGIANBATDAU_CCN.Value.Month == month && x.THOIGIANBATDAU_CCN.Value.Year == year).Count();
        }

        public int TinhSoGioLamTangCa(int manv, int year, int month)
        {
            int result = 0;
            var lstTangCa = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_LCC == 2 && x.MA_NV == manv && x.THOIGIANBATDAU_CCN.Value.Month == month && x.THOIGIANBATDAU_CCN.Value.Year == year);
            foreach (var item in lstTangCa)
            {
                result += (int)(item.THOIGIANKETTHUC_CCN.Value - item.THOIGIANBATDAU_CCN.Value).TotalHours;
            }
            return result;
        }

        public void ResetControls()
        {
            SoNgayLamViecChung = 0;
            SoNgayLamChinhThuc = 0;
            SoGioLamTangCa = 0;
            SoNgayNghiCoLuong = 0;
            SoNgayNghiKhongLuong = 0;
            TongLuong = 0;
            ListKhoanLuong = null;
        }

        public void InChiTietLuong(string sheetName, string title)
        {
            //Tạo các đối tượng Excel

            Microsoft.Office.Interop.Excel.Application oExcel = new Microsoft.Office.Interop.Excel.Application();

            Microsoft.Office.Interop.Excel.Workbooks oBooks;

            Microsoft.Office.Interop.Excel.Sheets oSheets;

            Microsoft.Office.Interop.Excel.Workbook oBook;

            Microsoft.Office.Interop.Excel.Worksheet oSheet;

            //Tạo mới một Excel WorkBook 

            oExcel.Visible = true;

            oExcel.DisplayAlerts = false;

            oExcel.Application.SheetsInNewWorkbook = 1;

            oBooks = oExcel.Workbooks;

            oBook = (Microsoft.Office.Interop.Excel.Workbook)(oExcel.Workbooks.Add(Type.Missing));

            oSheets = oBook.Worksheets;

            oSheet = (Microsoft.Office.Interop.Excel.Worksheet)oSheets.get_Item(1);

            oSheet.Name = sheetName;

            #region Tạo header
            // Tạo phần đầu nếu muốn

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "E1");

            head.MergeCells = true;

            head.Value2 = "Chi tiết lương tháng "+SelectedThang+" năm "+SelectedNam;

            head.Font.Bold = true;

            head.Font.Name = "Arial";

            head.Font.Size = "16";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            Microsoft.Office.Interop.Excel.Range hoTenNhanVien = oSheet.get_Range("A2", "A2");
            hoTenNhanVien.Value2 = "Họ tên nhân viên: ";
            hoTenNhanVien.ColumnWidth = 30;
            oSheet.get_Range("B2", "B2").Value2 = SelectedBangLuong.NHANVIEN.HOTEN_NV;

            oSheet.get_Range("B2", "B2").ColumnWidth=30;
            oSheet.get_Range("D2", "D2").ColumnWidth=30;

            Microsoft.Office.Interop.Excel.Range phongBan = oSheet.get_Range("C2", "C2");
            phongBan.Value2 = "Phòng ban: ";
            phongBan.ColumnWidth = 30;
            oSheet.get_Range("D2", "D2").Value2 = SelectedBangLuong.NHANVIEN.PHONGBAN.TEN_PB;

            Microsoft.Office.Interop.Excel.Range chucVuNhanVien = oSheet.get_Range("A3", "A3");
            chucVuNhanVien.Value2 = "Chức vụ: ";
            oSheet.get_Range("B3", "B3").Value2 = SelectedBangLuong.NHANVIEN.CHUCVU_NV;

            Microsoft.Office.Interop.Excel.Range thangLuong = oSheet.get_Range("C3", "C3");
            thangLuong.Value2 = "Tháng lương: ";
            oSheet.get_Range("D3", "D3").Value2 = SelectedThang;

            Microsoft.Office.Interop.Excel.Range nghiPhepCoLuong = oSheet.get_Range("A4", "A4");
            nghiPhepCoLuong.Value2 = "Nghỉ phép có lương (ngày): ";
            oSheet.get_Range("B4", "B4").Value2 = SoNgayNghiCoLuong;

            Microsoft.Office.Interop.Excel.Range nghiPhepKhongLuong = oSheet.get_Range("C4", "C4");
            nghiPhepKhongLuong.Value2 = "Nghỉ phép không lương (ngày): ";
            oSheet.get_Range("D4", "D4").Value2 = SoNgayNghiKhongLuong;

            Microsoft.Office.Interop.Excel.Range thoiGianDiLamChinhThuc = oSheet.get_Range("A5", "A5");
            thoiGianDiLamChinhThuc.Value2 = "Thời gian đi làm chính thức (ngày): ";
            oSheet.get_Range("B5", "B5").Value2 = SoNgayLamChinhThuc;

            Microsoft.Office.Interop.Excel.Range thoiGianDiLamTangCa = oSheet.get_Range("C5", "C5");
            thoiGianDiLamTangCa.Value2 = "Thời gian đi làm tăng ca (giờ): ";
            oSheet.get_Range("D5", "D5").Value2 = SoGioLamTangCa;

            Microsoft.Office.Interop.Excel.Range loaiLuong = oSheet.get_Range("A6", "A6");
            loaiLuong.Value2 = "Loại lương ";


            Microsoft.Office.Interop.Excel.Range soTien = oSheet.get_Range("B6", "B6");
            soTien.Value2 = "Số tiền";


            #endregion

            #region Thông tin chi tiết lương

            #endregion
            
            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListKhoanLuong.Count, 2];
            for (int row = 0; row < ListKhoanLuong.Count; row++)
            {
                for (int column = 0; column < 2; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListKhoanLuong.ElementAt(row).LOAILUONG.TEN_LL.ToString();
                            break;
                        case 1:
                            value = ((decimal)ListKhoanLuong.ElementAt(row).SOTIEN_KL).ToString("0");
                            break;                      
                            
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[7, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[7 + ListKhoanLuong.Count - 1, 2];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;

            range.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Tổng lương            
            Microsoft.Office.Interop.Excel.Range tongLuong = oSheet.get_Range("A" + (7 + ListKhoanLuong.Count).ToString(), "A" + (7 + ListKhoanLuong.Count).ToString());
            tongLuong.Value2 = "Tổng lương: ";
            tongLuong.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            tongLuong.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            //Giá trị tổng lương
            Microsoft.Office.Interop.Excel.Range giaTriTongLuong = oSheet.get_Range("B" + (7 + ListKhoanLuong.Count).ToString(), "D" + (7 + ListKhoanLuong.Count).ToString());
            giaTriTongLuong.MergeCells = true;
            giaTriTongLuong.Value2 = ((decimal)TongLuong).ToString("N0");
            giaTriTongLuong.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;

            tongLuong.Font.Bold = true;
            giaTriTongLuong.Font.Bold = true;

            oSheet.get_Range("A1", "D" + (7 + ListKhoanLuong.Count)).Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            #endregion

        }


        #endregion
    }
}
