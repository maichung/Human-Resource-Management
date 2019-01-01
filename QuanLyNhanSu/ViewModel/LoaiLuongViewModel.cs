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

namespace QuanLyNhanSu.ViewModel
{
    public class LoaiLuongViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<LOAILUONG> _ListLoaiLuong;
        public ObservableCollection<LOAILUONG> ListLoaiLuong { get => _ListLoaiLuong; set { _ListLoaiLuong = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _TenLoaiLuong;
        public string TenLoaiLuong { get => _TenLoaiLuong; set { _TenLoaiLuong = value; OnPropertyChanged(); } }

        private LOAILUONG _SelectedLoaiLuong;
        public LOAILUONG SelectedLoaiLuong { get => _SelectedLoaiLuong; set { _SelectedLoaiLuong = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchLoaiLuong;
        public string SearchLoaiLuong { get => _SearchLoaiLuong; set { _SearchLoaiLuong = value; OnPropertyChanged(); } }
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

        public LoaiLuongViewModel()
        {
            LoadListLoaiLuong();

            IsEditable = false;

            // Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
                ResetControls();

                LoaiLuongWindow loaiLuongWindow = new LoaiLuongWindow();
                loaiLuongWindow.ShowDialog();
            });

            // Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(TenLoaiLuong))
                {
                    MessageBox.Show("Vui lòng nhập tên loại lương!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedLoaiLuong == null)
                {
                    var LoaiLuongMoi = new LOAILUONG()
                    {
                        TEN_LL = TenLoaiLuong
                    };
                    DataProvider.Ins.model.LOAILUONG.Add(LoaiLuongMoi);
                    DataProvider.Ins.model.SaveChanges();
                    ListLoaiLuong.Add(LoaiLuongMoi);
                    MessageBox.Show("Thêm loại lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var LoaiLuongSua = DataProvider.Ins.model.LOAILUONG.Where(x => x.MA_LL == SelectedLoaiLuong.MA_LL).SingleOrDefault();
                    LoaiLuongSua.TEN_LL = TenLoaiLuong;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
              
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

            // Xóa command
            XoaCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedLoaiLuong == null)
                {
                    return false;
                }
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa loại loại", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        var lnp = DataProvider.Ins.model.LOAILUONG.Where(x => x.MA_LL == SelectedLoaiLuong.MA_LL).FirstOrDefault();
                        DataProvider.Ins.model.LOAILUONG.Remove(lnp);
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
                    LoadListLoaiLuong();
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
                return SelectedLoaiLuong == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;

                TenLoaiLuong = SelectedLoaiLuong.TEN_LL;

                LoaiLuongWindow loaiLuongWindow = new LoaiLuongWindow();
                loaiLuongWindow.ShowDialog();
            });

            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListLoaiLuong);
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
                if (string.IsNullOrEmpty(SearchLoaiLuong))
                {
                    CollectionViewSource.GetDefaultView(ListLoaiLuong).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListLoaiLuong).Filter = (searchLoaiLuong) =>
                    {
                        return (searchLoaiLuong as LOAILUONG).TEN_LL.IndexOf(SearchLoaiLuong, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
        }

        public void LoadListLoaiLuong()
        {
            ListLoaiLuong = new ObservableCollection<LOAILUONG>(DataProvider.Ins.model.LOAILUONG);
        }

        public void ResetControls()
        {
            SelectedLoaiLuong = null;
            TenLoaiLuong = null;
        }
    }
}
