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
        private ObservableCollection<KHOANLUONG> _ListKhoanLuong_MainWD;
        public ObservableCollection<KHOANLUONG> ListKhoanLuong_MainWD { get => _ListKhoanLuong_MainWD; set { _ListKhoanLuong_MainWD = value; OnPropertyChanged(); } }
        private ObservableCollection<KHOANLUONG> _ListKhoanLuong_KLWD;
        public ObservableCollection<KHOANLUONG> ListKhoanLuong_KLWD { get => _ListKhoanLuong_KLWD; set { _ListKhoanLuong_KLWD = value; OnPropertyChanged(); } }
        private ObservableCollection<KHOANLUONG> _ListKhoanLuong_CapNhat;
        public ObservableCollection<KHOANLUONG> ListKhoanLuong_CapNhat { get => _ListKhoanLuong_CapNhat; set { _ListKhoanLuong_CapNhat = value; OnPropertyChanged(); } }
        private ObservableCollection<KHOANLUONG> _ListKhoanLuong;
        public ObservableCollection<KHOANLUONG> ListKhoanLuong { get => _ListKhoanLuong; set { _ListKhoanLuong = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        //Chọn nhân viên trong combobox
        private NHANVIEN _SelectedCmbNV;
        public NHANVIEN SelectedCmbNV { get => _SelectedCmbNV; set { _SelectedCmbNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private KHOANLUONG _SelectedKhoanLuong;
        public KHOANLUONG SelectedKhoanLuong { get => _SelectedKhoanLuong; set { _SelectedKhoanLuong = value; OnPropertyChanged(); } }
        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        private bool _IsSelectNV;
        public bool IsSelectNV { get => _IsSelectNV; set { _IsSelectNV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
        private string _SearchKhoanLuong;
        public string SearchKhoanLuong { get => _SearchKhoanLuong; set { _SearchKhoanLuong = value; OnPropertyChanged(); } }
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
            LoadListKhoanLuong_MainWD();

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                IsEditable = true;
                IsSelectNV = true;
                SelectedKhoanLuong = null;
                LoadListKhoanLuong_KLWD();
                LoadListNhanVien();

                KhoanLuongWindow khoanLuongWindow = new KhoanLuongWindow();
                khoanLuongWindow.ShowDialog();
            });
            #endregion

            #region Hiển thị Command
            HienThiCommand = new RelayCommand<Object>((p) => { return SelectedKhoanLuong == null ? false : true; }, (p) =>
            {
                LoadListKhoanLuong_KLWD();
                LoadListNhanVien();
                IsEditable = false;
                IsSelectNV = false;
                SelectedCmbNV = SelectedKhoanLuong.NHANVIEN;


                KhoanLuongWindow khoanLuongWindow = new KhoanLuongWindow();
                khoanLuongWindow.ShowDialog();
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
                if (SelectedKhoanLuong == null)
                {
                    foreach (KHOANLUONG item in ListKhoanLuong_KLWD)
                    {
                        var khoanLuongMoi = new KHOANLUONG()
                        {
                            MA_NV = SelectedCmbNV.MA_NV,
                            MA_LL = item.MA_LL,
                            SOTIEN_KL = item.SOTIEN_KL
                        };

                        DataProvider.Ins.model.KHOANLUONG.Add(khoanLuongMoi);
                        DataProvider.Ins.model.SaveChanges();

                        ListKhoanLuong_MainWD.Add(khoanLuongMoi);
                    }

                    MessageBox.Show("Thêm khoản lương thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    var listKhoanLuongSua = DataProvider.Ins.model.KHOANLUONG.Where(x => x.MA_NV == SelectedKhoanLuong.MA_NV);
                    foreach (KHOANLUONG klCu in listKhoanLuongSua)
                    {
                        foreach (KHOANLUONG klMoi in ListKhoanLuong_KLWD)
                        {
                            if (klCu.MA_LL == klMoi.MA_LL)
                            {
                                klCu.SOTIEN_KL = klMoi.SOTIEN_KL;
                                break;
                            }
                        }
                    }
                    DataProvider.Ins.model.SaveChanges();

                    if (ListKhoanLuong_CapNhat != null)
                    {
                        foreach (KHOANLUONG item in ListKhoanLuong_CapNhat)
                        {
                            var khoanLuongMoi = new KHOANLUONG()
                            {
                                MA_NV = SelectedKhoanLuong.MA_NV,
                                MA_LL = item.MA_LL,
                                SOTIEN_KL = item.SOTIEN_KL
                            };

                            DataProvider.Ins.model.KHOANLUONG.Add(khoanLuongMoi);
                            DataProvider.Ins.model.SaveChanges();

                            ListKhoanLuong_MainWD.Add(khoanLuongMoi);
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
            SuaCommand = new RelayCommand<Object>((p) => 
            {
                if (SelectedKhoanLuong == null)
                {
                    MessageBox.Show("Vui lòng chọn khoản lương trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (string.IsNullOrEmpty(SearchKhoanLuong))
                {
                    CollectionViewSource.GetDefaultView(ListKhoanLuong_MainWD).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListKhoanLuong_MainWD).Filter = (searchKhoanLuong) =>
                    {
                        return (searchKhoanLuong as KHOANLUONG).NHANVIEN.HOTEN_NV.IndexOf(SearchKhoanLuong, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchKhoanLuong as KHOANLUONG).LOAILUONG.TEN_LL.IndexOf(SearchKhoanLuong, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListKhoanLuong_MainWD);
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
            if (SelectedKhoanLuong == null)
            {
                ListNhanVien = new ObservableCollection<NHANVIEN>();
                var listNhanVien = from nv in DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true)
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
                ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true));
            }


        }

        public void LoadListKhoanLuong_KLWD()
        {
            ListKhoanLuong_CapNhat = null;
            ListKhoanLuong_KLWD = new ObservableCollection<KHOANLUONG>();
            if (SelectedKhoanLuong == null)
            {
                var listLoaiLuong = from ll in DataProvider.Ins.model.LOAILUONG
                                    select ll;

                foreach (LOAILUONG item in listLoaiLuong)
                {
                    ListKhoanLuong_KLWD.Add(new KHOANLUONG()
                    {
                        LOAILUONG = item,
                        MA_LL = item.MA_LL,
                        SOTIEN_KL = 0
                    });
                }
            }
            else
            {
                var listLoaiLuongNV = from kl in DataProvider.Ins.model.KHOANLUONG
                                      where kl.MA_NV == SelectedKhoanLuong.MA_NV && kl.NHANVIEN.TRANGTHAI_NV == true
                                      select kl;

                foreach (KHOANLUONG item in listLoaiLuongNV)
                {
                    ListKhoanLuong_KLWD.Add(item);
                }

                //Xử lý việc có thêm loại lương cho nhân viên khi nhân viên đã được tạo khoản lương
                var listLoaiLuong = from ll in DataProvider.Ins.model.LOAILUONG
                                    where (from llNV in listLoaiLuongNV
                                           where ll.MA_LL == llNV.MA_LL
                                           select ll).FirstOrDefault() == null
                                    select ll;

                if (listLoaiLuong.Count() > 0)
                {
                    ListKhoanLuong_CapNhat = new ObservableCollection<KHOANLUONG>();
                    foreach (LOAILUONG item in listLoaiLuong)
                    {
                        KHOANLUONG kl = new KHOANLUONG()
                        {
                            MA_LL = item.MA_LL,
                            SOTIEN_KL = 0
                        };
                        ListKhoanLuong_KLWD.Add(kl);
                        ListKhoanLuong_CapNhat.Add(kl);
                    }
                }
            }

        }

        public void LoadListKhoanLuong_MainWD()
        {
            ListKhoanLuong_MainWD = new ObservableCollection<KHOANLUONG>(DataProvider.Ins.model.KHOANLUONG.Where(x => x.NHANVIEN.TRANGTHAI_NV == true));

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListKhoanLuong_MainWD);
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription("NHANVIEN.HOTEN_NV"));
        }
        #endregion
    }
}
