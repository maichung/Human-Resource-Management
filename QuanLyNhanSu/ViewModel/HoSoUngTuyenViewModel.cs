using Microsoft.Win32;
using QuanLyNhanSu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
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
    class HoSoUngTuyenViewModel : BaseViewModel
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

        private string _SelectedTrangThai;
        public string SelectedTrangThai { get => _SelectedTrangThai; set { _SelectedTrangThai = value; OnPropertyChanged(); } }

        private DateTime? _NgayNop;
        public DateTime? NgayNop { get => _NgayNop; set { _NgayNop = value; OnPropertyChanged(); } }

        private UNGVIEN _SelectedUngVien;
        public UNGVIEN SelectedUngVien { get => _SelectedUngVien; set { _SelectedUngVien = value; OnPropertyChanged(); } }

        private HOSOUNGTUYEN _SelectedHSUT;
        public HOSOUNGTUYEN SelectedHSUT { get => _SelectedHSUT; set { _SelectedHSUT = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        private byte[] _CV;
        public byte[] CV { get => _CV; set { _CV = value; OnPropertyChanged(); } }

        private string _TenFileCV;
        public string TenFileCV { get => _TenFileCV; set { _TenFileCV = value; OnPropertyChanged(); } }

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
        public ICommand ChonFileCommand { get; set; }
        public ICommand XemFileCommand { get; set; }
        #endregion

        #region Xử lý lưu và đọc file
        public byte[] FileToBinaryString(string filePath)
        {
            try
            {
                byte[] binaryString;
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        binaryString = reader.ReadBytes((int)stream.Length);
                    }
                }
                return binaryString;
            }
            catch (IOException e)
            {
                MessageBox.Show("Đã xảy ra lỗi!" + e.ToString(), "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }

        public string BinaryStringToFile(byte[] binaryString)
        {
            if (binaryString != null)
            {
                var fs = new FileStream("../../Resources/TempFiles/cv.pdf", FileMode.Create, FileAccess.Write);
                fs.Write(binaryString, 0, binaryString.Length);
                return fs.Name;
            }
            else
            {
                MessageBox.Show("Đã xảy ra lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return null;
            }
        }
        #endregion

        public HoSoUngTuyenViewModel()
        {
            LoadListHSUT();
            string[] DSTrangThai = new string[] { "Chưa xử lý", "Chấp nhận", "Từ chối" };
            ListTrangThai = new ObservableCollection<string>(DSTrangThai);
            IsEditable = false;

            //Xóa hồ sơ ứng tuyển command
            XoaHoSoUngTuyenCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedHSUT == null)
                {
                    MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa tài khoản", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
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
                        ReloadListHSUT();

                    }
                }

            });

            //Tạo mới command
            TaoHSUTCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedUngVien == null)
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
                        VITRICONGVIEC_HSUT = ViTriCongViec,
                        TRANGTHAI_HSUT = SelectedTrangThai,
                        NGAYNOP_HSUT = NgayNop,
                        MA_UV = SelectedUngVien.MA_UV,
                        CV_HSUT = CV
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
                    HSUTSua.TRANGTHAI_HSUT = SelectedTrangThai;
                    HSUTSua.NGAYNOP_HSUT = NgayNop;
                    HSUTSua.CV_HSUT = CV;


                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ReloadListHSUT();
                p.Close();
            });

            //Sort command
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

            //Search command
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
                if (SelectedHSUT==null)
                {
                    MessageBox.Show("Không thể sửa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
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
                SelectedTrangThai = SelectedHSUT.TRANGTHAI_HSUT;
                NgayNop = SelectedHSUT.NGAYNOP_HSUT;
                CV = SelectedHSUT.CV_HSUT;

                HoSoUngTuyen hoSoUngTuyenWindow = new HoSoUngTuyen();
                hoSoUngTuyenWindow.ShowDialog();

            });

            // Chọn file command
            ChonFileCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Chọn file CV",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "pdf",
                    //Filter = "PDF files (*.pdf)|*.txt|Microsoft Word Document (*.doc)|*.docx",
                    Filter = "PDF files (*.pdf)|*.pdf",
                    RestoreDirectory = true,

                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    TenFileCV = System.IO.Path.GetFileName(openFileDialog.FileName);
                    CV = FileToBinaryString(openFileDialog.FileName);
                }
            });

            // Xem file command
            XemFileCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    Process.Start(BinaryStringToFile(CV));
                }
                catch (Exception)
                {
                    MessageBox.Show("Đã xảy ra lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            });
        }


        void LoadListHSUT()
        {
            ListHSUT = new ObservableCollection<HOSOUNGTUYEN>(DataProvider.Ins.model.HOSOUNGTUYEN);

        }
        public void ReloadListHSUT()
        {
            if (SelectedUngVien == null)
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
            NgayNop = null;
        }

    }
}
