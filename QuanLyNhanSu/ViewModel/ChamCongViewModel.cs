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
        //#region DataContext
        //private ObservableCollection<CHAMCONGNGAY> _ListChamCong;
        //public ObservableCollection<CHAMCONGNGAY> ListChamCong { get => _ListChamCong; set { _ListChamCong = value; OnPropertyChanged(); } }
        //#endregion

        //#region Combobox item source
        //private ObservableCollection<NHANVIEN> _ListNhanVien;
        //public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }
        //#endregion

        //#region Thuộc tính binding
        //private NHANVIEN _SelectedNhanVien;
        //public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }

        
        //#endregion

        //#region Binding Command
        //public ICommand TaoMoiCommand { get; set; }
        //public ICommand HienThiCommand { get; set; }
        //public ICommand HuyCommand { get; set; }
        //public ICommand SuaCommand { get; set; }
        //public ICommand SortCommand { get; set; }
        //public ICommand SearchCommand { get; set; }
        //public ICommand LuuCommand { get; set; }
        //public ICommand XoaCommand { get; set; }
        //#endregion

        //#region Thuộc tính khác
        //private string _SearchChamCong;
        //public string SearchChamCong { get => _SearchChamCong; set { _SearchChamCong = value; OnPropertyChanged(); } }

        //public bool sort;
        //#endregion

        public ChamCongViewModel()
        {
        //    LoadListNghiPhep();
        //    LoadListNhanVien();

        //    #region Xử lý ẩn hiện tab
        //    TabNgayNghiPhepCommand = new RelayCommand<Object>((p) =>
        //    {
        //        return true;
        //    }, (p) =>
        //    {
        //        ChucNangNP = (int)ChucNangNghiPhep.NgayNghiPhep;
        //    });

        //    TabKhoanNghiPhepCommand = new RelayCommand<Object>((p) =>
        //    {
        //        return true;
        //    }, (p) =>
        //    {
        //        ChucNangNP = (int)ChucNangNghiPhep.KhoanNghiPhep;
        //    });

        //    TabLoaiNghiPhepCommand = new RelayCommand<Object>((p) =>
        //    {
        //        return true;
        //    }, (p) =>
        //    {
        //        ChucNangNP = (int)ChucNangNghiPhep.LoaiNghiPhep;
        //    });
        //    #endregion

        //    #region Hiển thị khoản nghỉ phép command
        //    HienThiKhoanNghiPhepCommand = new RelayCommand<Object>((p) =>
        //    {
        //        return SelectedNhanVien == null ? false : true;
        //    }, (p) =>
        //    {
        //        LoadListTTKhoanNghiPhep(SelectedNhanVien.MA_NV);
        //    });
        //    #endregion

        //    #region Tạo mới command
        //    TaoMoiCommand = new RelayCommand<Object>((p) =>
        //    {
        //        if (ListNhanVien == null)
        //        {
        //            MessageBox.Show("Vui lòng thiết lập khoản nghỉ phép cho nhân viên trước khi tạo nghỉ phép mới.", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        //            return false;
        //        }
        //        return true;
        //    }, (p) =>
        //    {
        //        IsEditable = true;
        //        IsNVChangeable = true;
        //        ResetControls();

        //        NghiPhepWindow nghiphepWindow = new NghiPhepWindow();
        //        nghiphepWindow.ShowDialog();
        //    });
        //    #endregion

        //    #region Hiển thị command
        //    HienThiCommand = new RelayCommand<Object>((p) =>
        //    {
        //        return SelectedNghiPhep == null ? false : true;
        //    }, (p) =>
        //    {
        //        IsEditable = false;
        //        IsNVChangeable = false;

        //        HienThiNghiPhep();

        //        NghiPhepWindow nghiphepWindow = new NghiPhepWindow();
        //        nghiphepWindow.ShowDialog();
        //    });
        //    #endregion

        //    #region Huỷ command
        //    HuyCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
        //    {
        //        MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
        //        if (result == MessageBoxResult.OK)
        //        {
        //            IsEditable = false;
        //            IsNVChangeable = false;
        //            p.Close();
        //        }
        //    });
        //    #endregion

        //    #region Sửa command
        //    SuaCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
        //    {
        //        IsEditable = true;
        //        if (SelectedNghiPhep == null)
        //        {
        //            IsNVChangeable = true;
        //        }
        //        else
        //        {
        //            IsNVChangeable = false;
        //        }

        //    });
        //    #endregion

        //    #region Sort command
        //    SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
        //    {
        //        CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListNghiPhep);
        //        if (sort)
        //        {
        //            view.SortDescriptions.Clear();
        //            view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Ascending));
        //        }
        //        else
        //        {
        //            view.SortDescriptions.Clear();
        //            view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Descending));
        //        }
        //        sort = !sort;
        //    });
        //    #endregion

        //    #region Search Command
        //    SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
        //    {
        //        if (string.IsNullOrEmpty(SearchNghiPhep))
        //        {
        //            CollectionViewSource.GetDefaultView(ListNghiPhep).Filter = (all) => { return true; };
        //        }
        //        else
        //        {
        //            CollectionViewSource.GetDefaultView(ListNghiPhep).Filter = (searchNghiPhep) =>
        //            {
        //                return (searchNghiPhep as NGHIPHEP).MA_NV.ToString().IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
        //                       (searchNghiPhep as NGHIPHEP).NGAYBATDAU_NP.ToString().IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
        //                       (searchNghiPhep as NGHIPHEP).NGAYKETTHUC_NP.ToString().IndexOf(SearchNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0;
        //            };
        //        }

        //    });
        //    #endregion

        //    #region Lưu command
        //    LuuCommand = new RelayCommand<Window>((p) =>
        //    {
        //        if (SelectedNhanVien == null || SelectedTTKhoanNghiPhep == null || NgayBatDau == null || NgayKetThuc == null || LiDo == null)
        //        {
        //            MessageBox.Show("Vui lòng nhập đầy đủ thông tin nghỉ phép!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return false;
        //        }
        //        if (IsEditable == false)
        //        {
        //            MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return false;
        //        }
        //        if (NgayBatDau > NgayKetThuc)
        //        {
        //            MessageBox.Show("Ngày kết thúc phải sau ngày bắt đầu, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return false;
        //        }

        //        TongNgayNghi = (NgayKetThuc.Value - NgayBatDau.Value).TotalDays + 1;

        //        if (TongNgayNghi > SelectedTTKhoanNghiPhep.SoNgayNghi)
        //        {
        //            MessageBox.Show("Số ngày nghỉ còn lại của loại nghỉ phép đã chọn không đủ, vui lòng chọn lại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            return false;
        //        }

        //        return true;
        //    }, (p) =>
        //    {
        //        if (SelectedNghiPhep == null)
        //        {
        //            var tempKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_NV == SelectedNhanVien.MA_NV && x.MA_LNP == SelectedTTKhoanNghiPhep.LoaiNghiPhep.MA_LNP).SingleOrDefault();
        //            var nghiPhepMoi = new NGHIPHEP()
        //            {
        //                MA_NV = SelectedNhanVien.MA_NV,
        //                MA_KNP = tempKNP.MA_KNP,
        //                NGAYBATDAU_NP = NgayBatDau,
        //                NGAYKETTHUC_NP = NgayKetThuc,
        //                LIDO_NP = LiDo
        //            };

        //            DataProvider.Ins.model.NGHIPHEP.Add(nghiPhepMoi);

        //            var updatedKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == nghiPhepMoi.MA_KNP).SingleOrDefault();
        //            updatedKNP.SONGAYNGHI_KNP -= (int)TongNgayNghi;

        //            DataProvider.Ins.model.SaveChanges();
        //            MessageBox.Show("Thêm nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //        else
        //        {
        //            var nghiPhepSua = DataProvider.Ins.model.NGHIPHEP.Where(x => x.MA_NP == SelectedNghiPhep.MA_NP).SingleOrDefault();

        //            // Khôi phục lại số ngày nghỉ của khoản nghỉ phép đã lưu
        //            var oldKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == nghiPhepSua.MA_KNP).SingleOrDefault();
        //            double oldDays = (nghiPhepSua.NGAYKETTHUC_NP.Value - nghiPhepSua.NGAYBATDAU_NP.Value).TotalDays + 1;
        //            oldKNP.SONGAYNGHI_KNP += (int)oldDays;

        //            // Thay đổi thông tin nghỉ phép
        //            var tempKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_NV == SelectedNhanVien.MA_NV && x.MA_LNP == SelectedTTKhoanNghiPhep.LoaiNghiPhep.MA_LNP).SingleOrDefault();

        //            nghiPhepSua.MA_KNP = tempKNP.MA_KNP;
        //            nghiPhepSua.NGAYBATDAU_NP = NgayBatDau;
        //            nghiPhepSua.NGAYKETTHUC_NP = NgayKetThuc;
        //            nghiPhepSua.LIDO_NP = LiDo;

        //            var updatedKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == nghiPhepSua.MA_KNP).SingleOrDefault();
        //            updatedKNP.SONGAYNGHI_KNP -= (int)TongNgayNghi;

        //            DataProvider.Ins.model.SaveChanges();
        //            MessageBox.Show("Chỉnh sửa nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }

        //        LoadListNghiPhep();
        //        p.Close();
        //    });
        //    #endregion

        //    #region Xoá command
        //    XoaCommand = new RelayCommand<Window>((p) =>
        //    {

        //        if (SelectedNghiPhep == null)
        //        {
        //            return false;
        //        }
        //        return true;
        //    }, (p) =>
        //    {
        //        MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Question);
        //        if (result == MessageBoxResult.Yes)
        //        {
        //            using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    var np = DataProvider.Ins.model.NGHIPHEP.Where(x => x.MA_NP == SelectedNghiPhep.MA_NP).FirstOrDefault();
        //                    DataProvider.Ins.model.NGHIPHEP.Remove(np);

        //                    // Khôi phục lại số ngày nghỉ của khoản nghỉ phép đã lưu                         
        //                    var oldKNP = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_KNP == np.MA_KNP).SingleOrDefault();
        //                    double oldDays = (np.NGAYKETTHUC_NP.Value - np.NGAYBATDAU_NP.Value).TotalDays + 1;
        //                    oldKNP.SONGAYNGHI_KNP += (int)oldDays;

        //                    DataProvider.Ins.model.SaveChanges();
        //                    transactions.Commit();
        //                    MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        //                    ResetControls();
        //                    p.Close();

        //                }
        //                catch (Exception e)
        //                {
        //                    MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                    transactions.Rollback();
        //                }
        //                LoadListNghiPhep();

        //            }
        //        }
        //    });
        //    #endregion
        }
             
        //public void LoadListNhanVien()
        //{
        //    ListNhanVien = new ObservableCollection<NHANVIEN>();

        //    var listNV = from nv in DataProvider.Ins.model.NHANVIEN
        //                 where (from knp in DataProvider.Ins.model.KHOANNGHIPHEP
        //                        where knp.MA_NV == nv.MA_NV
        //                        select knp).FirstOrDefault() != null
        //                 select nv;

        //    foreach (NHANVIEN item in listNV)
        //    {
        //        ListNhanVien.Add(item);
        //    }
        //}

        //public void ResetControls()
        //{
        //    SelectedNhanVien = null;
        //    SelectedTTKhoanNghiPhep = null;
        //    SelectedNghiPhep = null;
        //    NgayBatDau = null;
        //    NgayKetThuc = null;
        //    LiDo = null;
        //    ListTTKhoanNghiPhep = null;
        //}
    }
}
