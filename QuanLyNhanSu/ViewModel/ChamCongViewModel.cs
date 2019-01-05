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
    public class ChamCongViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinChamCong> _ListTTChamCong_ALLNV;
        public ObservableCollection<ThongTinChamCong> ListTTChamCong_ALLNV { get => _ListTTChamCong_ALLNV; set { _ListTTChamCong_ALLNV = value; OnPropertyChanged(); } }
        private ObservableCollection<CHAMCONGNGAY> _ListChamCong_1NV;
        public ObservableCollection<CHAMCONGNGAY> ListChamCong_1NV { get => _ListChamCong_1NV; set { _ListChamCong_1NV = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinChamCong> _ListTTChamCong_1NV;
        public ObservableCollection<ThongTinChamCong> ListTTChamCong_1NV { get => _ListTTChamCong_1NV; set { _ListTTChamCong_1NV = value; OnPropertyChanged(); } }
        #endregion

        #region Cbx source
        private ObservableCollection<int> _ListNam;
        public ObservableCollection<int> ListNam { get => _ListNam; set { _ListNam = value; OnPropertyChanged(); } }
        private int _SelectedNam;
        public int SelectedNam { get => _SelectedNam; set { _SelectedNam = value; OnPropertyChanged(); } }
        private ObservableCollection<int> _ListThang;
        public ObservableCollection<int> ListThang { get => _ListThang; set { _ListThang = value; OnPropertyChanged(); } }
        private int _SelectedThang;
        public int SelectedThang { get => _SelectedThang; set { _SelectedThang = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        private DateTime? _NgayChamCong;
        public DateTime? NgayChamCong { get => _NgayChamCong; set { _NgayChamCong = value; OnPropertyChanged(); } }
        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }
        private ImageSource _AvatarSource;
        public ImageSource AvatarSource { get => _AvatarSource; set { _AvatarSource = value; OnPropertyChanged(); } }
        #endregion

        #region Binding Command
        public ICommand TaoMoiCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand ChangeCmbCommand { get; set; }
        public ICommand DoiNgayChamCongCommand { get; set; }        
        #endregion

        #region Thuộc tính khác
        private string _SearchNhanVien;
        public string SearchNhanVien { get => _SearchNhanVien; set { _SearchNhanVien = value; OnPropertyChanged(); } }
        public bool sort;
        private ObservableCollection<ThongTinChamCong> backupListTTChamCong_1NV;
        #endregion

        public ChamCongViewModel()
        {
            if (DataProvider.Ins.model.LOAICHAMCONG.Count() == 0)
            {
                CreateLoaiChamCong();
            }                
            LoadListNhanVien();
            NgayChamCong = DateTime.Now;
            int[] DSNam = new int[] { 2019, 2020, 2021, 2022, 2023 };
            ListNam = new ObservableCollection<int>(DSNam);
            int[] DSThang = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            ListThang = new ObservableCollection<int>(DSThang);

            #region ChangeCmb command
            ChangeCmbCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                LoadListTTChamCong_1NV();
            });
            #endregion

            #region Đổi ngày chấm công command
            DoiNgayChamCongCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if(NgayChamCong.Value < DateTime.Now.Date)
                {
                    IsEditable = false;
                }
                else
                {
                    IsEditable = true;
                }
            });
            #endregion

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ListTTChamCong_1NV = null;
                SelectedNhanVien = null;
                IsEditable = true;
                LoadListTTChamCong_ALLNV();

                ChamCongWindow chamcongWindow = new ChamCongWindow();
                chamcongWindow.ShowDialog();
            });
            #endregion

            #region Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedNhanVien == null ? false : true;
            }, (p) =>
            {
                ListTTChamCong_ALLNV = null;
                IsEditable = false;
                AvatarSource = NhanVienViewModel.GetImage(SelectedNhanVien.AVATAR_NV);                
                SelectedNam = DateTime.Now.Year;
                SelectedThang = DateTime.Now.Month;
                LoadListTTChamCong_1NV();

                ChiTietChamCongWindow ctccWindow = new ChiTietChamCongWindow();
                ctccWindow.ShowDialog();
            });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    p.Close();
                }
            });
            #endregion

            #region Sửa command
            SuaCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListNhanVien);
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

            #region Search Command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchNhanVien))
                {
                    CollectionViewSource.GetDefaultView(ListNhanVien).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListNhanVien).Filter = (searchNhanVien) =>
                    {
                        return (searchNhanVien as NHANVIEN).HOTEN_NV.ToString().IndexOf(SearchNhanVien, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchNhanVien as NHANVIEN).CHUCVU_NV.ToString().IndexOf(SearchNhanVien, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }
            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (IsEditable == false)
                {
                    MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                if(ListTTChamCong_ALLNV != null)
                {
                    foreach (ThongTinChamCong item in ListTTChamCong_ALLNV)
                    {
                        if (item.TangCa == true)
                        {
                            if (item.GioBatDau == null || item.GioKetThuc == null)
                            {
                                MessageBox.Show("Vui lòng chọn giờ bắt đầu và giờ kết thúc cho những nhân viên tăng ca!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return false;
                            }
                            if (item.GioBatDau > item.GioKetThuc)
                            {
                                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return false;
                            }
                        }
                    }
                }

                if (ListTTChamCong_1NV != null && SelectedNhanVien != null)
                {
                    foreach (ThongTinChamCong item in ListTTChamCong_1NV)
                    {
                        if (item.TangCa == true)
                        {
                            if (item.GioBatDau == null || item.GioKetThuc == null)
                            {
                                MessageBox.Show("Vui lòng chọn giờ bắt đầu và giờ kết thúc cho những nhân viên tăng ca!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return false;
                            }
                            if (item.GioBatDau.Value.TimeOfDay > item.GioKetThuc.Value.TimeOfDay)
                            {
                                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                return false;
                            }
                        }
                    }
                }

                return true;
            }, (p) =>
            {
                if (SelectedNhanVien == null)
                {
                    foreach(ThongTinChamCong item in ListTTChamCong_ALLNV)
                    {
                        if(item.HanhChinh == true)
                        {
                            var chamCongNgayMoi = new CHAMCONGNGAY()
                            {
                                MA_NV = item.NhanVien.MA_NV,
                                MA_LCC = 1,
                                THOIGIANBATDAU_CCN = new DateTime(NgayChamCong.Value.Year, NgayChamCong.Value.Month, NgayChamCong.Value.Day, 8, 0, 0),
                                THOIGIANKETTHUC_CCN = new DateTime(NgayChamCong.Value.Year, NgayChamCong.Value.Month, NgayChamCong.Value.Day, 17, 0, 0),
                            };
                            DataProvider.Ins.model.CHAMCONGNGAY.Add(chamCongNgayMoi);
                            DataProvider.Ins.model.SaveChanges();
                        }
                        if (item.TangCa == true)
                        {
                            var chamCongNgayMoi = new CHAMCONGNGAY()
                            {
                                MA_NV = item.NhanVien.MA_NV,
                                MA_LCC = 2,
                                THOIGIANBATDAU_CCN = new DateTime(NgayChamCong.Value.Year, NgayChamCong.Value.Month, NgayChamCong.Value.Day, item.GioBatDau.Value.Hour, item.GioBatDau.Value.Minute, item.GioBatDau.Value.Second),
                                THOIGIANKETTHUC_CCN = new DateTime(NgayChamCong.Value.Year, NgayChamCong.Value.Month, NgayChamCong.Value.Day, item.GioKetThuc.Value.Hour, item.GioKetThuc.Value.Minute, item.GioKetThuc.Value.Second),
                            };
                            DataProvider.Ins.model.CHAMCONGNGAY.Add(chamCongNgayMoi);
                            DataProvider.Ins.model.SaveChanges();
                        }
                    }

                    LoadListNhanVien();
                    MessageBox.Show("Chấm công ngày thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    //Duyệt danh sách tìm ra object nào trong danh sách có sự thay đổi
                    foreach(ThongTinChamCong item in ListTTChamCong_1NV)
                    {
                        foreach (ThongTinChamCong temp in backupListTTChamCong_1NV)
                        {
                            if(item.NgayChamCong.Value == temp.NgayChamCong.Value)
                            {
                                //TH1: thay đổi chấm công hành chính
                                if (item.HanhChinh != temp.HanhChinh)
                                {
                                    var ccn = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_NV == SelectedNhanVien.MA_NV && 
                                                                                             x.MA_LCC == 1 && 
                                                                                             x.THOIGIANKETTHUC_CCN.Value == item.NgayChamCong.Value).SingleOrDefault();
                                    //Xóa
                                    if(item.HanhChinh == false)
                                    {
                                        using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                DataProvider.Ins.model.CHAMCONGNGAY.Remove(ccn);
                                                DataProvider.Ins.model.SaveChanges();
                                                transactions.Commit();
                                            }
                                            catch (Exception e)
                                            {
                                                transactions.Rollback();
                                            }
                                        }                                        
                                    }
                                    //Thêm mới
                                    else
                                    {
                                        var chamCongNgayMoi = new CHAMCONGNGAY()
                                        {
                                            MA_NV = SelectedNhanVien.MA_NV,
                                            MA_LCC = 1,
                                            THOIGIANBATDAU_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, 8, 0, 0),
                                            THOIGIANKETTHUC_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, 17, 0, 0),
                                        };
                                        DataProvider.Ins.model.CHAMCONGNGAY.Add(chamCongNgayMoi);
                                        DataProvider.Ins.model.SaveChanges();
                                    }
                                    
                                }
                                //TH2: thay đổi chấm công tăng ca
                                if (item.TangCa != temp.TangCa)
                                {
                                    var ccn = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_NV == SelectedNhanVien.MA_NV &&
                                                                                             x.MA_LCC == 2 &&
                                                                                             x.THOIGIANKETTHUC_CCN.Value == item.GioKetThuc.Value).SingleOrDefault();
                                    //Xóa
                                    if (item.TangCa == false)
                                    {
                                        using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                                        {
                                            try
                                            {
                                                DataProvider.Ins.model.CHAMCONGNGAY.Remove(ccn);
                                                DataProvider.Ins.model.SaveChanges();
                                                transactions.Commit();
                                            }
                                            catch (Exception e)
                                            {
                                                transactions.Rollback();
                                            }
                                        }
                                    }
                                    //Thêm mới
                                    else
                                    {
                                        var chamCongNgayMoi = new CHAMCONGNGAY()
                                        {
                                            MA_NV = item.NhanVien.MA_NV,
                                            MA_LCC = 2,
                                            THOIGIANBATDAU_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, item.GioBatDau.Value.Hour, item.GioBatDau.Value.Minute, item.GioBatDau.Value.Second),
                                            THOIGIANKETTHUC_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, item.GioKetThuc.Value.Hour, item.GioKetThuc.Value.Minute, item.GioKetThuc.Value.Second),
                                        };
                                        DataProvider.Ins.model.CHAMCONGNGAY.Add(chamCongNgayMoi);
                                        DataProvider.Ins.model.SaveChanges();
                                    }
                                }
                                //Sửa
                                else
                                {
                                    if(item.TangCa == true)
                                    {
                                        if (item.GioBatDau != temp.GioBatDau)
                                        {
                                            var ccn = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_NV == SelectedNhanVien.MA_NV &&
                                                                                                 x.MA_LCC == 2 &&
                                                                                                 x.THOIGIANKETTHUC_CCN.Value == item.GioKetThuc.Value).SingleOrDefault();
                                            ccn.THOIGIANBATDAU_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, item.GioBatDau.Value.Hour, item.GioBatDau.Value.Minute, item.GioBatDau.Value.Second);
                                            DataProvider.Ins.model.SaveChanges();
                                        }
                                        if (item.GioKetThuc != temp.GioKetThuc)
                                        {
                                            var ccn = DataProvider.Ins.model.CHAMCONGNGAY.Where(x => x.MA_NV == SelectedNhanVien.MA_NV &&
                                                                                                 x.MA_LCC == 2 &&
                                                                                                 x.THOIGIANKETTHUC_CCN.Value == item.GioKetThuc.Value).SingleOrDefault();
                                            ccn.THOIGIANKETTHUC_CCN = new DateTime(item.NgayChamCong.Value.Year, item.NgayChamCong.Value.Month, item.NgayChamCong.Value.Day, item.GioKetThuc.Value.Hour, item.GioKetThuc.Value.Minute, item.GioKetThuc.Value.Second);
                                            DataProvider.Ins.model.SaveChanges();
                                        }
                                    }                                    
                                }
                                break;
                            }
                            
                        }
                    }

                    MessageBox.Show("Chỉnh sửa chấm công thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                p.Close();
            });
            #endregion
        }

        public void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>();

            var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true)
                             where (from ccn in DataProvider.Ins.model.CHAMCONGNGAY
                                    where ccn.MA_NV == nv.MA_NV
                                    select ccn).FirstOrDefault() != null
                             select nv;

            if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                foreach (NHANVIEN item in listNhanVien)
                {
                    if (MainViewModel.TaiKhoan.NHANVIEN.MA_PB == item.MA_PB)
                    {
                        ListNhanVien.Add(item);
                    }
                }
                return;
            }

            foreach (NHANVIEN item in listNhanVien)
            {
                ListNhanVien.Add(item);
            }
        }

        public void CreateLoaiChamCong()
        {
            DataProvider.Ins.model.LOAICHAMCONG.Add(new LOAICHAMCONG() { TEN_LCC = "Hành chính" });
            DataProvider.Ins.model.LOAICHAMCONG.Add(new LOAICHAMCONG() { TEN_LCC = "Tăng ca" });
            DataProvider.Ins.model.SaveChanges();
        }

        public void LoadListTTChamCong_ALLNV()
        {
            ListTTChamCong_ALLNV = new ObservableCollection<ThongTinChamCong>();

            var listTTChamCong_ALLNV = from nv in DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true)
                               select new ThongTinChamCong()
                               {
                                   NhanVien = nv,
                                   HanhChinh = false,
                                   TangCa = false
                               };

            if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                foreach (ThongTinChamCong item in listTTChamCong_ALLNV)
                {
                    if (MainViewModel.TaiKhoan.NHANVIEN.MA_PB == item.NhanVien.MA_PB)
                    {
                        ListTTChamCong_ALLNV.Add(item);
                    }
                }
                return;
            }

            foreach (ThongTinChamCong item in listTTChamCong_ALLNV)
            {
                ListTTChamCong_ALLNV.Add(item);
            }
        }

        public void LoadListTTChamCong_1NV()
        {
            ListChamCong_1NV = new ObservableCollection<CHAMCONGNGAY>();
            ListTTChamCong_1NV = new ObservableCollection<ThongTinChamCong>();
            backupListTTChamCong_1NV = new ObservableCollection<ThongTinChamCong>();

            var listChamCong_1NV = from ccn in DataProvider.Ins.model.CHAMCONGNGAY
                                   where ccn.MA_NV == SelectedNhanVien.MA_NV &&
                                         ccn.THOIGIANBATDAU_CCN.Value.Year == SelectedNam &&
                                         ccn.THOIGIANBATDAU_CCN.Value.Month == SelectedThang
                                   select ccn;
            //Tạo ngày đầu tiên trong tháng để khi lấy ngày cuối của tháng ta chỉ cần cộng 1 tháng và trừ 1 ngày
            DateTime firstDayOfMonth = new DateTime(SelectedNam, SelectedThang, 1);
            if (listChamCong_1NV.Count() < 1)
            {                
                //Duyệt từng ngày trong tháng
                for (var i = 1; i <= firstDayOfMonth.AddMonths(1).AddDays(-1).Day; i++)
                {
                    ListTTChamCong_1NV.Add(new ThongTinChamCong()
                    {
                        NhanVien = SelectedNhanVien,
                        HanhChinh = false,
                        TangCa = false,
                        NgayChamCong = new DateTime(SelectedNam, SelectedThang, i)
                    });
                }
                return;
            }

            foreach (CHAMCONGNGAY item in listChamCong_1NV)
            {
                ListChamCong_1NV.Add(item);
            }

            //Kiểm tra xem danh sách chấm công đã được thêm thông tin chấm công ngày mà nhân viên có cả 2 loại
            bool isAdd;
            //Lấy ra danh sách chấm công của nhân viên trong tháng/năm được chọn (chỉ có chấm công)
            for (var i = 0; i < ListChamCong_1NV.Count(); i++)
            {
                isAdd = false;
                for (var j = ListChamCong_1NV.Count() - 1; j > i; j--)
                {
                    if (ListChamCong_1NV[i].THOIGIANBATDAU_CCN.Value.Date == ListChamCong_1NV[j].THOIGIANBATDAU_CCN.Value.Date)
                    {
                        if(ListChamCong_1NV[i].MA_LCC == 2)
                        {
                            ListTTChamCong_1NV.Add(new ThongTinChamCong
                            {
                                NhanVien = SelectedNhanVien,
                                HanhChinh = true,
                                TangCa = true,
                                GioBatDau = ListChamCong_1NV[i].THOIGIANBATDAU_CCN.Value,
                                GioKetThuc = ListChamCong_1NV[i].THOIGIANKETTHUC_CCN.Value,
                                NgayChamCong = new DateTime(SelectedNam, SelectedThang, ListChamCong_1NV[i].THOIGIANKETTHUC_CCN.Value.Day, 17, 0, 0)
                            });
                        }
                        else if (ListChamCong_1NV[j].MA_LCC == 2)
                        {
                            ListTTChamCong_1NV.Add(new ThongTinChamCong
                            {
                                NhanVien = SelectedNhanVien,
                                HanhChinh = true,
                                TangCa = true,
                                GioBatDau = ListChamCong_1NV[j].THOIGIANBATDAU_CCN.Value,
                                GioKetThuc = ListChamCong_1NV[j].THOIGIANKETTHUC_CCN.Value,
                                NgayChamCong = new DateTime(SelectedNam, SelectedThang, ListChamCong_1NV[j].THOIGIANKETTHUC_CCN.Value.Day, 17, 0, 0)
                            });
                        }
                        isAdd = true;
                        ListChamCong_1NV.Remove(ListChamCong_1NV[j]);
                        break;
                    }
                }
                //Khi danh sách thông tin chấm công không rơi vào trường hợp nhân viên trong ngày đó có cả 2 loại chấm công
                if(isAdd == false)
                {
                    if (ListChamCong_1NV[i].MA_LCC == 1)
                    {
                        ListTTChamCong_1NV.Add(new ThongTinChamCong
                        {
                            NhanVien = SelectedNhanVien,
                            HanhChinh = true,
                            TangCa = false,
                            NgayChamCong = ListChamCong_1NV[i].THOIGIANKETTHUC_CCN.Value,
                        });
                    }
                    else if (ListChamCong_1NV[i].MA_LCC == 2)
                    {
                        ListTTChamCong_1NV.Add(new ThongTinChamCong
                        {
                            NhanVien = SelectedNhanVien,
                            HanhChinh = false,
                            TangCa = true,
                            GioBatDau = ListChamCong_1NV[i].THOIGIANBATDAU_CCN.Value,
                            GioKetThuc = ListChamCong_1NV[i].THOIGIANKETTHUC_CCN.Value,
                            NgayChamCong = ListChamCong_1NV[i].THOIGIANKETTHUC_CCN.Value
                        });
                    }
                }                
            }

            //Kiểm tra xem ngày đó nhân viên đã được chấm công hay chưa
            bool isChamCong;
            //Lấy ra danh sách chấm công của nhân viên trong tháng/năm được chọn (tất cả ngày có chấm công hoặc không)
            if (ListTTChamCong_1NV.Count() <= 15)
            {
                for (var i = 1; i <= DateTime.DaysInMonth(SelectedNam, SelectedThang); i++)
                {
                    isChamCong = false;
                    foreach (ThongTinChamCong item in ListTTChamCong_1NV)
                    {
                        if (item.NgayChamCong.Value.Day == i)
                        {
                            isChamCong = true;
                            break;
                        }
                            
                    }

                    if(isChamCong == false)
                    {
                        ListTTChamCong_1NV.Add(new ThongTinChamCong()
                        {
                            NhanVien = SelectedNhanVien,
                            HanhChinh = false,
                            TangCa = false,
                            NgayChamCong = new DateTime(SelectedNam, SelectedThang, i)
                        });
                    }
                }
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTTChamCong_1NV);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription("NgayChamCong", ListSortDirection.Ascending));
            foreach(ThongTinChamCong item in ListTTChamCong_1NV)
            {
                backupListTTChamCong_1NV.Add(new ThongTinChamCong(item));
            }
        }
    }
}
