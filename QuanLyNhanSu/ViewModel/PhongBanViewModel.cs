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
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVienPhongBan;
        public ObservableCollection<NHANVIEN> ListNhanVienPhongBan { get => _ListNhanVienPhongBan; set { _ListNhanVienPhongBan = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _TenPhongBan;
        public string TenPhongBan { get => _TenPhongBan; set { _TenPhongBan = value; OnPropertyChanged(); } }
        private Nullable<Int32> _SelectedTruongPhong;
        public Nullable<Int32> SelectedTruongPhong { get => _SelectedTruongPhong; set { _SelectedTruongPhong = value; OnPropertyChanged(); } }
        private string _SelectedGioiTinh;
        public string SelectedGioiTinh { get => _SelectedGioiTinh; set { _SelectedGioiTinh = value; OnPropertyChanged(); } }
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
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        #endregion

        public PhongBanViewModel()
        {
            LoadListPhongBan();
            ListNhanVienPhongBan = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN);
            IsEditable = false;

            // Tạo mới command
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

            // Lưu command
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
                        MATRUONGPHONG_PB = SelectedTruongPhong,
                        NGAYTHANHLAP_PB = NgayThanhLap,
                        DIACHI_PB = DiaChi
                    };
                    DataProvider.Ins.model.PHONGBAN.Add(PhongBanMoi);
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm phòng ban thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var PhongBanSua = DataProvider.Ins.model.PHONGBAN.Where(x => x.MA_PB == SelectedPhongBan.MA_PB).SingleOrDefault();
                    PhongBanSua.TEN_PB = TenPhongBan;
                    PhongBanSua.MA_PB = SelectedPhongBan.MA_PB;
                    PhongBanSua.MATRUONGPHONG_PB = SelectedTruongPhong;
                    PhongBanSua.NGAYTHANHLAP_PB = NgayThanhLap;
                    PhongBanSua.DIACHI_PB = DiaChi;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadListPhongBan();
                p.Close();
            });

            // Huỷ command
            HuyCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn chắc chứ?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    p.Close();
                }

            });

            // Sửa command
            SuaCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
            });

            // Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedPhongBan == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;

                TenPhongBan = SelectedPhongBan.TEN_PB;
                //SelectedTruongPhong = SelectedPhongBan.MATRUONGPHONG_PB;
                NgayThanhLap = SelectedPhongBan.NGAYTHANHLAP_PB;
                DiaChi = SelectedPhongBan.DIACHI_PB;

                PhongBanWindow phongBanWindow = new PhongBanWindow();
                phongBanWindow.ShowDialog();
            });

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
                        return (searchPhongBan as PHONGBAN).TEN_PB.IndexOf(SearchPhongBan, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
        }

        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
        }

        public void ResetControls()
        {
            SelectedPhongBan = null;
            TenPhongBan = null;
            SelectedTruongPhong = null;
            NgayThanhLap = null;
            DiaChi = null;
        }
    }
}
