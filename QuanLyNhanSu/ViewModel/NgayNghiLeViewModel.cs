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
    class NgayNghiLeViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<NGAYNGHILE> _ListNgayNghiLe;
        public ObservableCollection<NGAYNGHILE> ListNgayNghiLe { get => _ListNgayNghiLe; set { _ListNgayNghiLe = value; OnPropertyChanged(); } }
        private ObservableCollection<string> _ListTenNgayNghiLe;
        public ObservableCollection<string> ListTenNgayNghiLe { get => _ListTenNgayNghiLe; set { _ListTenNgayNghiLe = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _TenNgayNghiLe;
        public string TenNgayNghiLe { get => _TenNgayNghiLe; set { _TenNgayNghiLe = value; OnPropertyChanged(); } }
        private DateTime? _Ngay;
        public DateTime? Ngay { get => _Ngay; set { _Ngay = value; OnPropertyChanged(); } }
        private NGAYNGHILE _SelectedNgayNghiLe;
        public NGAYNGHILE SelectedNgayNghiLe { get => _SelectedNgayNghiLe; set { _SelectedNgayNghiLe = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchNgayNghiLe;
        public string SearchNgayNghiLe { get => _SearchNgayNghiLe; set { _SearchNgayNghiLe = value; OnPropertyChanged(); } }
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

        public NgayNghiLeViewModel()
        {
            LoadListNgayNghiLe();

            IsEditable = false;

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
                ResetControls();

                NgayNghiLeWindow ngayNghiLeWindow = new NgayNghiLeWindow();
                ngayNghiLeWindow.ShowDialog();
            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(TenNgayNghiLe) || Ngay == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin ngày nghỉ lễ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedNgayNghiLe == null)
                {
                    var NgayNghiLeMoi = new NGAYNGHILE()
                    {
                        TEN_NNL = TenNgayNghiLe,
                        NGAY_NNL = Ngay
                    };
                    DataProvider.Ins.model.NGAYNGHILE.Add(NgayNghiLeMoi);
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm ngày nghỉ lễ thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var NgayNghiLeSua = DataProvider.Ins.model.NGAYNGHILE.Where(x => x.MA_NNL == SelectedNgayNghiLe.MA_NNL).SingleOrDefault();
                    NgayNghiLeSua.TEN_NNL = TenNgayNghiLe;
                    NgayNghiLeSua.NGAY_NNL = Ngay;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadListNgayNghiLe();
                p.Close();
            });
            #endregion

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

                if (SelectedNgayNghiLe == null)
                {
                    return false;
                }
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa ngày nghỉ lễ", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        var nnl = DataProvider.Ins.model.NGAYNGHILE.Where(x => x.MA_NNL == SelectedNgayNghiLe.MA_NNL).FirstOrDefault();
                        DataProvider.Ins.model.NGAYNGHILE.Remove(nnl);
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
                    LoadListNgayNghiLe();
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
                return SelectedNgayNghiLe == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;

                TenNgayNghiLe = SelectedNgayNghiLe.TEN_NNL;
                Ngay = SelectedNgayNghiLe.NGAY_NNL;

                NgayNghiLeWindow ngayNghiLeWindow = new NgayNghiLeWindow();
                ngayNghiLeWindow.ShowDialog();
            });

            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListNgayNghiLe);
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
                if (string.IsNullOrEmpty(SearchNgayNghiLe))
                {
                    CollectionViewSource.GetDefaultView(ListNgayNghiLe).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListNgayNghiLe).Filter = (searchNgayNghiLe) =>
                    {
                        return (searchNgayNghiLe as NGAYNGHILE).TEN_NNL.IndexOf(SearchNgayNghiLe, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
        }

        public void LoadListNgayNghiLe()
        {
            ListNgayNghiLe = new ObservableCollection<NGAYNGHILE>(DataProvider.Ins.model.NGAYNGHILE);
        }

        public void ResetControls()
        {
            SelectedNgayNghiLe = null;
            TenNgayNghiLe = null;
            Ngay = null;
        }
    }
}
