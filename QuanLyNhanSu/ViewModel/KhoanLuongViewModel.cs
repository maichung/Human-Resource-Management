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
    public class KhoanLuongViewModel : BaseViewModel
    {
        #region List item source
        private ObservableCollection<ThongTinKhoanLuong> _ListTTKhoanLuong_MainWD;
        public ObservableCollection<ThongTinKhoanLuong> ListTTKhoanLuong_MainWD { get => _ListTTKhoanLuong_MainWD; set { _ListTTKhoanLuong_MainWD = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinKhoanLuong> _ListTTKhoanLuong_KLWD;
        public ObservableCollection<ThongTinKhoanLuong> ListTTKhoanLuong_KLWD { get => _ListTTKhoanLuong_KLWD; set { _ListTTKhoanLuong_KLWD = value; OnPropertyChanged(); } }
        private ObservableCollection<ThongTinKhoanLuong> _ListTTKhoanLuong_CapNhat;
        public ObservableCollection<ThongTinKhoanLuong> ListTTKhoanLuong_CapNhat { get => _ListTTKhoanLuong_CapNhat; set { _ListTTKhoanLuong_CapNhat = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        //Chọn nhân viên trong combobox
        private NHANVIEN _SelectedCmbNV;
        public NHANVIEN SelectedCmbNV { get => _SelectedCmbNV; set { _SelectedCmbNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private ThongTinKhoanLuong _SelectedTTKhoanLuong;
        public ThongTinKhoanLuong SelectedTTKhoanLuong { get => _SelectedTTKhoanLuong; set { _SelectedTTKhoanLuong = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        private bool _IsSelectNV;
        public bool IsSelectNV { get => _IsSelectNV; set { _IsSelectNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchTTKhoanLuong;
        public string SearchTTKhoanLuong { get => _SearchTTKhoanLuong; set { _SearchTTKhoanLuong = value; OnPropertyChanged(); } }
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

        public KhoanLuongViewModel()
        {
            //Màn hình chính tab Khoản nghỉ phép
            LoadTTKhoanLuong_MainWD();

            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = true;
                SelectedTTKhoanLuong = null;
                LoadTTKhoanLuong_KLWD();
                LoadListNhanVien();

                KhoanLuongWindow khoanLuongWindow = new KhoanLuongWindow();
                khoanLuongWindow.ShowDialog();
            });

            //Hiển thị Command
            HienThiCommand = new RelayCommand<Object>((p) => { return SelectedTTKhoanLuong == null ? false : true; }, (p) =>
            {
                LoadTTKhoanLuong_KLWD();
                LoadListNhanVien();
                IsEditable = false;
                IsSelectNV = false;
                SelectedCmbNV = SelectedTTKhoanLuong.NhanVien;


                KhoanLuongWindow khoanLuongWindow = new KhoanLuongWindow();
                khoanLuongWindow.ShowDialog();
            });

            // Lưu command
            LuuCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (SelectedTTKhoanLuong == null)
                {
                    foreach (ThongTinKhoanLuong item in ListTTKhoanLuong_KLWD)
                    {
                        var KhoanLuongMoi = new KHOANLUONG()
                        {
                            MA_NV = SelectedCmbNV.MA_NV,
                            MA_LL = item.LoaiLuong.MA_LL,
                            SOTIEN_KL = item.SoTien
                        };

                        DataProvider.Ins.model.KHOANLUONG.Add(KhoanLuongMoi);
                        DataProvider.Ins.model.SaveChanges();
                    }

                    MessageBox.Show("Thêm khoản lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var listKhoanLuongSua = DataProvider.Ins.model.KHOANLUONG.Where(x => x.MA_NV == SelectedTTKhoanLuong.NhanVien.MA_NV);
                    foreach (KHOANLUONG kl in listKhoanLuongSua)
                    {
                        foreach (ThongTinKhoanLuong ttkl in ListTTKhoanLuong_KLWD)
                        {
                            if (kl.MA_LL == ttkl.LoaiLuong.MA_LL)
                            {
                                kl.SOTIEN_KL = ttkl.SoTien;
                                break;
                            }
                        }
                    }
                    DataProvider.Ins.model.SaveChanges();

                    if (ListTTKhoanLuong_CapNhat != null)
                    {
                        foreach (ThongTinKhoanLuong item in ListTTKhoanLuong_CapNhat)
                        {
                            var KhoanLuongMoi = new KHOANLUONG()
                            {
                                MA_NV = SelectedTTKhoanLuong.NhanVien.MA_NV,
                                MA_LL = item.LoaiLuong.MA_LL,
                                SOTIEN_KL = item.SoTien
                            };

                            DataProvider.Ins.model.KHOANLUONG.Add(KhoanLuongMoi);
                            DataProvider.Ins.model.SaveChanges();
                        }
                    }

                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                LoadTTKhoanLuong_MainWD();
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
                if (string.IsNullOrEmpty(SearchTTKhoanLuong))
                {
                    CollectionViewSource.GetDefaultView(ListTTKhoanLuong_MainWD).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListTTKhoanLuong_MainWD).Filter = (searchTTKhoanLuong) =>
                    {
                        return (searchTTKhoanLuong as ThongTinKhoanLuong).NhanVien.HOTEN_NV.IndexOf(SearchTTKhoanLuong, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchTTKhoanLuong as ThongTinKhoanLuong).LoaiLuong.TEN_LL.IndexOf(SearchTTKhoanLuong, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });

            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTTKhoanLuong_MainWD);
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

            if (SelectedTTKhoanLuong == null)
            {
                ListNhanVien = new ObservableCollection<NHANVIEN>();
                var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN
                                   where (from kl in DataProvider.Ins.model.KHOANLUONG
                                          where nv.MA_NV == kl.MA_NV
                                          select kl)
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

        public void LoadTTKhoanLuong_KLWD()
        {
            ListTTKhoanLuong_KLWD = null;
            ListTTKhoanLuong_CapNhat = null;
            ListTTKhoanLuong_KLWD = new ObservableCollection<ThongTinKhoanLuong>();
            if (SelectedTTKhoanLuong == null)
            {
                var listLoaiLuong = from ll in DataProvider.Ins.model.LOAILUONG
                                       select new ThongTinKhoanLuong()
                                       {
                                           NhanVien = null,
                                           LoaiLuong = ll,
                                           SoTien = 0
                                       };
                foreach (ThongTinKhoanLuong item in listLoaiLuong)
                {
                    ListTTKhoanLuong_KLWD.Add(item);
                }
            }
            else
            {
                var listLoaiLuongNV = from kl in DataProvider.Ins.model.KHOANLUONG
                                         join nv in DataProvider.Ins.model.NHANVIEN
                                         on kl.MA_NV equals nv.MA_NV
                                         join ll in DataProvider.Ins.model.LOAILUONG
                                         on kl.MA_LL equals ll.MA_LL
                                         where nv.MA_NV == SelectedTTKhoanLuong.NhanVien.MA_NV
                                         select new ThongTinKhoanLuong()
                                         {
                                             NhanVien = nv,
                                             LoaiLuong = ll,
                                             SoTien = (int)kl.SOTIEN_KL
                                         };

                foreach (ThongTinKhoanLuong item in listLoaiLuongNV)
                {
                    ListTTKhoanLuong_KLWD.Add(item);
                }

                //Xử lý việc có thêm loại nghỉ phép cho nhân viên khi nhân viên đã được tạo khoản nghỉ phép
                var listLoaiLuong = from ll in DataProvider.Ins.model.LOAILUONG
                                       where (from llNV in listLoaiLuongNV
                                              where ll.MA_LL == llNV.LoaiLuong.MA_LL
                                              select ll).FirstOrDefault() == null
                                       select new ThongTinKhoanLuong()
                                       {
                                           NhanVien = null,
                                           LoaiLuong = ll,
                                           SoTien = 0
                                       };

                if (listLoaiLuong.Count() > 0)
                {
                    ListTTKhoanLuong_CapNhat = new ObservableCollection<ThongTinKhoanLuong>();
                    foreach (ThongTinKhoanLuong item in listLoaiLuong)
                    {
                        ListTTKhoanLuong_KLWD.Add(item);
                        ListTTKhoanLuong_CapNhat.Add(item);
                    }
                }
            }

        }

        public void LoadTTKhoanLuong_MainWD()
        {
            ListTTKhoanLuong_MainWD = new ObservableCollection<ThongTinKhoanLuong>();
            var listKhoanLuong = from kl in DataProvider.Ins.model.KHOANLUONG
                                    join nv in DataProvider.Ins.model.NHANVIEN
                                    on kl.MA_NV equals nv.MA_NV
                                    join ll in DataProvider.Ins.model.LOAILUONG
                                    on kl.MA_LL equals ll.MA_LL
                                    select new ThongTinKhoanLuong()
                                    {
                                        NhanVien = nv,
                                        LoaiLuong = ll,
                                        SoTien = (int)kl.SOTIEN_KL
                                    };
            foreach (ThongTinKhoanLuong item in listKhoanLuong)
            {
                ListTTKhoanLuong_MainWD.Add(item);
            }

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTTKhoanLuong_MainWD);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("NhanVien.HOTEN_NV"));
        }
    }
}
