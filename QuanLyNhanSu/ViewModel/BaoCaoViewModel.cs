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
            NhanVien,PhongBan,NghiPhep,ChamCong,Luong,UngVien,ChiPhi,NgayNghiLe
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
            string[] DSChucNangBC = new string[] { "Nhân viên", "Phòng ban", "Nghỉ phép trong tháng", "Chấm công trong tháng","Lương trong tháng",
                "Ứng viên trong tháng", "Chi phí trong tháng", "Ngày nghỉ lễ năm" };
            ListChucNangBaoCao = new ObservableCollection<string>(DSChucNangBC);
            SelectedChucNangBaoCao = "Nhân viên";

            string[] DSTrangThaiNV = new string[] { "Đang làm việc","Đã nghỉ việc"};
            ListTrangThaiNhanVien = new ObservableCollection<string>(DSTrangThaiNV);

            int[] DsThang = new int[] { 1,2,3,4,5,6,7,8,9,10,11,12 };
            ListThang = new ObservableCollection<int>(DsThang);
            SelectedThang = 0;

            int[] DsNam = new int[] { 2018,2019,2020,2021,2022,2023 };
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
                    case "Nghỉ phép trong tháng":
                        ChucNangBC = (int)ChucNangBaoCao.NghiPhep;
                        break;
                    case "Chấm công trong tháng":
                        ChucNangBC = (int)ChucNangBaoCao.ChamCong;
                        break;
                    case "Lương trong tháng":
                        ChucNangBC = (int)ChucNangBaoCao.Luong;
                        break;
                    case "Ứng viên trong tháng":
                        ChucNangBC = (int)ChucNangBaoCao.UngVien;
                        break;
                    case "Chi phí trong tháng":
                        ChucNangBC = (int)ChucNangBaoCao.ChiPhi;
                        break;
                    case "Ngày nghỉ lễ năm":
                        ChucNangBC = (int)ChucNangBaoCao.NgayNghiLe;
                        break;
                   
                }
            });
            #endregion

            #region ChangedThangCommand

            #endregion

            #region ChangedNamCommand

            #endregion

            #region Báo cáo ứng viên command
            XuatBaoCaoCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                switch (ChucNangBC)
                {
                    case (int)ChucNangBaoCao.UngVien:
                        XuatBaoCaoUngVien("Báo cáo ứng viên", "Báo cáo ứng viên");
                        break;
            }
            });
            #endregion
        }

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
                ObservableCollection<NGHIPHEP> listNghiPhepAll = new ObservableCollection<NGHIPHEP>(DataProvider.Ins.model.NGHIPHEP);
                foreach (NGHIPHEP np in listNghiPhepAll)
                {
                   if (SelectedThang!=0 && SelectedNam !=0)
                    {
                      //  if ( np.NGAYBATDAU_NP)
                    }
                        
                }
            }
        }

        public void LoadListChamCong()
        {
            ListChamCong = new ObservableCollection<CHAMCONGNGAY>(DataProvider.Ins.model.CHAMCONGNGAY);
        }

        public void LoadListBangLuong()
        {
            ListBangLuong = new ObservableCollection<BANGLUONG>(DataProvider.Ins.model.BANGLUONG);
        }

        public void LoadListUngvien()
        {
            ListUngVien = new ObservableCollection<UNGVIEN>(DataProvider.Ins.model.UNGVIEN);
        }

        public void LoadListPhieuChi()
        {
            ListPhieuChi = new ObservableCollection<PHIEUCHI>(DataProvider.Ins.model.PHIEUCHI);
        }
        public void LoadListNgayNghiLe()
        {
            ListNgayNghiLe = new ObservableCollection<NGAYNGHILE>(DataProvider.Ins.model.NGAYNGHILE);
        }

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

            // Tạo phần đầu nếu muốn

            Microsoft.Office.Interop.Excel.Range head = oSheet.get_Range("A1", "C1");

            head.MergeCells = true;

            head.Value2 = title;

            head.Font.Bold = true;

            head.Font.Name = "Tahoma";

            head.Font.Size = "18";

            head.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;

            // Tạo tiêu đề cột 

            Microsoft.Office.Interop.Excel.Range cl1 = oSheet.get_Range("A3", "A3");

            cl1.Value2 = "Mã ứng viên";

            cl1.ColumnWidth = 13.5;

            Microsoft.Office.Interop.Excel.Range cl2 = oSheet.get_Range("B3", "B3");

            cl2.Value2 = "Tên ứng viên";

            cl2.ColumnWidth = 25.0;

            Microsoft.Office.Interop.Excel.Range cl3 = oSheet.get_Range("C3", "C3");

            cl3.Value2 = "Địa chỉ";

            cl3.ColumnWidth = 40.0;

            Microsoft.Office.Interop.Excel.Range rowHead = oSheet.get_Range("A3", "C3");

            rowHead.Font.Bold = true;

            // Kẻ viền

            rowHead.Borders.LineStyle = Microsoft.Office.Interop.Excel.Constants.xlSolid;

            // Thiết lập màu nền

            rowHead.Interior.ColorIndex = 15;

            rowHead.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;


            ListUngVien.GroupBy(x => x.MA_UV);
            object[,] arr = new object[5, 5];
            for (int r = 0; r < ListUngVien.Count; r++)

            {


                for (int c = 0; c < 3; c++)
                {
                    string value = "";
                    switch (c)
                    {
                        case 0:
                            value = ListUngVien.ElementAt(r).MA_UV.ToString();
                            break;
                        case 1:
                            value = ListUngVien.ElementAt(r).HOTEN_UV.ToString();
                            break;
                        case 2:
                            value = ListUngVien.ElementAt(r).NGAYSINH_UV.ToString();
                            break;
                    }
                    arr[r, c] = value;
                }
            }
            Microsoft.Office.Interop.Excel.Range c1 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4, 1];
            Microsoft.Office.Interop.Excel.Range c2 = (Microsoft.Office.Interop.Excel.Range)oSheet.Cells[4 + ListUngVien.Count - 1, 3];

            Microsoft.Office.Interop.Excel.Range range = oSheet.get_Range(c1, c2);
            range.Value2 = arr;
        }
    }
}
