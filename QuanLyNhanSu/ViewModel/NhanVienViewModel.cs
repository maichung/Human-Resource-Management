using Microsoft.Win32;
using QuanLyNhanSu.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QuanLyNhanSu.ViewModel
{
    public class NhanVienViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        private ObservableCollection<LICHSUNHANVIEN> _ListLichSuNhanVien;
        public ObservableCollection<LICHSUNHANVIEN> ListLichSuNhanVien { get => _ListLichSuNhanVien; set { _ListLichSuNhanVien = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item source
        private ObservableCollection<PHONGBAN> _ListPhongBan;
        public ObservableCollection<PHONGBAN> ListPhongBan { get => _ListPhongBan; set { _ListPhongBan = value; OnPropertyChanged(); } }

        private ObservableCollection<string> _ListGioiTinh;
        public ObservableCollection<string> ListGioiTinh { get => _ListGioiTinh; set { _ListGioiTinh = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(); } }

        private PHONGBAN _SelectedPhongBan;
        public PHONGBAN SelectedPhongBan { get => _SelectedPhongBan; set { _SelectedPhongBan = value; OnPropertyChanged(); } }

        private string _SelectedGioiTinh;
        public string SelectedGioiTinh { get => _SelectedGioiTinh; set { _SelectedGioiTinh = value; OnPropertyChanged(); } }

        private DateTime? _NgaySinh;
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(); } }

        private string _ChucVu;
        public string ChucVu { get => _ChucVu; set { _ChucVu = value; OnPropertyChanged(); } }

        private DateTime? _NgayVaoLam;
        public DateTime? NgayVaoLam { get => _NgayVaoLam; set { _NgayVaoLam = value; OnPropertyChanged(); } }

        private string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        private string _SoDienThoai;
        public string SoDienThoai { get => _SoDienThoai; set { _SoDienThoai = value; OnPropertyChanged(); } }

        private string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        private string _Avatar;
        public string Avatar { get => _Avatar; set { _Avatar = value; OnPropertyChanged(); } }

        private ImageSource _AvatarSource;
        public ImageSource AvatarSource { get => _AvatarSource; set { _AvatarSource = value; OnPropertyChanged(); } }

        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }
        #endregion

        #region Binding command      
        public ICommand TaoMoiCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SortLichSuNVCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand SearchLichSuNVCommand { get; set; }
        public ICommand ThayAnhCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        #endregion

        #region Thuộc tính ẩn hiện tab
        public enum ChucNangNhanVien
        {
            ThongTinNhanVien, LichSuNhanVien
        };

        private int _ChucNangNV;
        public int ChucNangNV { get => _ChucNangNV; set { _ChucNangNV = value; OnPropertyChanged(); } }

        public ICommand TabThongTinNhanVienCommand { get; set; }
        public ICommand TabLichSuNhanVienCommand { get; set; }
        #endregion

        #region Thuộc tính khác
        private string _SearchNhanVien;
        public string SearchNhanVien { get => _SearchNhanVien; set { _SearchNhanVien = value; OnPropertyChanged(); } }

        private string _SearchLichSuNV;
        public string SearchLichSuNV { get => _SearchLichSuNV; set { _SearchLichSuNV = value; OnPropertyChanged(); } }

        public bool sort;
        #endregion

        #region Xử lý ảnh

        // Hàm hiển thị hình ảnh từ một string
        public static BitmapImage GetImage(string imageSourceString)
        {
            var img = System.Drawing.Image.FromStream(new MemoryStream(Convert.FromBase64String(imageSourceString)));
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(img);
            BitmapImage result = BitmapToImageSource(bmp);
            return result;
        }

        // Hàm chuyển đổi image thành một string
        public static string ImageToString(string imagePath)
        {
            int width = 150;
            int height = 150;

            var source = Bitmap.FromFile(imagePath);
            var result = (Bitmap)ResizeImageKeepAspectRatio(source, width, height);
            result.Save("../../Resources/TempFiles/avatar.jpg");

            byte[] imageArray = System.IO.File.ReadAllBytes("../../Resources/TempFiles/avatar.jpg");
            return Convert.ToBase64String(imageArray);
        }

        // Hàm chuyển đổi bitmap thành image
        public static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        // Hàm resize ảnh mà vẫn giữ tỉ lệ gốc
        public static System.Drawing.Image ResizeImageKeepAspectRatio(System.Drawing.Image source, int width, int height)
        {
            System.Drawing.Image result = null;
            try
            {
                if (source.Width != width || source.Height != height)
                {
                    // Resize image
                    float sourceRatio = (float)source.Width / source.Height;
                    using (var target = new Bitmap(width, height))
                    {
                        using (var g = System.Drawing.Graphics.FromImage(target))
                        {
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;

                            // Scaling
                            float scaling;
                            float scalingY = (float)source.Height / height;
                            float scalingX = (float)source.Width / width;
                            if (scalingX < scalingY) scaling = scalingX; else scaling = scalingY;
                            int newWidth = (int)(source.Width / scaling);
                            int newHeight = (int)(source.Height / scaling);

                            // Correct float to int rounding
                            if (newWidth < width) newWidth = width;
                            if (newHeight < height) newHeight = height;

                            // See if image needs to be cropped
                            int shiftX = 0;
                            int shiftY = 0;
                            if (newWidth > width)
                            {
                                shiftX = (newWidth - width) / 2;
                            }

                            if (newHeight > height)
                            {
                                shiftY = (newHeight - height) / 2;
                            }

                            // Draw image
                            g.DrawImage(source, -shiftX, -shiftY, newWidth, newHeight);
                        }
                        result = (System.Drawing.Image)target.Clone();
                    }
                }
                else
                {
                    // Image size matched the given size
                    result = (System.Drawing.Image)source.Clone();
                }
            }
            catch (Exception)
            {
                result = null;
            }

            return result;
        }
        #endregion

        public NhanVienViewModel()
        {
            #region Khởi tạo
            LoadListNhanVien();
            string[] DSGioiTinh = new string[] { "Nam", "Nữ" };
            ListGioiTinh = new ObservableCollection<string>(DSGioiTinh);            
            #endregion

            #region Xử lý ẩn hiện tab
            TabThongTinNhanVienCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNV = (int)ChucNangNhanVien.ThongTinNhanVien;
            });

            TabLichSuNhanVienCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ChucNangNV = (int)ChucNangNhanVien.LichSuNhanVien;
            });
            #endregion

            #region Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
              {
                  var listPhongBan = DataProvider.Ins.model.PHONGBAN.Count();
                  if (listPhongBan > 0)
                  {
                      return true;
                  }
                  MessageBox.Show("Vui lòng tạo phòng ban trước khi thêm nhân viên.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                  return false;
              }, (p) =>
              {
                  IsEditable = true;
                  ResetControls();
                  LoadListLichSuNhanVien();
                  LoadListPhongBan();

                  NhanVienWindow nhanVienWindow = new NhanVienWindow();
                  nhanVienWindow.ShowDialog();
              });
            #endregion

            #region Hiển thị command
            HienThiCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedNhanVien == null ? false : true;
            }, (p) =>
            {
                LoadListLichSuNhanVien();
                LoadListPhongBan();
                IsEditable = false;

                HoTen = SelectedNhanVien.HOTEN_NV;
                SelectedPhongBan = SelectedNhanVien.PHONGBAN;
                SelectedGioiTinh = SelectedNhanVien.GIOITINH_NV == true ? "Nữ" : "Nam";
                NgaySinh = SelectedNhanVien.NGAYSINH_NV;
                ChucVu = SelectedNhanVien.CHUCVU_NV;
                NgayVaoLam = SelectedNhanVien.NGAYVAOLAM_NV;
                Email = SelectedNhanVien.EMAIL_NV;
                SoDienThoai = SelectedNhanVien.SODIENTHOAI_NV;
                DiaChi = SelectedNhanVien.DIACHI_NV;
                Avatar = SelectedNhanVien.AVATAR_NV;
                AvatarSource = GetImage(Avatar);

                NhanVienWindow nhanVienWindow = new NhanVienWindow();
                nhanVienWindow.ShowDialog();
            });
            #endregion

            #region Huỷ command
            HuyCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi thay đổi nếu có sẽ không được lưu, bạn có chắc chắn không?", "Thông báo", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    IsEditable = false;
                    p.Close();
                }

            });
            #endregion

            #region Sửa command
            SuaCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (SelectedNhanVien == null)
                {
                    MessageBoxResult result = MessageBox.Show("Vui lòng chọn nhân viên trước khi chỉnh sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                IsEditable = true;
            });
            #endregion

            #region Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListNhanVien);
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

            #region  Search command
            SearchCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (string.IsNullOrEmpty(SearchNhanVien))
                {
                    CollectionViewSource.GetDefaultView(ListNhanVien).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListNhanVien).Filter = (searchNhanVien) =>
                    {
                        return (searchNhanVien as NHANVIEN).HOTEN_NV.IndexOf(SearchNhanVien, StringComparison.OrdinalIgnoreCase) >= 0 ||
                               (searchNhanVien as NHANVIEN).CHUCVU_NV.IndexOf(SearchNhanVien, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }
            });
            #endregion

            #region Thay ảnh command
            ThayAnhCommand = new RelayCommand<Object>((p) =>
            {
                if (IsEditable == false)
                {
                    MessageBox.Show("Vui lòng nhất nút chỉnh sửa trước khi thay đổi ảnh đại diện!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    InitialDirectory = @"C:\",
                    Title = "Chọn ảnh đại diện",

                    CheckFileExists = true,
                    CheckPathExists = true,

                    DefaultExt = "txt",
                    Filter = "Images (*.JPG;*.PNG)|*.JPG;*.PNG",
                    RestoreDirectory = true,
                    ReadOnlyChecked = true,
                    ShowReadOnly = true
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    Avatar = ImageToString(openFileDialog.FileName);
                    AvatarSource = GetImage(Avatar);                    
                }
            });
            #endregion

            #region Lưu command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(HoTen) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(SoDienThoai) ||
                string.IsNullOrEmpty(DiaChi) || string.IsNullOrEmpty(ChucVu) || SelectedPhongBan == null || SelectedGioiTinh == null ||
                NgaySinh == null || NgayVaoLam == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin nhân viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedNhanVien == null)
                {
                    var NhanVienMoi = new NHANVIEN()
                    {
                        HOTEN_NV = HoTen,
                        MA_PB = SelectedPhongBan.MA_PB,
                        GIOITINH_NV = SelectedGioiTinh == "Nữ" ? true : false,
                        NGAYSINH_NV = NgaySinh,
                        CHUCVU_NV = ChucVu,
                        NGAYVAOLAM_NV = NgayVaoLam,
                        EMAIL_NV = Email,
                        SODIENTHOAI_NV = SoDienThoai,
                        DIACHI_NV = DiaChi,
                        AVATAR_NV = Avatar,
                        TRANGTHAI_NV = true
                    };

                    DataProvider.Ins.model.NHANVIEN.Add(NhanVienMoi);
                    DataProvider.Ins.model.SaveChanges();
                    ListNhanVien.Add(NhanVienMoi);
                    MessageBox.Show("Thêm nhân viên thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                else
                {
                    LoadListLichSuNhanVien();
                    var NhanVienSua = DataProvider.Ins.model.NHANVIEN.Where(x => x.MA_NV == SelectedNhanVien.MA_NV).SingleOrDefault();

                    AddLichSuNhanVien(NhanVienSua);

                    NhanVienSua.HOTEN_NV = HoTen;
                    NhanVienSua.MA_PB = SelectedPhongBan.MA_PB;
                    NhanVienSua.GIOITINH_NV = SelectedGioiTinh == "Nữ" ? true : false;
                    NhanVienSua.NGAYSINH_NV = NgaySinh;
                    NhanVienSua.CHUCVU_NV = ChucVu;
                    NhanVienSua.NGAYVAOLAM_NV = NgayVaoLam;
                    NhanVienSua.EMAIL_NV = Email;
                    NhanVienSua.SODIENTHOAI_NV = SoDienThoai;
                    NhanVienSua.DIACHI_NV = DiaChi;
                    NhanVienSua.AVATAR_NV = Avatar;
                    NhanVienSua.TRANGTHAI_NV = true;

                    DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                p.Close();
            });
            #endregion

            #region Xoá command
            XoaCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedNhanVien == null)
                {
                    MessageBox.Show("Vui lòng chọn nhân viên trước khi xoá!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Thao tác này không thể hoàn tác! Bạn có chắc chắn xoá nhân viên này không? ", "Thông báo", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    var NhanVienSua = DataProvider.Ins.model.NHANVIEN.Where(x => x.MA_NV == SelectedNhanVien.MA_NV).SingleOrDefault();
                    NhanVienSua.TRANGTHAI_NV = false;

                    DataProvider.Ins.model.SaveChanges();
                    LoadListNhanVien();
                    p.Close();
                }

            });
            #endregion

            #region SortLichSuNV command
            SortLichSuNVCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (SelectedNhanVien == null)
                {
                    return;
                }
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListLichSuNhanVien);
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

            #region SearchLichSuNV command
            SearchLichSuNVCommand = new RelayCommand<Object>((p) => { return true; }, (p) =>
            {
                if (SelectedNhanVien == null)
                {
                    return;
                }
                if (string.IsNullOrEmpty(SearchLichSuNV))
                {
                    CollectionViewSource.GetDefaultView(ListLichSuNhanVien).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListLichSuNhanVien).Filter = (searchLichSuNV) =>
                    {
                        return (searchLichSuNV as LICHSUNHANVIEN).MOTA_LSNV.IndexOf(SearchLichSuNV, StringComparison.OrdinalIgnoreCase) >= 0 ||
                        (searchLichSuNV as LICHSUNHANVIEN).THOIGIAN_LSNV.ToString().IndexOf(SearchLichSuNV, StringComparison.OrdinalIgnoreCase) >= 0;
                    };
                }
            });
            #endregion
        }

        #region Các hàm hỗ trợ
        public void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN.Where(x => x.TRANGTHAI_NV == true));
        }

        public void LoadListPhongBan()
        {
            ListPhongBan = new ObservableCollection<PHONGBAN>(DataProvider.Ins.model.PHONGBAN);
        }

        public void LoadListLichSuNhanVien()
        {
            if (SelectedNhanVien == null)
            {
                ListLichSuNhanVien=null;
            }
            else
            {
                ListLichSuNhanVien = new ObservableCollection<LICHSUNHANVIEN>(DataProvider.Ins.model.LICHSUNHANVIEN.Where(x => x.MA_NV == SelectedNhanVien.MA_NV));
            }
            
        }

        public void AddLichSuNhanVien(NHANVIEN nv)
        {
            //Thêm lịch sử chức vụ
            string mota = "";
            if (nv.CHUCVU_NV != ChucVu)
                mota = "Thay đổi chức vụ từ " + nv.CHUCVU_NV + " thành " + ChucVu;

            if (mota != "")
            {
                var LichSuNhanVienMoi = new LICHSUNHANVIEN()
                {
                    MA_NV = nv.MA_NV,
                    MOTA_LSNV = mota,
                    THOIGIAN_LSNV = DateTime.Now
                };

                DataProvider.Ins.model.LICHSUNHANVIEN.Add(LichSuNhanVienMoi);
                DataProvider.Ins.model.SaveChanges();
            }

            mota = "";
            //Thêm lịch sử phòng ban
            if (nv.MA_PB != SelectedPhongBan.MA_PB)
                mota = "Thay đổi phòng ban từ " + nv.PHONGBAN.TEN_PB + " thành " + SelectedPhongBan.TEN_PB;

            if (mota != "")
            {
                var LichSuNhanVienMoi = new LICHSUNHANVIEN()
                {
                    MA_NV = nv.MA_NV,
                    MOTA_LSNV = mota,
                    THOIGIAN_LSNV = DateTime.Now
                };

                DataProvider.Ins.model.LICHSUNHANVIEN.Add(LichSuNhanVienMoi);
                DataProvider.Ins.model.SaveChanges();
            }


        }

        public void ResetControls()
        {
            SelectedNhanVien = null;
            HoTen = null;
            SelectedPhongBan = null;
            SelectedGioiTinh = null;
            NgaySinh = null;
            ChucVu = null;
            NgayVaoLam = null;
            Email = null;
            SoDienThoai = null;
            DiaChi = null;
            Avatar = ImageToString("../../Resources/Icons/default_user.png");
            AvatarSource = GetImage(Avatar);
        }
        #endregion
    }

}

