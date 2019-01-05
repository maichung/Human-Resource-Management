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
    public class PhongBanViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<PHONGBAN> _ListPhongBan;
        public ObservableCollection<PHONGBAN> ListPhongBan { get => _ListPhongBan; set { _ListPhongBan = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTenTruongPhong;
        public ObservableCollection<string> ListTenTruongPhong { get => _ListTenTruongPhong; set { _ListTenTruongPhong = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVienPhongBan;
        public ObservableCollection<NHANVIEN> ListNhanVienPhongBan { get => _ListNhanVienPhongBan; set { _ListNhanVienPhongBan = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _TenPhongBan;
        public string TenPhongBan { get => _TenPhongBan; set { _TenPhongBan = value; OnPropertyChanged(); } }
        private NHANVIEN _SelectedTruongPhong;
        public NHANVIEN SelectedTruongPhong { get => _SelectedTruongPhong; set { _SelectedTruongPhong = value; OnPropertyChanged(); } }
        private DateTime? _NgayThanhLap;
        public DateTime? NgayThanhLap { get => _NgayThanhLap; set { _NgayThanhLap = value; OnPropertyChanged(); } }
        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }
        private PHONGBAN _SelectedPhongBan;
        public PHONGBAN SelectedPhongBan { get => _SelectedPhongBan; set { _SelectedPhongBan = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchPhongBan;
        public string SearchPhongBan { get => _SearchPhongBan; set { _SearchPhongBan = value; OnPropertyChanged(); } }
        public bool sort;
        #endregion

        #region Command binding
        public ICommand TaoMoiCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        #endregion

        public PhongBanViewModel()
        {
            #region Khởi tạo
            LoadListPhongBan();

            IsEditable = false;
            #endregion

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
                ResetControls();

                PhongBanWindow phongBanWindow = new PhongBanWindow();
                phongBanWindow.ShowDialog();
            });
            #endregion

            #region Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedPhongBan == null ? false : true;
            }, (p) =>
            {
                ListNhanVienPhongBan = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(nv => nv.MA_PB == SelectedPhongBan.MA_PB));

                IsEditable = false;

                TenPhongBan = SelectedPhongBan.TEN_PB;
                SelectedTruongPhong = DataProvider.Ins.model.NHANVIEN.Where(nv => nv.MA_NV == SelectedPhongBan.MATRUONGPHONG_PB).SingleOrDefault();
                NgayThanhLap = SelectedPhongBan.NGAYTHANHLAP_PB;
                DiaChi = SelectedPhongBan.DIACHI_PB;

                PhongBanWindow phongBanWindow = new PhongBanWindow();
                phongBanWindow.ShowDialog();
            });
            #endregion

            #region Sửa command
            SuaCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (SelectedPhongBan == null)
                {
                    MessageBoxResult result = MessageBox.Show("Vui lòng chọn phòng ban trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                IsEditable = true;
            });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    p.Close();
                }

            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListPhongBan);
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

            #region Search command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchPhongBan))
                {
                    CollectionViewSource.GetDefaultView(ListPhongBan).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListPhongBan).Filter = (searchPhongBan) =>
                    {
                        return (searchPhongBan as PHONGBAN).TEN_PB.IndexOf(SearchPhongBan, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchPhongBan as PHONGBAN).DIACHI_PB.IndexOf(SearchPhongBan, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(TenPhongBan) || string.IsNullOrEmpty(DiaChi) || NgayThanhLap == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin phòng ban! Mã trưởng phòng có thể để trống.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (IsEditable == false)
                {
                    MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                if (SelectedPhongBan == null)
                {
                    var PhongBanMoi = new PHONGBAN()
                    {
                        TEN_PB = TenPhongBan,
                        NGAYTHANHLAP_PB = NgayThanhLap,
                        DIACHI_PB = DiaChi
                    };
                    DataProvider.Ins.model.PHONGBAN.Add(PhongBanMoi);
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm phòng ban thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ListNhanVienPhongBan = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(nv => nv.MA_PB == SelectedPhongBan.MA_PB));
                    var PhongBanSua = DataProvider.Ins.model.PHONGBAN.Where(x => x.MA_PB == SelectedPhongBan.MA_PB).SingleOrDefault();
                    PhongBanSua.TEN_PB = TenPhongBan;
                    PhongBanSua.MA_PB = SelectedPhongBan.MA_PB;

                    if (SelectedPhongBan.MATRUONGPHONG_PB != null)
                        PhongBanSua.MATRUONGPHONG_PB = SelectedTruongPhong.MA_NV;

                    PhongBanSua.NGAYTHANHLAP_PB = NgayThanhLap;
                    PhongBanSua.DIACHI_PB = DiaChi;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadListPhongBan();
                p.Close();
            });
            #endregion

            #region Xóa command
            XoaCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedPhongBan == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng ban trước khi xoá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                MessageBoxResult result = MessageBox.Show("Thao tác này không thể hoàn tác! Bạn có chắc chắn xoá phòng ban này không?", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }, (p) =>
            {
                using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                {
                    try
                    {
                        var pb = DataProvider.Ins.model.PHONGBAN.Where(x => x.MA_PB == SelectedPhongBan.MA_PB).FirstOrDefault();
                        DataProvider.Ins.model.PHONGBAN.Remove(pb);
                        DataProvider.Ins.model.SaveChanges();
                        transactions.Commit();
                        MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        p.Close();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        transactions.Rollback();
                    }
                    LoadListPhongBan();
                }
            });
            #endregion
        }

        #region Các hàm hỗ trợ
        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
            ListTenTruongPhong = new ObservableCollection<string>();
            var listTenTruongPhong = from pb in DataProvider.Ins.model.PHONGBAN
                                     join nv in DataProvider.Ins.model.NHANVIEN
                                     on pb.MATRUONGPHONG_PB equals nv.MA_NV
                                     select nv.HOTEN_NV;
            foreach (string s in listTenTruongPhong)
            {
                ListTenTruongPhong.Add(s);
            }
        }

        public void ResetControls()
        {
            SelectedPhongBan = null;
            TenPhongBan = null;
            SelectedTruongPhong = null;
            NgayThanhLap = null;
            DiaChi = null;
            ListNhanVienPhongBan = null;
        }
        #endregion
    }
}
