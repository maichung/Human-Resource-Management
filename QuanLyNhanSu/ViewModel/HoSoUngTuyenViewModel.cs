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
    class HoSoUngTuyenViewModel:BaseViewModel
    {
        #region DataContext
        private ObservableCollection<HOSOUNGTUYEN> _ListHSUT;
        public ObservableCollection<HOSOUNGTUYEN> ListHSUT { get => _ListHSUT; set { _ListHSUT = value; OnPropertyChanged(); } }
        #endregion 

        #region Combobox item sources
        private ObservableCollection<string> _ListTrangThai;
        public ObservableCollection<string> ListTrangThai { get => _ListTrangThai; set { _ListTrangThai = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _ViTriCongViec;
        public string ViTriCongViec { get => _ViTriCongViec; set { _ViTriCongViec = value; OnPropertyChanged(); } }

        private string _ChucVu;
        public string ChucVu { get => _ChucVu; set { _ChucVu = value; OnPropertyChanged(); } }

        private string _SelectedTrangThai;
        public string SelectedTrangThai { get => _SelectedTrangThai; set { _SelectedTrangThai = value; OnPropertyChanged(); } }

        private DateTime? _NgayNop;
        public DateTime? NgayNop { get => _NgayNop; set { _NgayNop = value; OnPropertyChanged(); } }

        private  UNGVIEN _SelectedUngVien;
        public  UNGVIEN SelectedUngVien { get => _SelectedUngVien; set { _SelectedUngVien = value; OnPropertyChanged(); } }

        private HOSOUNGTUYEN _SelectedHSUT;
        public HOSOUNGTUYEN SelectedHSUT { get => _SelectedHSUT; set { _SelectedHSUT = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        private string _SearchHSUT;
        public string SearchHSUT { get => _SearchHSUT; set { _SearchHSUT = value; OnPropertyChanged(); } }

        public bool sort;
        #endregion

        #region Command binding
        public ICommand TaoHSUTCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        public ICommand XoaHoSoUngTuyenCommand { get; set; }
        #endregion

 

        public HoSoUngTuyenViewModel()
        {
            LoadListHSUT();
            string[] DSTrangThai = new string[] { "Passed", "Failed" };
            ListTrangThai = new ObservableCollection<string>(DSTrangThai);
            IsEditable = false;

            //Xóa hồ sơ ứng tuyển command
            XoaHoSoUngTuyenCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedHSUT == null)
                {
                    return false;
                }
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa tài khoản", MessageBoxButton.YesNo, MessageBoxImage.Question);
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
                        var hsut = DataProvider.Ins.model.HOSOUNGTUYEN.Where(x => x.MA_HSUT == SelectedHSUT.MA_HSUT).FirstOrDefault();
                        DataProvider.Ins.model.HOSOUNGTUYEN.Remove(hsut);
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
                    LoadListHSUT();

                }
            });
            //Tạo mới command
            TaoHSUTCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedUngVien==null)
                {
                    MessageBox.Show("Vui lòng thêm ứng viên trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                
                ResetControls();
                IsEditable = true;
                SelectedHSUT = null;
                HoSoUngTuyen hoSoUngTuyen = new HoSoUngTuyen();
                
                hoSoUngTuyen.ShowDialog();
            });

            //Lưu Command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(ViTriCongViec)
                || string.IsNullOrEmpty(ChucVu)
                || SelectedTrangThai == null
                || NgayNop == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin hồ sơ ứng tuyển!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedHSUT == null)
                {
                    var HSUTMoi = new HOSOUNGTUYEN()
                    {
                        CHUCVU_HSUT = ChucVu,
                        VITRICONGVIEC_HSUT = ViTriCongViec,
                        TRANGTHAI_HSUT = SelectedTrangThai == "Passed" ? true : false,
                        NGAYNOP_HSUT = NgayNop,
                        MA_UV = SelectedUngVien.MA_UV,
                        CV_HSUT="ds"
                    };
                    DataProvider.Ins.model.HOSOUNGTUYEN.Add(HSUTMoi);
                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm hồ sơ ứng tuyển mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                    //ReloadListHSUT();
                }
                else
                {
                    var HSUTSua = DataProvider.Ins.model.HOSOUNGTUYEN.Where(x => x.MA_HSUT == SelectedHSUT.MA_HSUT).SingleOrDefault();
                    HSUTSua.VITRICONGVIEC_HSUT = ViTriCongViec;
                    HSUTSua.CHUCVU_HSUT = ChucVu;
                    HSUTSua.TRANGTHAI_HSUT =SelectedTrangThai == "Passed" ? true : false; ;
                    HSUTSua.NGAYNOP_HSUT = NgayNop;
                   // HSUTSua.MA_UV = SDT;
                   // HSUTSua.CV_HSUT = DiaChi;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ReloadListHSUT();
                p.Close();
            });

            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListHSUT);
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
                if (string.IsNullOrEmpty(SearchHSUT))
                {
                    CollectionViewSource.GetDefaultView(ListHSUT).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListHSUT).Filter = (searchHSUT) =>
                    {
                        return (searchHSUT as UNGVIEN).HOTEN_UV.IndexOf(SearchHSUT, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }

            });
            //Hủy command
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
            //Sửa Command
            SuaCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                IsEditable = true;
            }
            );

            //Hiển thị Command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedHSUT == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;
                ViTriCongViec = SelectedHSUT.VITRICONGVIEC_HSUT;
                SelectedTrangThai = SelectedHSUT.TRANGTHAI_HSUT == true ? "Passed" : "Failed";
                ChucVu = SelectedHSUT.CHUCVU_HSUT;
                NgayNop = SelectedHSUT.NGAYNOP_HSUT;
                //CV = SelectedHSUT.CV_HSUT;
               
                HoSoUngTuyen hoSoUngTuyenWindow= new HoSoUngTuyen();
                hoSoUngTuyenWindow.ShowDialog();

            });

        }

        void LoadListHSUT()
        {
            ListHSUT = new ObservableCollection<HOSOUNGTUYEN>(DataProvider.Ins.model.HOSOUNGTUYEN);
           
        }
       public void ReloadListHSUT()
        {
            if (SelectedUngVien==null)
            {
                ListHSUT = null;
                return;
               
               // LoadListHSUT();
            }
            else
            ListHSUT = new ObservableCollection<HOSOUNGTUYEN>(DataProvider.Ins.model.HOSOUNGTUYEN.Where(x => x.MA_UV == SelectedUngVien.MA_UV));
        }
       
        public void ResetControls()
        {
            ViTriCongViec = null;
            SearchHSUT = null;
            ChucVu = null;
            NgayNop = null;
        }
    }
}
