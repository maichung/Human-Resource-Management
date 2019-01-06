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
    public class NghiPhepViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<NGHIPHEP> _ListNghiPhep;
        public ObservableCollection<NGHIPHEP> ListNghiPhep { get => _ListNghiPhep; set { _ListNghiPhep = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        private ObservableCollection<KHOANNGHIPHEP> _ListKhoanNghiPhep;
        public ObservableCollection<KHOANNGHIPHEP> ListKhoanNghiPhep { get => _ListKhoanNghiPhep; set { _ListKhoanNghiPhep = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }

        private KHOANNGHIPHEP _SelectedKhoanNghiPhep;
        public KHOANNGHIPHEP SelectedKhoanNghiPhep { get => _SelectedKhoanNghiPhep; set { _SelectedKhoanNghiPhep = value; OnPropertyChanged(); } }

        private NGHIPHEP _SelectedNghiPhep;
        public NGHIPHEP SelectedNghiPhep { get => _SelectedNghiPhep; set { _SelectedNghiPhep = value; OnPropertyChanged(); } }

        private DateTime? _NgayBatDau;
        public DateTime? NgayBatDau { get => _NgayBatDau; set { _NgayBatDau = value; OnPropertyChanged(); } }

        private DateTime? _NgayKetThuc;
        public DateTime? NgayKetThuc { get => _NgayKetThuc; set { _NgayKetThuc = value; OnPropertyChanged(); } }

        private string _LiDo;
        public string LiDo { get => _LiDo; set { _LiDo = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        private bool _IsNVChangeable;
        public bool IsNVChangeable { get => _IsNVChangeable; set { _IsNVChangeable = value; OnPropertyChanged(); } }
        #endregion

        #region Binding Command
        public ICommand LoadDataNPCommand { get; set; }
        public ICommand HienThiKhoanNghiPhepCommand { get; set; }
        public ICommand TaoMoiCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        #endregion

        #region Thuộc tính ẩn hiện tab
        public enum ChucNangNghiPhep
        {
            NgayNghiPhep, KhoanNghiPhep, LoaiNghiPhep
        };

        private int _ChucNangNP;
        public int ChucNangNP { get => _ChucNangNP; set { _ChucNangNP = value; OnPropertyChanged(); } }

        public ICommand TabNgayNghiPhepCommand { get; set; }
        public ICommand TabKhoanNghiPhepCommand { get; set; }
        public ICommand TabLoaiNghiPhepCommand { get; set; }
        #endregion

        #region Thuộc tính khác
        private double _TongNgayNghi;
        public double TongNgayNghi { get => _TongNgayNghi; set { _TongNgayNghi = value; OnPropertyChanged(); } }

        private string _SearchNghiPhep;
        public string SearchNghiPhep { get => _SearchNghiPhep; set { _SearchNghiPhep = value; OnPropertyChanged(); } }

        public bool sort;
        #endregion

        public NghiPhepViewModel()
        {
            #region Khởi tạo
            LoadListNhanVien();
            #endregion

            #region Xử lý ẩn hiện tab
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

            #region Load dữ liệu nghỉ phép command
            LoadDataNPCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                LoadListNghiPhep();
            });
            #endregion

            #region Hiển thị khoản nghỉ phép command
            HienThiKhoanNghiPhepCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedNhanVien == null ? false : true;
            }, (p) =>
            {
                LoadListKhoanNghiPhep(SelectedNhanVien.MA_NV);
            });
            #endregion

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                if (ListNhanVien == null)
                {
                    MessageBox.Show("Vui lòng thiết lập khoản nghỉ phép cho nhân viên trước khi tạo nghỉ phép mới.", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                IsEditable = true;
                IsNVChangeable = true;
                ResetControls();
                LoadListNhanVien();

                NghiPhepWindow nghiphepWindow = new NghiPhepWindow();
                nghiphepWindow.ShowDialog();
            });
            #endregion

            #region Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedNghiPhep == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;
                IsNVChangeable = false;

                HienThiNghiPhep();

                NghiPhepWindow nghiphepWindow = new NghiPhepWindow();
                nghiphepWindow.ShowDialog();
            });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
              {
                  MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                  if (result == MessageBoxResult.OK)
                  {
                      IsEditable = false;
                      IsNVChangeable = false;
                      p.Close();
                  }
              });
            #endregion

            #region Sửa command
            SuaCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {                
                if (SelectedNghiPhep == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày nghỉ phép trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsNVChangeable = true;
                }
                else
                {
                    IsNVChangeable = false;
                }
                IsEditable = true;
            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListNghiPhep);
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
                if (string.IsNullOrEmpty(SearchNghiPhep))
                {
                    CollectionViewSource.GetDefaultView(ListNghiPhep).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListNghiPhep).Filter = (searchNghiPhep) =>
                    {
                        return (searchNghiPhep as NGHIPHEP).NHANVIEN.HOTEN_NV.IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchNghiPhep as NGHIPHEP).NGAYBATDAU_NP.ToString().IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchNghiPhep as NGHIPHEP).NGAYKETTHUC_NP.ToString().IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedNhanVien == null || SelectedKhoanNghiPhep == null || NgayBatDau == null || NgayKetThuc == null || LiDo == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin nghỉ phép!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (IsEditable == false)
                {
                    MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (NgayBatDau > NgayKetThuc)
                {
                    MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                if (SelectedNghiPhep == null)
                {
                    // Kiểm tra ngày còn lại của khoản nghỉ phép
                    TongNgayNghi = (NgayKetThuc.Value - NgayBatDau.Value).TotalDays + 1;

                    if (TongNgayNghi > SelectedKhoanNghiPhep.SONGAYNGHI_KNP)
                    {
                        MessageBox.Show("Số ngày nghỉ còn lại của loại nghỉ phép đã chọn không đủ, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var nghiPhepMoi = new NGHIPHEP()
                    {
                        MA_NV = SelectedNhanVien.MA_NV,
                        MA_KNP = SelectedKhoanNghiPhep.MA_KNP,
                        NGAYBATDAU_NP = NgayBatDau,
                        NGAYKETTHUC_NP = NgayKetThuc,
                        LIDO_NP = LiDo
                    };

                    DataProvider.Ins.model.NGHIPHEP.Add(nghiPhepMoi);

                    SelectedKhoanNghiPhep.SONGAYNGHI_KNP -= (int)TongNgayNghi;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    ListNghiPhep.Add(nghiPhepMoi);
                }
                else
                {
                    var nghiPhepSua = DataProvider.Ins.model.NGHIPHEP.Where(x => x.MA_NP == SelectedNghiPhep.MA_NP).SingleOrDefault();


                    // Khôi phục lại số ngày nghỉ của khoản nghỉ phép đã lưu
                    var oldKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == nghiPhepSua.MA_KNP).SingleOrDefault();
                    double oldDays = (nghiPhepSua.NGAYKETTHUC_NP.Value - nghiPhepSua.NGAYBATDAU_NP.Value).TotalDays + 1;
                    oldKNP.SONGAYNGHI_KNP += (int)oldDays;

                    // Kiểm tra ngày còn lại của khoản nghỉ phép
                    DateTime a = new DateTime(2018, 12, 10);
                    DateTime b = DateTime.Now;
                    TongNgayNghi = (NgayKetThuc.Value - NgayBatDau.Value).TotalDays + 1;

                    if (TongNgayNghi > SelectedKhoanNghiPhep.SONGAYNGHI_KNP)
                    {
                        MessageBox.Show("Số ngày nghỉ còn lại của loại nghỉ phép đã chọn không đủ, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Cập nhật thông tin mới
                    nghiPhepSua.MA_KNP = SelectedKhoanNghiPhep.MA_KNP;
                    nghiPhepSua.NGAYBATDAU_NP = NgayBatDau;
                    nghiPhepSua.NGAYKETTHUC_NP = NgayKetThuc;
                    nghiPhepSua.LIDO_NP = LiDo;

                    var updatedKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == nghiPhepSua.MA_KNP).SingleOrDefault();
                    updatedKNP.SONGAYNGHI_KNP -= (int)TongNgayNghi;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Chỉnh sửa nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }                
                p.Close();
            });
            #endregion

            #region Xoá command
            XoaCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedNghiPhep == null)
                {
                    MessageBox.Show("Vui lòng chọn ngày nghỉ phép trước khi xoá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Thao tác này không thể hoàn tác! Bạn có chắc chắn xoá ngày nghỉ phép này không? ", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                    {
                        try
                        {
                            var np = DataProvider.Ins.model.NGHIPHEP.Where(x => x.MA_NP == SelectedNghiPhep.MA_NP).FirstOrDefault();
                            DataProvider.Ins.model.NGHIPHEP.Remove(np);

                            // Khôi phục lại số ngày nghỉ của khoản nghỉ phép đã lưu                         
                            var oldKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == np.MA_KNP).SingleOrDefault();
                            double oldDays = (np.NGAYKETTHUC_NP.Value - np.NGAYBATDAU_NP.Value).TotalDays + 1;
                            oldKNP.SONGAYNGHI_KNP += (int)oldDays;

                            DataProvider.Ins.model.SaveChanges();
                            transactions.Commit();

                            ResetControls();
                            p.Close();
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            transactions.Rollback();
                        }
                        LoadListNghiPhep();
                    }
                }
            });
            #endregion
        }

        #region Các hàm hỗ trợ
        public void HienThiNghiPhep()
        {
            SelectedNhanVien = DataProvider.Ins.model.NHANVIEN.Where(x => x.MA_NV == SelectedNghiPhep.MA_NV && x.TRANGTHAI_NV == true).SingleOrDefault();
            LoadListKhoanNghiPhep(SelectedNghiPhep.MA_NV);
            SelectedKhoanNghiPhep = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == SelectedNghiPhep.MA_KNP).SingleOrDefault();
            NgayBatDau = SelectedNghiPhep.NGAYBATDAU_NP;
            NgayKetThuc = SelectedNghiPhep.NGAYKETTHUC_NP;
            LiDo = SelectedNghiPhep.LIDO_NP;
        }

        public void LoadListNghiPhep()
        {
            if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                ListNghiPhep = new ObservableCollection<NGHIPHEP>(DataProvider.Ins.model.NGHIPHEP.Where(x => x.NHANVIEN.MA_PB == MainViewModel.TaiKhoan.NHANVIEN.MA_PB && x.NHANVIEN.TRANGTHAI_NV == true));
            }
            else
            {
                ListNghiPhep = new ObservableCollection<NGHIPHEP>(DataProvider.Ins.model.NGHIPHEP.Where(x => x.NHANVIEN.TRANGTHAI_NV == true));
            }
        }

        public void LoadListKhoanNghiPhep(int maNV)
        {
            ListKhoanNghiPhep = new ObservableCollection<KHOANNGHIPHEP>();
            var listKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_NV == maNV);
            foreach (KHOANNGHIPHEP item in listKNP)
            {
                ListKhoanNghiPhep.Add(item);
            }
        }

        public void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>();

            var listNV = from nv in DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true)
                         where (from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                where knp.MA_NV == nv.MA_NV
                                select knp).FirstOrDefault() != null
                         select nv;

            if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                foreach (NHANVIEN item in listNV)
                {
                    if (MainViewModel.TaiKhoan.NHANVIEN.MA_PB == item.MA_PB)
                    {
                        ListNhanVien.Add(item);
                    }
                }
                return;
            }

            foreach (NHANVIEN item in listNV)
            {
                ListNhanVien.Add(item);
            }
        }

        public void ResetControls()
        {
            SelectedNhanVien = null;
            SelectedKhoanNghiPhep = null;
            SelectedNghiPhep = null;
            NgayBatDau = null;
            NgayKetThuc = null;
            LiDo = null;
            ListKhoanNghiPhep = null;
        }
        #endregion
    }
}
