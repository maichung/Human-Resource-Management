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
    class KhoanNghiPhepViewModel : BaseViewModel
    {
        #region List item source
        private ObservableCollection<KHOANNGHIPHEP> _ListKhoanNghiPhep_MainWD;
        public ObservableCollection<KHOANNGHIPHEP> ListKhoanNghiPhep_MainWD { get => _ListKhoanNghiPhep_MainWD; set { _ListKhoanNghiPhep_MainWD = value; OnPropertyChanged(); } }
        private ObservableCollection<KHOANNGHIPHEP> _ListKhoanNghiPhep_KNPWD;
        public ObservableCollection<KHOANNGHIPHEP> ListKhoanNghiPhep_KNPWD { get => _ListKhoanNghiPhep_KNPWD; set { _ListKhoanNghiPhep_KNPWD = value; OnPropertyChanged(); } }
        private ObservableCollection<KHOANNGHIPHEP> _ListKhoanNghiPhep_CapNhat;
        public ObservableCollection<KHOANNGHIPHEP> ListKhoanNghiPhep_CapNhat { get => _ListKhoanNghiPhep_CapNhat; set { _ListKhoanNghiPhep_CapNhat = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        //Chọn nhân viên trong combobox
        private NHANVIEN _SelectedCmbNV;
        public NHANVIEN SelectedCmbNV { get => _SelectedCmbNV; set { _SelectedCmbNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private KHOANNGHIPHEP _SelectedKhoanNghiPhep;
        public KHOANNGHIPHEP SelectedKhoanNghiPhep { get => _SelectedKhoanNghiPhep; set { _SelectedKhoanNghiPhep = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        private bool _IsSelectNV;
        public bool IsSelectNV { get => _IsSelectNV; set { _IsSelectNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchKhoanNghiPhep;
        public string SearchKhoanNghiPhep { get => _SearchKhoanNghiPhep; set { _SearchKhoanNghiPhep = value; OnPropertyChanged(); } }
        public bool sort;
        #endregion

        #region Command binding
        public ICommand LoadDataKNPCommand { get; set; }
        public ICommand TaoMoiCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SortCommand { get; set; }
        #endregion

        public KhoanNghiPhepViewModel()
        {         
            #region Load dữ liệu khoản nghỉ phép Command
            LoadDataKNPCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                LoadListKhoanNghiPhep_MainWD();
            });
            #endregion

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = true;
                SelectedKhoanNghiPhep = null;
                LoadListKhoanNghiPhep_KNPWD();
                LoadListNhanVien();

                KhoanNghiPhepWindow khoanNghiPhepWindow = new KhoanNghiPhepWindow();
                khoanNghiPhepWindow.ShowDialog();
            });
            #endregion

            #region Hiển thị Command
            HienThiCommand = new RelayCommand<Object>((p) => { return SelectedKhoanNghiPhep == null ? false : true; }, (p) =>
            {
                LoadListKhoanNghiPhep_KNPWD();
                LoadListNhanVien();
                IsEditable = false;
                IsSelectNV = false;
                SelectedCmbNV = SelectedKhoanNghiPhep.NHANVIEN;
                

                KhoanNghiPhepWindow khoanNghiPhepWindow = new KhoanNghiPhepWindow();
                khoanNghiPhepWindow.ShowDialog();
            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) => {
                if (SelectedCmbNV == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                if (SelectedKhoanNghiPhep == null)
                {
                    foreach (KHOANNGHIPHEP item in ListKhoanNghiPhep_KNPWD)
                    {
                        var KhoanNghiPhepMoi = new KHOANNGHIPHEP()
                        {
                            MA_NV = SelectedCmbNV.MA_NV,
                            MA_LNP = item.MA_LNP,
                            SONGAYNGHI_KNP = item.SONGAYNGHI_KNP
                        };

                        DataProvider.Ins.model.KHOANNGHIPHEP.Add(KhoanNghiPhepMoi);
                        DataProvider.Ins.model.SaveChanges();

                        ListKhoanNghiPhep_MainWD.Add(KhoanNghiPhepMoi);
                    }                    
                    
                    MessageBox.Show("Thêm khoản nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var listKhoanNghiPhepSua = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_NV == SelectedKhoanNghiPhep.MA_NV);
                    foreach (KHOANNGHIPHEP knpCu in listKhoanNghiPhepSua)
                    {
                        foreach (KHOANNGHIPHEP knpMoi in ListKhoanNghiPhep_KNPWD)
                        {
                            if (knpCu.MA_LNP == knpMoi.MA_LNP)
                            {
                                knpCu.SONGAYNGHI_KNP = knpMoi.SONGAYNGHI_KNP;
                                break;
                            }
                        }
                    }
                    DataProvider.Ins.model.SaveChanges();

                    if (ListKhoanNghiPhep_CapNhat != null)
                    {
                        foreach (KHOANNGHIPHEP item in ListKhoanNghiPhep_CapNhat)
                        {
                            var KhoanNghiPhepMoi = new KHOANNGHIPHEP()
                            {
                                MA_NV = SelectedKhoanNghiPhep.MA_NV,
                                MA_LNP = item.MA_LNP,
                                SONGAYNGHI_KNP = item.SONGAYNGHI_KNP
                            };

                            DataProvider.Ins.model.KHOANNGHIPHEP.Add(KhoanNghiPhepMoi);
                            DataProvider.Ins.model.SaveChanges();

                            ListKhoanNghiPhep_MainWD.Add(KhoanNghiPhepMoi);
                        }
                    }

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                p.Close();
            });
            #endregion

            #region Hủy command
            HuyCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn chắc chứ?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    IsSelectNV = false;
                    p.Close();
                }
            });
            #endregion

            #region Sửa Command
            SuaCommand = new RelayCommand<Object>((p) => {
                if(SelectedKhoanNghiPhep == null)
                {
                    MessageBox.Show("Vui lòng chọn khoản nghỉ phép trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = false;
            });
            #endregion

            #region Search command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchKhoanNghiPhep))
                {
                    CollectionViewSource.GetDefaultView(ListKhoanNghiPhep_MainWD).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListKhoanNghiPhep_MainWD).Filter = (searchKhoanNghiPhep) =>
                    {
                        return (searchKhoanNghiPhep as KHOANNGHIPHEP).NHANVIEN.HOTEN_NV.IndexOf(SearchKhoanNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchKhoanNghiPhep as KHOANNGHIPHEP).LOAINGHIPHEP.TEN_LNP.IndexOf(SearchKhoanNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListKhoanNghiPhep_MainWD);
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
        }

        #region Các hàm hỗ trợ
        void LoadListNhanVien()
        {
            if(SelectedKhoanNghiPhep == null)
            {
                ListNhanVien = new ObservableCollection<NHANVIEN>();
                var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true)
                                   where (from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                          where nv.MA_NV == knp.MA_NV
                                          select knp)
                                         .FirstOrDefault() == null
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
            else
            {
                if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
                {
                    ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(x => x.MA_PB == MainViewModel.TaiKhoan.NHANVIEN.MA_PB && x.TRANGTHAI_NV == true));
                    return;
                }

                ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true));
            }
        }

        public void LoadListKhoanNghiPhep_KNPWD()
        {
            ListKhoanNghiPhep_CapNhat = null;
            ListKhoanNghiPhep_KNPWD = new ObservableCollection<KHOANNGHIPHEP>();
            if(SelectedKhoanNghiPhep == null)
            {
                var listLoaiNghiPhep = from lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                       select lnp;

                foreach (LOAINGHIPHEP item in listLoaiNghiPhep)
                {
                    ListKhoanNghiPhep_KNPWD.Add(new KHOANNGHIPHEP()
                    {
                        LOAINGHIPHEP = item,
                        MA_LNP = item.MA_LNP,
                        SONGAYNGHI_KNP = 0
                    });
                }
            }
            else
            {
                var listLoaiNghiPhepNV = from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                         where knp.MA_NV == SelectedKhoanNghiPhep.MA_NV
                                         select knp;

                if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
                {
                    foreach (KHOANNGHIPHEP item in listLoaiNghiPhepNV)
                    {
                        if (MainViewModel.TaiKhoan.NHANVIEN.MA_PB == item.NHANVIEN.MA_PB)
                        {
                            ListKhoanNghiPhep_KNPWD.Add(item);
                        }
                    }
                }
                else
                {
                    foreach (KHOANNGHIPHEP item in listLoaiNghiPhepNV)
                    {
                        ListKhoanNghiPhep_KNPWD.Add(item);
                    }
                }

                //Xử lý việc có thêm loại nghỉ phép cho nhân viên khi nhân viên đã được tạo khoản nghỉ phép
                var listLoaiNghiPhep = from lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                       where (from lnpNV in listLoaiNghiPhepNV
                                              where lnp.MA_LNP == lnpNV.MA_LNP
                                              select lnp).FirstOrDefault() == null
                                       select lnp;

                if (listLoaiNghiPhep.Count() > 0)
                {
                    ListKhoanNghiPhep_CapNhat = new ObservableCollection<KHOANNGHIPHEP>();
                    foreach (LOAINGHIPHEP item in listLoaiNghiPhep)
                    {
                        KHOANNGHIPHEP knp = new KHOANNGHIPHEP()
                        {
                            LOAINGHIPHEP = item,
                            MA_LNP = item.MA_LNP,
                            SONGAYNGHI_KNP = 0
                        };
                        ListKhoanNghiPhep_KNPWD.Add(knp);
                        ListKhoanNghiPhep_CapNhat.Add(knp);
                    }
                }
            }
            
        }

        public void LoadListKhoanNghiPhep_MainWD()
        {
            if (MainViewModel.TaiKhoan.QUYEN_TK == "Trưởng các bộ phận khác")
            {
                ListKhoanNghiPhep_MainWD = new ObservableCollection<KHOANNGHIPHEP>(DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.NHANVIEN.MA_PB == MainViewModel.TaiKhoan.NHANVIEN.MA_PB && x.NHANVIEN.TRANGTHAI_NV == true));
            }
            else
            {
                ListKhoanNghiPhep_MainWD = new ObservableCollection<KHOANNGHIPHEP>(DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.NHANVIEN.TRANGTHAI_NV == true));
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListKhoanNghiPhep_MainWD);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("NHANVIEN.HOTEN_NV"));
        }
        #endregion
    }
}
