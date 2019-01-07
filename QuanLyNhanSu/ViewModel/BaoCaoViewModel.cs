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
    public class BaoCaoViewModel : BaseViewModel
    {
        #region Thuộc tính ẩn hiện grid
        public enum ChucNangBaoCao
        {
            NhanVien, PhongBan, NghiPhep, ChamCong, Luong, UngVien, ChiPhi, NgayNghiLe
        };
        private int _ChucNangBC;
        public int ChucNangBC { get => _ChucNangBC; set { _ChucNangBC = value; OnPropertyChanged(); } }
        
        #endregion 

        #region Items Source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        private ObservableCollection<PHONGBAN> _ListPhongBan;
        public ObservableCollection<PHONGBAN> ListPhongBan { get => _ListPhongBan; set { _ListPhongBan = value; OnPropertyChanged(); } }

        private ObservableCollection<NGHIPHEP> _ListNghiPhep;
        public ObservableCollection<NGHIPHEP> ListNghiPhep { get => _ListNghiPhep; set { _ListNghiPhep = value; OnPropertyChanged(); } }

        private ObservableCollection<CHAMCONGNGAY> _ListChamCong;
        public ObservableCollection<CHAMCONGNGAY> ListChamCong { get => _ListChamCong; set { _ListChamCong = value; OnPropertyChanged(); } }

        private ObservableCollection<BANGLUONG> _ListBangLuong;
        public ObservableCollection<BANGLUONG> ListBangLuong { get => _ListBangLuong; set { _ListBangLuong = value; OnPropertyChanged(); } }

        private ObservableCollection<UNGVIEN> _ListUngVien;
        public ObservableCollection<UNGVIEN> ListUngVien { get => _ListUngVien; set { _ListUngVien = value; OnPropertyChanged(); } }

        private ObservableCollection<PHIEUCHI> _ListPhieuChi;
        public ObservableCollection<PHIEUCHI> ListPhieuChi { get => _ListPhieuChi; set { _ListPhieuChi = value; OnPropertyChanged(); } }

        private ObservableCollection<NGAYNGHILE> _ListNgayNghiLe;
        public ObservableCollection<NGAYNGHILE> ListNgayNghiLe { get => _ListNgayNghiLe; set { _ListNgayNghiLe = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác

        private ObservableCollection<string> _ListChucNangBaoCao;
        public ObservableCollection<string> ListChucNangBaoCao { get => _ListChucNangBaoCao; set { _ListChucNangBaoCao = value; OnPropertyChanged(); } }

        private string _SelectedChucNangBaoCao;
        public string SelectedChucNangBaoCao { get => _SelectedChucNangBaoCao; set { _SelectedChucNangBaoCao = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _ListTrangThaiNhanVien;
        public ObservableCollection<string> ListTrangThaiNhanVien { get => _ListTrangThaiNhanVien; set { _ListTrangThaiNhanVien = value; OnPropertyChanged(); } }

        private string _SelectedTrangThaiNhanVien;
        public string SelectedTrangThaiNhanVien { get => _SelectedTrangThaiNhanVien; set { _SelectedTrangThaiNhanVien = value; OnPropertyChanged(); } }

        private ObservableCollection<int> _ListThang;
        public ObservableCollection<int> ListThang { get => _ListThang; set { _ListThang = value; OnPropertyChanged(); } }

        private int _SelectedThang;
        public int SelectedThang { get => _SelectedThang; set { _SelectedThang = value; OnPropertyChanged(); } }

        private int _SelectedNam;
        public int SelectedNam { get => _SelectedNam; set { _SelectedNam = value; OnPropertyChanged(); } }

        private ObservableCollection<int> _ListNam;
        public ObservableCollection<int> ListNam { get => _ListNam; set { _ListNam = value; OnPropertyChanged(); } }

        #endregion

        #region Command ẩn hiện grid

        #endregion

        #region Binding command
        public ICommand ChangedTrangThaiNhanVienCommand { get; set; }
        public ICommand ChangedChucNangBaoCaoCommand { get; set; }
        public ICommand XuatBaoCaoCommand { get; set; }
        public ICommand ChangedThangCommand { get; set; }
        public ICommand ChangedNamCommand { get; set; }
        #endregion

        public BaoCaoViewModel()
        {
            #region Load dữ liệu khi chạy chương trình
            string[] DSChucNangBC = new string[] { "Nhân viên", "Phòng ban", "Nghỉ phép", "Chấm công","Lương",
                "Ứng viên", "Chi phí", "Ngày nghỉ lễ" };
            ListChucNangBaoCao = new ObservableCollection<string>(DSChucNangBC);
            SelectedChucNangBaoCao = "Nhân viên";

            string[] DSTrangThaiNV = new string[] { "Đang làm việc","Đã nghỉ việc"};
            ListTrangThaiNhanVien = new ObservableCollection<string>(DSTrangThaiNV);

            int[] DsThang = new int[] { 0,1,2,3,4,5,6,7,8,9,10,11,12 };
            ListThang = new ObservableCollection<int>(DsThang);
            SelectedThang = 0;

            int[] DsNam = new int[] { 0,2018,2019,2020,2021,2022,2023 };
            ListNam = new ObservableCollection<int>(DsNam);
            SelectedNam = 0;

            SelectedTrangThaiNhanVien = "Đang làm việc";
            LoadListNhanVien();
            LoadListPhongBan();
            LoadListNghiPhep();
            LoadListChamCong();
            LoadListBangLuong();
            LoadListUngvien();
            LoadListPhieuChi();
            LoadListChamCong();
            LoadListNgayNghiLe();
            #endregion

            #region SelectedChangedTrangThaiNhanVien command
            ChangedTrangThaiNhanVienCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                LoadListNhanVien();
            });
            #endregion

            #region ChangedChucNangBaoCaoCommand
            ChangedChucNangBaoCaoCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (SelectedChucNangBaoCao)
                {
                    case "Nhân viên":
                        ChucNangBC = (int)ChucNangBaoCao.NhanVien;
                        break;
                    case "Phòng ban":
                        ChucNangBC = (int)ChucNangBaoCao.PhongBan;
                        break;
                    case "Nghỉ phép":
                        ChucNangBC = (int)ChucNangBaoCao.NghiPhep;
                        break;
                    case "Chấm công":
                        ChucNangBC = (int)ChucNangBaoCao.ChamCong;
                        break;
                    case "Lương":
                        ChucNangBC = (int)ChucNangBaoCao.Luong;
                        break;
                    case "Ứng viên":
                        ChucNangBC = (int)ChucNangBaoCao.UngVien;
                        break;
                    case "Chi phí":
                        ChucNangBC = (int)ChucNangBaoCao.ChiPhi;
                        break;
                    case "Ngày nghỉ lễ":
                        ChucNangBC = (int)ChucNangBaoCao.NgayNghiLe;
                        break;
                   
                }
            });
            #endregion

            #region ChangedThangCommand

            ChangedThangCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (ChucNangBC)
                {
                    case (int)ChucNangBaoCao.ChamCong:
                        LoadListChamCong();
                        break;
                    case (int)ChucNangBaoCao.Luong:
                        LoadListBangLuong();
                        break;
                    case (int)ChucNangBaoCao.ChiPhi:
                        LoadListPhieuChi();
                        break;
                    case (int)ChucNangBaoCao.NgayNghiLe:
                        LoadListNgayNghiLe();
                        break;
                    case (int)ChucNangBaoCao.NghiPhep:
                        LoadListNghiPhep();
                        break;
                }
            });
            #endregion

            #region ChangedNamCommand
            ChangedNamCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (ChucNangBC)
                {
                    case (int)ChucNangBaoCao.ChamCong:
                        LoadListChamCong();
                        break;
                    case (int)ChucNangBaoCao.Luong:
                        LoadListBangLuong();
                        break;
                    case (int)ChucNangBaoCao.ChiPhi:
                        LoadListPhieuChi();
                        break;
                    case (int)ChucNangBaoCao.NgayNghiLe:
                        LoadListNgayNghiLe();
                        break;
                    case (int)ChucNangBaoCao.NghiPhep:
                        LoadListNghiPhep();
                        break;                        
                }
                
            });
            #endregion

            #region Xuất báo cáo command
            XuatBaoCaoCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (ChucNangBC)
                {
                    case (int)ChucNangBaoCao.ChamCong:
                         XuatBaoCaoChamCong("Báo cáo chấm công", "Báo cáo chấm công");
                        break;
                    case (int)ChucNangBaoCao.ChiPhi:
                        XuatBaoCaoChiPhi("Báo cáo chi phí", "Báo cáo chi phí");
                        break;
                    case (int)ChucNangBaoCao.Luong:
                        XuatBaoCaoLuong("Báo cáo lương", "Báo cáo lương");
                        break;
                    case (int)ChucNangBaoCao.NgayNghiLe:
                        XuatBaoCaoNgayNghiLe("Báo cáo ngày nghỉ lễ", "Báo cáo ngày nghỉ lễ");
                        break;
                    case (int)ChucNangBaoCao.NghiPhep:
                        XuatBaoCaoNghiPhep("Báo cáo nghỉ phép", "Báo cáo nghỉ phép");
                        break;
                    case (int)ChucNangBaoCao.NhanVien:
                        XuatBaoCaoNhanVien("Báo cáo nhân viên", "Báo cáo nhân viên");
                        break;
                    case (int)ChucNangBaoCao.PhongBan:
                        XuatBaoCaoPhongBan("Báo cáo phòng ban", "Báo cáo phòng ban");
                        break;
                    case (int)ChucNangBaoCao.UngVien:
                        XuatBaoCaoUngVien("Báo cáo ứng viên", "Báo cáo ứng viên");
                        break;
                }
            });
            #endregion
        }

        #region Load dữ liệu báo cáo
        public void LoadListNhanVien()
        {
            bool TrangThai = SelectedTrangThaiNhanVien == "Đang làm việc" ? true : false;
            ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(nv=>nv.TRANGTHAI_NV==TrangThai));
        }

        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
        }

        public void LoadListNghiPhep()
        {
            if (SelectedThang==0 && SelectedNam==0)
            ListNghiPhep = new ObservableCollection<NGHIPHEP>(DataProvider.Ins.model.NGHIPHEP);
            else
            {
                ListNghiPhep = new ObservableCollection<NGHIPHEP>();
               ObservableCollection <NGHIPHEP> listNghiPhepAll = new ObservableCollection<NGHIPHEP>(DataProvider.Ins.model.NGHIPHEP);
                foreach (NGHIPHEP np in listNghiPhepAll)
                {
                   if (SelectedThang!=0 && SelectedNam !=0)
                    {
                        if (np.NGAYBATDAU_NP.Value.Month == SelectedThang && np.NGAYBATDAU_NP.Value.Year == SelectedNam)
                            ListNghiPhep.Add(np);
                    }
                   else
                    {
                        if (np.NGAYBATDAU_NP.Value.Month == SelectedThang)
                            ListNghiPhep.Add(np);
                        else
                        if (np.NGAYBATDAU_NP.Value.Year == SelectedNam)
                            ListNghiPhep.Add(np);
                    }                        
                }
            }
        }

        public void LoadListChamCong()
        {
            if (SelectedThang == 0 && SelectedNam == 0)
                ListChamCong = new ObservableCollection<CHAMCONGNGAY>(DataProvider.Ins.model.CHAMCONGNGAY);
            else
            {
                ListChamCong = new ObservableCollection<CHAMCONGNGAY>();
                ObservableCollection<CHAMCONGNGAY> ListChamCongAll = new ObservableCollection<CHAMCONGNGAY>(DataProvider.Ins.model.CHAMCONGNGAY);
                foreach (CHAMCONGNGAY ccn in ListChamCongAll)
                {
                    if (SelectedThang != 0 && SelectedNam != 0)
                    {
                        if (ccn.THOIGIANBATDAU_CCN.Value.Month == SelectedThang && ccn.THOIGIANBATDAU_CCN.Value.Year == SelectedNam)
                            ListChamCong.Add(ccn);
                    }
                    else
                    {
                        if (ccn.THOIGIANBATDAU_CCN.Value.Month == SelectedThang)
                            ListChamCong.Add(ccn);
                        else
                        if (ccn.THOIGIANBATDAU_CCN.Value.Year == SelectedNam)
                            ListChamCong.Add(ccn);
                    }
                }
            }
        }

        public void LoadListBangLuong()
        {
            ListBangLuong = new ObservableCollection<BANGLUONG>(DataProvider.Ins.model.BANGLUONG);
            if (SelectedThang == 0 && SelectedNam == 0)
                ListBangLuong = new ObservableCollection<BANGLUONG>(DataProvider.Ins.model.BANGLUONG);
            else
            {
                ListBangLuong = new ObservableCollection<BANGLUONG>();
                ObservableCollection<BANGLUONG> ListBangLuongAll = new ObservableCollection<BANGLUONG>(DataProvider.Ins.model.BANGLUONG);
                foreach (BANGLUONG bl in ListBangLuongAll)
                {
                    if (SelectedThang != 0 && SelectedNam != 0)
                    {
                        if (bl.THANG_BL.Value.Month == SelectedThang && bl.THANG_BL.Value.Year == SelectedNam)
                            ListBangLuong.Add(bl);
                    }
                    else
                    {
                        
                        if (bl.THANG_BL.Value.Month == SelectedThang)
                            ListBangLuong.Add(bl);
                        else
                        if (bl.THANG_BL.Value.Year == SelectedNam)
                            ListBangLuong.Add(bl);
                    }
                }
            }
        }

        public void LoadListUngvien()
        {
            ListUngVien = new ObservableCollection<UNGVIEN>(DataProvider.Ins.model.UNGVIEN);
                       
        }

        public void LoadListPhieuChi()
        {
            ListPhieuChi = new ObservableCollection<PHIEUCHI>(DataProvider.Ins.model.PHIEUCHI);
            if (SelectedThang == 0 && SelectedNam == 0)
                ListPhieuChi = new ObservableCollection<PHIEUCHI>(DataProvider.Ins.model.PHIEUCHI);
            else
            {
                ListPhieuChi = new ObservableCollection<PHIEUCHI>();
                ObservableCollection<PHIEUCHI> ListPhieuChiAll = new ObservableCollection<PHIEUCHI>(DataProvider.Ins.model.PHIEUCHI);
                foreach (PHIEUCHI pc in ListPhieuChiAll)
                {
                    if (SelectedThang != 0 && SelectedNam != 0)
                    {
                        if (pc.THOIGIANLAP_PC.Value.Month == SelectedThang && pc.THOIGIANLAP_PC.Value.Year == SelectedNam)
                            ListPhieuChi.Add(pc);
                    }
                    else
                    {

                        if (pc.THOIGIANLAP_PC.Value.Month == SelectedThang)
                            ListPhieuChi.Add(pc);
                        else
                        if (pc.THOIGIANLAP_PC.Value.Year == SelectedNam)
                            ListPhieuChi.Add(pc);
                    }
                }
            }
        }

        public void LoadListNgayNghiLe()
        {
            ListNgayNghiLe = new ObservableCollection<NGAYNGHILE>(DataProvider.Ins.model.NGAYNGHILE);
            if (SelectedThang == 0 && SelectedNam == 0)
                ListNgayNghiLe = new ObservableCollection<NGAYNGHILE>(DataProvider.Ins.model.NGAYNGHILE);
            else
            {
                ListNgayNghiLe = new ObservableCollection<NGAYNGHILE>();
                ObservableCollection<NGAYNGHILE> ListNgayNghiLeAll = new ObservableCollection<NGAYNGHILE>(DataProvider.Ins.model.NGAYNGHILE);
                foreach (NGAYNGHILE nnl in ListNgayNghiLeAll)
                {
                    if (SelectedThang != 0 && SelectedNam != 0)
                    {
                        if (nnl.NGAY_NNL.Value.Month == SelectedThang && nnl.NGAY_NNL.Value.Year == SelectedNam)
                            ListNgayNghiLe.Add(nnl);
                    }
                    else
                    {
                        if (nnl.NGAY_NNL.Value.Month == SelectedThang)
                            ListNgayNghiLe.Add(nnl);
                        else
                        if (nnl.NGAY_NNL.Value.Year == SelectedNam)
                            ListNgayNghiLe.Add(nnl);
                    }
                }
            }
        }
        #endregion

        #region Xuất báo cáo
        public void XuatBaoCaoUngVien(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "F1");

            head.MergeCells = true;

            head.Value2 = "Danh sách ứng viên";

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã ứng viên";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Họ tên ứng viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Ngày sinh";

            column3.ColumnWidth = 14.0;
            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Giới tính";

            column4.ColumnWidth = 10.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Số điện thoại";

            column5.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range column6 = oSheet.get_Range("F3", "F3");

            column6.Value2 = "Địa chỉ";

            column6.ColumnWidth = 40.0;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "F3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListUngVien.Count, 6];
            for (int row = 0; row < ListUngVien.Count; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListUngVien.ElementAt(row).MA_UV.ToString();
                            break;
                        case 1:
                            value = ListUngVien.ElementAt(row).HOTEN_UV.ToString();
                            break;
                        case 2:
                            value = ListUngVien.ElementAt(row).NGAYSINH_UV.ToString();
                            break;
                        case 3:
                            value = ListUngVien.ElementAt(row).GIOITINH_UV == true ? "Nữ" : "Nam" ;
                            break;
                        case 4:
                            value = ListUngVien.ElementAt(row).SODIENTHOAI_UV.ToString();
                            break;
                        case 5:
                            value = ListUngVien.ElementAt(row).DIACHI_UV.ToString();
                            break;
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListUngVien.Count - 1, 6];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoNhanVien(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "H1");

            head.MergeCells = true;

            head.Value2 = "Danh sách nhân viên " + SelectedTrangThaiNhanVien.ToLower();

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã nhân viên";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Họ tên nhân viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Chức vụ";

            column3.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Phòng ban";

            column4.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Ngày vào làm";

            column5.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column6 = oSheet.get_Range("F3", "F3");

            column6.Value2 = "Email";

            column6.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range column7 = oSheet.get_Range("G3", "G3");

            column7.Value2 = "Số điện thoại";

            column7.ColumnWidth = 15.0;

            Microsoft.Office.Interop.Excel.Range column8 = oSheet.get_Range("H3", "H3");

            column8.Value2 = "Địa chỉ";

            column8.ColumnWidth = 40.0;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "H3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListNhanVien.Count, 8];
            for (int row = 0; row < ListNhanVien.Count; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListNhanVien.ElementAt(row).MA_NV.ToString();
                            break;
                        case 1:
                            value = ListNhanVien.ElementAt(row).HOTEN_NV.ToString();
                            break;
                        case 2:
                            value = ListNhanVien.ElementAt(row).CHUCVU_NV.ToString();
                            break;
                        case 3:
                            value = ListNhanVien.ElementAt(row).PHONGBAN.TEN_PB.ToString();
                            break;
                        case 4:
                            value = ListNhanVien.ElementAt(row).NGAYVAOLAM_NV.Value.ToString();
                            break;
                        case 5:
                            value = ListNhanVien.ElementAt(row).EMAIL_NV.ToString();
                            break;
                        case 6:
                            value = ListNhanVien.ElementAt(row).SODIENTHOAI_NV.ToString();
                            break;
                        case 7:
                            value = ListNhanVien.ElementAt(row).DIACHI_NV.ToString();
                            break;
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListNhanVien.Count - 1, 8];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoPhongBan(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "D1");

            head.MergeCells = true;

            head.Value2 = "Danh sách phòng ban";

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã phòng ban";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Tên phòng ban";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Địa chỉ";

            column3.ColumnWidth = 14.0;
            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Ngày thành lập";

            column4.ColumnWidth = 14.0;


            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "D3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListPhongBan.Count, 4];
            for (int row = 0; row < ListPhongBan.Count; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListPhongBan.ElementAt(row).MA_PB.ToString();
                            break;
                        case 1:
                            value = ListPhongBan.ElementAt(row).TEN_PB.ToString();
                            break;
                        case 2:
                            value = ListPhongBan.ElementAt(row).DIACHI_PB.ToString();
                            break;
                        case 3:
                            value = ListPhongBan.ElementAt(row).NGAYTHANHLAP_PB.Value.ToString();
                            break;

                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListPhongBan.Count - 1, 4];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoNghiPhep(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "F1");

            head.MergeCells = true;

            head.Value2 = "Danh sách nghỉ phép " ;

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã nghỉ phép";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Họ tên nhân viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Tên loại nghỉ phép";

            column3.ColumnWidth = 24.0;

            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Ngày bắt đầu";

            column4.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Ngày kết thúc";

            column5.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column6 = oSheet.get_Range("F3", "F3");

            column6.Value2 = "Lí do";

            column6.ColumnWidth = 20.0;          
            

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "F3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListNghiPhep.Count, 6];
            for (int row = 0; row < ListNghiPhep.Count; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListNghiPhep.ElementAt(row).MA_NV.ToString();
                            break;
                        case 1:
                            value = ListNghiPhep.ElementAt(row).NHANVIEN.HOTEN_NV.ToString();
                            break;
                        case 2:
                            value = ListNghiPhep.ElementAt(row).KHOANNGHIPHEP.LOAINGHIPHEP.TEN_LNP.ToString();
                            break;
                        case 3:
                            value = ListNghiPhep.ElementAt(row).NGAYBATDAU_NP.Value.ToString();
                            break;
                        case 4:
                            value = ListNghiPhep.ElementAt(row).NGAYKETTHUC_NP.Value.ToString();
                            break;
                        case 5:
                            value = ListNghiPhep.ElementAt(row).LIDO_NP.ToString();
                            break;
                      
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListNghiPhep.Count - 1, 6];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoNgayNghiLe(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "C1");

            head.MergeCells = true;

            head.Value2 = "Danh sách ngày nghỉ lễ ";

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã ngày nghỉ lễ";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Ngày";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Tên ngày nghỉ lễ";

            column3.ColumnWidth = 24.0;

         

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "C3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListNgayNghiLe.Count, 3];
            for (int row = 0; row < ListNgayNghiLe.Count; row++)
            {
                for (int column = 0; column < 3; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListNgayNghiLe.ElementAt(row).MA_NNL.ToString();
                            break;
                        case 1:
                            value = ListNgayNghiLe.ElementAt(row).NGAY_NNL.Value.ToString();
                            break;
                        case 2:
                            value = ListNgayNghiLe.ElementAt(row).TEN_NNL.ToString();
                            break;
                     
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListNgayNghiLe.Count - 1, 3];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoLuong(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "H1");

            head.MergeCells = true;

            head.Value2 = "Danh sách lương tháng " + SelectedThang +" năm "+SelectedNam;
            if (SelectedThang == 0 || SelectedNam == 0)
                head.Value2 = "Danh sách lương tháng ";
            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã nhân viên";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = " Tên nhân viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Tháng";

            column3.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Năm";

            column4.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Tổng lương";

            column5.ColumnWidth = 10.0;

            Microsoft.Office.Interop.Excel.Range column6 = oSheet.get_Range("F3", "F3");

            column6.Value2 = "Phòng ban";

            column6.ColumnWidth = 15.0;                    

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "F3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListBangLuong.Count, 6];
            for (int row = 0; row < ListBangLuong.Count; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListBangLuong.ElementAt(row).MA_BL.ToString();
                            break;
                        case 1:
                            value = ListBangLuong.ElementAt(row).NHANVIEN.HOTEN_NV.ToString();
                            break;
                        case 2:
                            value = ListBangLuong.ElementAt(row).THANG_BL.Value.Month.ToString();
                            break;
                        case 3:
                            value = ListBangLuong.ElementAt(row).THANG_BL.Value.Year.ToString();
                            break;
                        case 4:
                            value = ListBangLuong.ElementAt(row).TONGLUONG_BL.ToString();
                            break;
                        case 5:
                            value = ListBangLuong.ElementAt(row).NHANVIEN.PHONGBAN.TEN_PB.ToString();
                            break;
                      
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListBangLuong.Count - 1, 6];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoChiPhi(string sheetName, string title)
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

            head.Value2 = "Danh sách phiếu chi " + SelectedThang + " năm " + SelectedNam;
            if (SelectedThang == 0 || SelectedNam == 0)
                head.Value2 = "Danh sách phiếu chi ";

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã phiếu chi";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Họ tên nhân viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Trị giá";

            column3.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Thời gian lập";

            column4.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Phòng ban";

            column5.ColumnWidth = 10.0;            

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "E3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListPhieuChi.Count, 5];
            for (int row = 0; row < ListPhieuChi.Count; row++)
            {
                for (int column = 0; column < 5; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListPhieuChi.ElementAt(row).MA_PC.ToString();
                            break;
                        case 1:
                            value = ListPhieuChi.ElementAt(row).NHANVIEN.HOTEN_NV.ToString();
                            break;
                        case 2:
                            value = ListPhieuChi.ElementAt(row).TRIGIA_PC.ToString();
                            break;
                        case 3:
                            value = ListPhieuChi.ElementAt(row).THOIGIANLAP_PC.Value.ToString();
                            break;
                        case 4:
                            value = ListPhieuChi.ElementAt(row).NHANVIEN.PHONGBAN.TEN_PB.ToString();
                            break;                      
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListNhanVien.Count - 1, 5];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }

        public void XuatBaoCaoChamCong(string sheetName, string title)
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

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "F1");

            head.MergeCells = true;

            head.Value2 = "Danh sách chấm công " + SelectedThang + " năm " + SelectedNam;
            if (SelectedThang == 0 || SelectedNam == 0)
                head.Value2 = "Danh sách chấm công ";

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            #endregion

            #region Tạo tiêu đề cột
            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range column1 = oSheet.get_Range("A3", "A3");

            column1.Value2 = "Mã chấm công";

            column1.ColumnWidth = 11;

            Microsoft.Office.Interop.Excel.Range column2 = oSheet.get_Range("B3", "B3");

            column2.Value2 = "Họ tên nhân viên";

            column2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range column3 = oSheet.get_Range("C3", "C3");

            column3.Value2 = "Loại chấm công";

            column3.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column4 = oSheet.get_Range("D3", "D3");

            column4.Value2 = "Thời gian bắt đâu";

            column4.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column5 = oSheet.get_Range("E3", "E3");

            column5.Value2 = "Thời gian kết thúc";

            column5.ColumnWidth = 14.0;

            Microsoft.Office.Interop.Excel.Range column6 = oSheet.get_Range("F3", "F3");

            column6.Value2 = "Phòng ban";

            column6.ColumnWidth = 20.0;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "F3");

            rowHead.Font.Bold = true;
            #endregion  

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            #region Đưa dữ liệu vào mảng
            object[,] arrValue = new object[ListChamCong.Count, 6];
            for (int row = 0; row < ListChamCong.Count; row++)
            {
                for (int column = 0; column < 6; column++)
                {
                    string value = "";
                    switch (column)
                    {
                        case 0:
                            value = ListChamCong.ElementAt(row).MA_CCN.ToString();
                            break;
                        case 1:
                            value = ListChamCong.ElementAt(row).NHANVIEN.HOTEN_NV.ToString();
                            break;
                        case 2:
                            value = ListChamCong.ElementAt(row).LOAICHAMCONG.TEN_LCC.ToString();
                            break;
                        case 3:
                            value = ListChamCong.ElementAt(row).THOIGIANBATDAU_CCN.Value.ToString();
                            break;
                        case 4:
                            value = ListChamCong.ElementAt(row).THOIGIANKETTHUC_CCN.Value.ToString();
                            break;
                        case 5:
                            value = (ListChamCong.ElementAt(row).NHANVIEN).PHONGBAN.TEN_PB.ToString();
                            break;
                    }
                    arrValue[row, column] = value;
                }
            }
            #endregion

            #region Điền dữ liệu vào Excel
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListNhanVien.Count - 1, 6];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arrValue;
            #endregion
        }
        #endregion
    }
}
