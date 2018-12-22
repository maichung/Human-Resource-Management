using QuanLyNhanSu.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyNhanSu.ViewModel
{
    class LoaiNghiPhepViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<LOAINGHIPHEP> _ListLoaiNghiPhep;
        public ObservableCollection<LOAINGHIPHEP> ListLoaiNghiPhep { get => _ListLoaiNghiPhep; set { _ListLoaiNghiPhep = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<string> _ListCoLuong;
        public ObservableCollection<string> ListCoLuong { get => _ListCoLuong; set { _ListCoLuong = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _TenLoaiNghiPhep;
        public string TenLoaiNghiPhep { get => _TenLoaiNghiPhep; set { _TenLoaiNghiPhep = value; OnPropertyChanged(); } }
        private string _SelectedCoLuong;
        public string SelectedCoLuong { get => _SelectedCoLuong; set { _SelectedCoLuong = value; OnPropertyChanged(); } }

        private LOAINGHIPHEP _SelectedLoaiNghiPhep;
        public LOAINGHIPHEP SelectedLoaiNghiPhep { get => _SelectedLoaiNghiPhep; set { _SelectedLoaiNghiPhep = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchLoaiNghiPhep;
        public string SearchLoaiNghiPhep { get => _SearchLoaiNghiPhep; set { _SearchLoaiNghiPhep = value; OnPropertyChanged(); } }
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

        public LoaiNghiPhepViewModel()
        {
            LoadListLoaiNghiPhep();

            string[] CoLuongArray = new string[] { "Có", "Không" };
            ListCoLuong = new ObservableCollection<string>(CoLuongArray);

            IsEditable = false;

            // Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
                ResetControls();

                LoaiNghiPhepWindow loaiNghiPhepWindow = new LoaiNghiPhepWindow();
                loaiNghiPhepWindow.ShowDialog();
            });

            // Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(TenLoaiNghiPhep) || SelectedCoLuong == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin loại nghỉ phép!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedLoaiNghiPhep == null)
                {
                    var LoaiNghiPhepMoi = new LOAINGHIPHEP()
                    {
                        TEN_LNP = TenLoaiNghiPhep,
                        COLUONG_LNP = SelectedCoLuong == "Có" ? true : false
                    };
                    DataProvider.Ins.model.LOAINGHIPHEP.Add(LoaiNghiPhepMoi);
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm loại nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var LoaiNghiPhepSua = DataProvider.Ins.model.LOAINGHIPHEP.Where(x => x.MA_LNP == SelectedLoaiNghiPhep.MA_LNP).SingleOrDefault();
                    LoaiNghiPhepSua.TEN_LNP = TenLoaiNghiPhep;
                    LoaiNghiPhepSua.COLUONG_LNP = _SelectedCoLuong == "Có" ? true : false;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadListLoaiNghiPhep();
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

                if (SelectedLoaiNghiPhep == null)
                {
                    return false;
                }
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa loại nghỉ phép", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        var lnp = DataProvider.Ins.model.LOAINGHIPHEP.Where(x => x.MA_LNP == SelectedLoaiNghiPhep.MA_LNP).FirstOrDefault();
                        DataProvider.Ins.model.LOAINGHIPHEP.Remove(lnp);
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
                    LoadListLoaiNghiPhep();
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
                return SelectedLoaiNghiPhep == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;

                TenLoaiNghiPhep = SelectedLoaiNghiPhep.TEN_LNP;
                SelectedCoLuong = SelectedLoaiNghiPhep.COLUONG_LNP == true ? "Có" : "Không";

                LoaiNghiPhepWindow loaiNghiPhepWindow = new LoaiNghiPhepWindow();
                loaiNghiPhepWindow.ShowDialog();
            });

            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListLoaiNghiPhep);
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
                if (string.IsNullOrEmpty(SearchLoaiNghiPhep))
                {
                    CollectionViewSource.GetDefaultView(ListLoaiNghiPhep).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListLoaiNghiPhep).Filter = (searchLoaiNghiPhep) =>
                    {
                        return (searchLoaiNghiPhep as LOAINGHIPHEP).TEN_LNP.IndexOf(SearchLoaiNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
        }

        public void LoadListLoaiNghiPhep()
        {
            ListLoaiNghiPhep = new ObservableCollection<LOAINGHIPHEP>(DataProvider.Ins.model.LOAINGHIPHEP);
        }

        public void ResetControls()
        {
            SelectedLoaiNghiPhep = null;
            TenLoaiNghiPhep = null;
            SelectedCoLuong = null;
        }

    }
}
