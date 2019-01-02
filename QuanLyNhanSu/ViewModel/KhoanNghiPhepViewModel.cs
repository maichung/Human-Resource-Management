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
        private ObservableCollection<ThongTinKhoanNghiPhep> _ListTTKhoanNghiPhep_MainWD;
        public ObservableCollection<ThongTinKhoanNghiPhep> ListTTKhoanNghiPhep_MainWD { get => _ListTTKhoanNghiPhep_MainWD; set { _ListTTKhoanNghiPhep_MainWD = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinKhoanNghiPhep> _ListTTKhoanNghiPhep_KNPWD;
        public ObservableCollection<ThongTinKhoanNghiPhep> ListTTKhoanNghiPhep_KNPWD { get => _ListTTKhoanNghiPhep_KNPWD; set { _ListTTKhoanNghiPhep_KNPWD = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinKhoanNghiPhep> _ListTTKhoanNghiPhep_CapNhat;
        public ObservableCollection<ThongTinKhoanNghiPhep> ListTTKhoanNghiPhep_CapNhat { get => _ListTTKhoanNghiPhep_CapNhat; set { _ListTTKhoanNghiPhep_CapNhat = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        //Chọn nhân viên trong combobox
        private NHANVIEN _SelectedCmbNV;
        public NHANVIEN SelectedCmbNV { get => _SelectedCmbNV; set { _SelectedCmbNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private ThongTinKhoanNghiPhep _SelectedTTKhoanNghiPhep;
        public ThongTinKhoanNghiPhep SelectedTTKhoanNghiPhep { get => _SelectedTTKhoanNghiPhep; set { _SelectedTTKhoanNghiPhep = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        private bool _IsSelectNV;
        public bool IsSelectNV { get => _IsSelectNV; set { _IsSelectNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchTTKhoanNghiPhep;
        public string SearchTTKhoanNghiPhep { get => _SearchTTKhoanNghiPhep; set { _SearchTTKhoanNghiPhep = value; OnPropertyChanged(); } }
        public bool sort;
        #endregion

        #region Command binding
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
            //Màn hình chính tab Khoản nghỉ phép
            LoadTTKhoanNghiPhep_MainWD();            

            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = true;
                SelectedTTKhoanNghiPhep = null;
                LoadTTKhoanNghiPhep_KNPWD();
                LoadListNhanVien();

                KhoanNghiPhepWindow khoanNghiPhepWindow = new KhoanNghiPhepWindow();
                khoanNghiPhepWindow.ShowDialog();
            });

            //Hiển thị Command
            HienThiCommand = new RelayCommand<Object>((p) => { return SelectedTTKhoanNghiPhep == null ? false : true; }, (p) =>
            {
                LoadTTKhoanNghiPhep_KNPWD();
                LoadListNhanVien();
                IsEditable = false;
                IsSelectNV = false;
                SelectedCmbNV = SelectedTTKhoanNghiPhep.NhanVien;
                

                KhoanNghiPhepWindow khoanNghiPhepWindow = new KhoanNghiPhepWindow();
                khoanNghiPhepWindow.ShowDialog();
            });

            // Lưu command
            LuuCommand = new RelayCommand<Window>((p) => {
                if (SelectedCmbNV == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
                if (SelectedTTKhoanNghiPhep == null)
                {
                    foreach (ThongTinKhoanNghiPhep item in ListTTKhoanNghiPhep_KNPWD)
                    {
                        var KhoanNghiPhepMoi = new KHOANNGHIPHEP()
                        {
                            MA_NV = SelectedCmbNV.MA_NV,
                            MA_LNP = item.LoaiNghiPhep.MA_LNP,
                            SONGAYNGHI_KNP = item.SoNgayNghi
                        };

                        DataProvider.Ins.model.KHOANNGHIPHEP.Add(KhoanNghiPhepMoi);
                        DataProvider.Ins.model.SaveChanges();
                    }
                    
                    MessageBox.Show("Thêm khoản nghỉ phép thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var listKhoanNghiPhepSua = DataProvider.Ins.model.KHOANNGHIPHEP.Where(x => x.MA_NV == SelectedTTKhoanNghiPhep.NhanVien.MA_NV);
                    foreach (KHOANNGHIPHEP knp in listKhoanNghiPhepSua)
                    {
                        foreach (ThongTinKhoanNghiPhep ttknp in ListTTKhoanNghiPhep_KNPWD)
                        {
                            if (knp.MA_LNP == ttknp.LoaiNghiPhep.MA_LNP)
                            {
                                knp.SONGAYNGHI_KNP = ttknp.SoNgayNghi;
                                break;
                            }
                        }
                    }
                    DataProvider.Ins.model.SaveChanges();

                    if (ListTTKhoanNghiPhep_CapNhat != null)
                    {
                        foreach (ThongTinKhoanNghiPhep item in ListTTKhoanNghiPhep_CapNhat)
                        {
                            var KhoanNghiPhepMoi = new KHOANNGHIPHEP()
                            {
                                MA_NV = SelectedTTKhoanNghiPhep.NhanVien.MA_NV,
                                MA_LNP = item.LoaiNghiPhep.MA_LNP,
                                SONGAYNGHI_KNP = item.SoNgayNghi
                            };

                            DataProvider.Ins.model.KHOANNGHIPHEP.Add(KhoanNghiPhepMoi);
                            DataProvider.Ins.model.SaveChanges();
                        }
                    }

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadTTKhoanNghiPhep_MainWD();
                p.Close();
            });

            //Hủy command
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

            //Sửa Command
            SuaCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = false;
            });

            //Search command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchTTKhoanNghiPhep))
                {
                    CollectionViewSource.GetDefaultView(ListTTKhoanNghiPhep_MainWD).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListTTKhoanNghiPhep_MainWD).Filter = (searchTTKhoanNghiPhep) =>
                    {
                        return (searchTTKhoanNghiPhep as ThongTinKhoanNghiPhep).NhanVien.HOTEN_NV.IndexOf(SearchTTKhoanNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchTTKhoanNghiPhep as ThongTinKhoanNghiPhep).LoaiNghiPhep.TEN_LNP.IndexOf(SearchTTKhoanNghiPhep, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });

            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTTKhoanNghiPhep_MainWD);
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
        }

        void LoadListNhanVien()
        {
            ListNhanVien = null;           

            if(SelectedTTKhoanNghiPhep == null)
            {
                ListNhanVien = new ObservableCollection<NHANVIEN>();
                var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                                   where (from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                          where nv.MA_NV == knp.MA_NV
                                          select knp)
                                         .FirstOrDefault() == null
                                   select nv;

                foreach (NHANVIEN item in listNhanVien)
                {
                    ListNhanVien.Add(item);
                }
            }
            else
            {
                ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN);
            }


        }

        public void LoadTTKhoanNghiPhep_KNPWD()
        {
            ListTTKhoanNghiPhep_KNPWD = null;
            ListTTKhoanNghiPhep_CapNhat = null;
            ListTTKhoanNghiPhep_KNPWD = new ObservableCollection<ThongTinKhoanNghiPhep>();
            if(SelectedTTKhoanNghiPhep == null)
            {
                var listLoaiNghiPhep = from lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                       select new ThongTinKhoanNghiPhep()
                                       {
                                           NhanVien = null,
                                           LoaiNghiPhep = lnp,
                                           SoNgayNghi = 0
                                       };
                foreach (ThongTinKhoanNghiPhep item in listLoaiNghiPhep)
                {
                    ListTTKhoanNghiPhep_KNPWD.Add(item);
                }
            }
            else
            {
                var listLoaiNghiPhepNV = from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                       join nv in DataProvider.Ins.model.NHANVIEN
                                       on knp.MA_NV equals nv.MA_NV
                                       join lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                       on knp.MA_LNP equals lnp.MA_LNP
                                       where nv.MA_NV == SelectedTTKhoanNghiPhep.NhanVien.MA_NV
                                       select new ThongTinKhoanNghiPhep()
                                       {
                                           NhanVien = nv,
                                           LoaiNghiPhep = lnp,
                                           SoNgayNghi = (int)knp.SONGAYNGHI_KNP
                                       };

                foreach (ThongTinKhoanNghiPhep item in listLoaiNghiPhepNV)
                {
                    ListTTKhoanNghiPhep_KNPWD.Add(item);
                }

                //Xử lý việc có thêm loại nghỉ phép cho nhân viên khi nhân viên đã được tạo khoản nghỉ phép
                var listLoaiNghiPhep = from lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                       where (from lnpNV in listLoaiNghiPhepNV
                                              where lnp.MA_LNP == lnpNV.LoaiNghiPhep.MA_LNP
                                              select lnp).FirstOrDefault() == null
                                       select new ThongTinKhoanNghiPhep()
                                       {
                                           NhanVien = null,
                                           LoaiNghiPhep = lnp,
                                           SoNgayNghi = 0
                                       };

                if (listLoaiNghiPhep.Count() > 0)
                {
                    ListTTKhoanNghiPhep_CapNhat = new ObservableCollection<ThongTinKhoanNghiPhep>();
                    foreach (ThongTinKhoanNghiPhep item in listLoaiNghiPhep)
                    {
                        ListTTKhoanNghiPhep_KNPWD.Add(item);
                        ListTTKhoanNghiPhep_CapNhat.Add(item);
                    }
                }
            }
            
        }

        public void LoadTTKhoanNghiPhep_MainWD()
        {
            ListTTKhoanNghiPhep_MainWD = new ObservableCollection<ThongTinKhoanNghiPhep>();
            var listKhoanNghiPhep = from knp in DataProvider.Ins.model.KHOANNGHIPHEP
                                    join nv in DataProvider.Ins.model.NHANVIEN
                                    on knp.MA_NV equals nv.MA_NV
                                    join lnp in DataProvider.Ins.model.LOAINGHIPHEP
                                    on knp.MA_LNP equals lnp.MA_LNP
                                    select new ThongTinKhoanNghiPhep()
                                    {
                                        NhanVien = nv,
                                        LoaiNghiPhep = lnp,
                                        SoNgayNghi = (int)knp.SONGAYNGHI_KNP
                                    };
            foreach (ThongTinKhoanNghiPhep item in listKhoanNghiPhep)
            {
                ListTTKhoanNghiPhep_MainWD.Add(item);
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTTKhoanNghiPhep_MainWD);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("NhanVien.HOTEN_NV"));
        }
    }
}
