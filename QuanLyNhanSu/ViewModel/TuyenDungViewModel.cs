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
    class TuyenDungViewModel : BaseViewModel
    {
        #region Phiếu chi ViewModel
        #region DataContext
        private ObservableCollection<UNGVIEN> _ListUngVien;
        public ObservableCollection<UNGVIEN> ListUngVien { get => _ListUngVien; set { _ListUngVien = value; OnPropertyChanged(); } }

        #endregion

        #region Combobox item sources
        private ObservableCollection<string> _ListGioiTinh;
        public ObservableCollection<string> ListGioiTinh { get => _ListGioiTinh; set { _ListGioiTinh = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding


        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged(); } }

        private string _SelectedGioiTinh;
        public string SelectedGioiTinh { get => _SelectedGioiTinh; set { _SelectedGioiTinh = value; OnPropertyChanged(); } }

        private DateTime? _NgaySinh;
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value; OnPropertyChanged(); } }

        public string _Email;
        public string Email { get => _Email; set { _Email = value; OnPropertyChanged(); } }

        public string _SDT;
        public string SDT { get => _SDT; set { _SDT = value; OnPropertyChanged(); } }

        public string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value; OnPropertyChanged(); } }

        private UNGVIEN _SelectedUngVien;
        public UNGVIEN SelectedUngVien { get => _SelectedUngVien; set { _SelectedUngVien = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }


        #endregion

        #region Thuộc tính khác
        private string _SearchUngVien;
        public string SearchUngVien { get => _SearchUngVien; set { _SearchUngVien = value; OnPropertyChanged(); } }

        public bool sort;
        #endregion

        #region Command binding
        public ICommand TaoMoiCommand { get; set; }
        public ICommand LuuCommand { get; set; }
        public ICommand HuyCommand { get; set; }
        public ICommand SuaCommand { get; set; }
        public ICommand HienThiCommand { get; set; }
        public ICommand SortCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand XoaCommand { get; set; }
        #endregion

        public TuyenDungViewModel()
        {
            LoadListUngVien();
            string[] DSGioiTinh = new string[] { "Nam", "Nữ" };
            ListGioiTinh = new ObservableCollection<string>(DSGioiTinh);
            IsEditable = false;
            string[] DSTrangThai = new string[] { "Chưa xử lý", "Chấp nhận", "Từ chối" };
            ListTrangThai_HSUT = new ObservableCollection<string>(DSTrangThai);

            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
             {
                 return true;
             }, (p) =>
             {
                 IsEditable = true;
                 ResetControls();
                 SelectedUngVien = null;          
                 ReloadListHoSoUngTuyen();
                 UngVienWindow ungVienWindow = new UngVienWindow();
                 ungVienWindow.ShowDialog();
             });


            // Xóa ứng viên
            XoaCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedUngVien == null)
                {
                    MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa ứng viên và các hồ sơ ứng tuyển của ứng viên?", "Xóa phiếu chi", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                    {
                        {
                            try
                            {
                                foreach (HOSOUNGTUYEN hsut in ListHoSoUngTuyen)
                                {
                                    DataProvider.Ins.model.HOSOUNGTUYEN.Remove(hsut);
                                }
                                var uv = DataProvider.Ins.model.UNGVIEN.Where(x => x.MA_UV == SelectedUngVien.MA_UV).FirstOrDefault();
                                DataProvider.Ins.model.UNGVIEN.Remove(uv);
                                DataProvider.Ins.model.SaveChanges();
                                transactions.Commit();
                                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                                LoadListUngVien();
                                p.Close();
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                                transactions.Rollback();
                            }
                        }
                    }
                }
                else
                {
                    return;

                }
            });
            //Lưu Command
            LuuCommand = new RelayCommand<Window>((p) =>
              {
                  if (string.IsNullOrEmpty(HoTen)
                   || string.IsNullOrEmpty(Email)
                   || string.IsNullOrEmpty(SDT)
                   || string.IsNullOrEmpty(DiaChi)
                   || SelectedGioiTinh == null
                   || NgaySinh == null)
                  {
                      MessageBox.Show("Vui lòng nhập đầy đủ thông tin ứng viên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                   // Thêm mới ứng viên và các hồ sơ ứng tuyển
                   if (SelectedUngVien == null)
                   {
                       var ungVienMoi = new UNGVIEN()
                       {
                           HOTEN_UV = HoTen,
                           NGAYSINH_UV = NgaySinh,
                           GIOITINH_UV = SelectedGioiTinh == "Nữ" ? true : false,
                           EMAIL_UV = Email,
                           SODIENTHOAI_UV = SDT,
                           DIACHI_UV = DiaChi
                       };
                       
                       //Thêm ứng viên trong model
                       DataProvider.Ins.model.UNGVIEN.Add(ungVienMoi);
                       DataProvider.Ins.model.SaveChanges();

                       foreach (HOSOUNGTUYEN x in ListHoSoUngTuyen)
                       {
                           x.MA_UV = ungVienMoi.MA_UV;
                           DataProvider.Ins.model.HOSOUNGTUYEN.Add(x);
                       }

                       DataProvider.Ins.model.SaveChanges();
                       MessageBox.Show("Thêm ứng viên mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                   }

                   //Chỉnh sửa ứng viên đã có
                   else
                   {
                       var UngvienSua = DataProvider.Ins.model.UNGVIEN.Where(x => x.MA_UV == SelectedUngVien.MA_UV).SingleOrDefault();
                       UngvienSua.HOTEN_UV = HoTen;
                       UngvienSua.GIOITINH_UV = SelectedGioiTinh == "Nữ" ? true : false;
                       UngvienSua.NGAYSINH_UV = NgaySinh;
                       UngvienSua.EMAIL_UV = Email;
                       UngvienSua.SODIENTHOAI_UV = SDT;
                       UngvienSua.DIACHI_UV = DiaChi;

                       DataProvider.Ins.model.SaveChanges();
                       MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                   }
                   LoadListUngVien();
                   p.Close();

               });


            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListUngVien);
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
                if (string.IsNullOrEmpty(SearchUngVien))
                {
                    CollectionViewSource.GetDefaultView(ListUngVien).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListUngVien).Filter = (searchUngVien) =>
                    {
                        return (searchUngVien as UNGVIEN).HOTEN_UV.ToString().IndexOf(SearchUngVien, StringComparison.OrdinalIgnoreCase) >= 0;
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
                     UnchangedAllActions();
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
                  return SelectedUngVien == null ? false : true;
              }, (p) =>
             {
                 IsEditable = false;
                 HoTen = SelectedUngVien.HOTEN_UV;
                 SelectedGioiTinh = SelectedUngVien.GIOITINH_UV == true ? "Nữ" : "Nam";
                 NgaySinh = SelectedUngVien.NGAYSINH_UV;
                 Email = SelectedUngVien.EMAIL_UV;
                 SDT = SelectedUngVien.SODIENTHOAI_UV;
                 DiaChi = SelectedUngVien.DIACHI_UV;
                 ReloadListHoSoUngTuyen();

                 UngVienWindow ungVienWindow = new UngVienWindow();
                 ungVienWindow.ShowDialog();
             });



            /* --------------------------------------------------------------------------------------*/


            //Xóa hồ sơ ứng tuyển command
            Xoa_HSUTCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedHoSoUngTuyen == null)
                {
                    MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa hồ sơ ứng tuyển", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (SelectedUngVien != null)
                    {
                        //Xóa trên model
                        DataProvider.Ins.model.HOSOUNGTUYEN.Remove(SelectedHoSoUngTuyen);

                        //Xóa trên hiển thị
                        ListHoSoUngTuyen.Remove(SelectedHoSoUngTuyen);
                    }
                    else
                    {
                        //Xóa trên hiển thị
                        ListHoSoUngTuyen.Remove(SelectedHoSoUngTuyen);
                    }
                    p.Close();
                }
                else return;

            });

            //Tạo mới hồ sơ ứng tuyển command
            TaoMoi_HSUTCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                ResetControls_HSUT();
                IsEditable_HSUT = true;
                SelectedHoSoUngTuyen = null;
                

                HoSoUngTuyenWindow hoSoUngTuyen = new HoSoUngTuyenWindow();
                hoSoUngTuyen.ShowDialog();

            });

            //Lưu hồ sơ ứng tuyển Command
            Luu_HSUTCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(ViTriCongViec)
               || SelectedTrangThai == null
               || NgayNop == null
               || CV_HoSoUngTuyen == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin hồ sơ ứng tuyển!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (IsEditable_HSUT == false)
                {
                    MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
           {
               if (SelectedHoSoUngTuyen == null)    //Trường hợp thêm mới
               {
                   {
                       // Thêm hồ sơ ứng tuyển vào ứng viên đang tạo
                       if (SelectedUngVien == null)
                       {
                           var HSUTMoi = new HOSOUNGTUYEN()
                           {
                               VITRICONGVIEC_HSUT = ViTriCongViec,
                               TRANGTHAI_HSUT = SelectedTrangThai,
                               NGAYNOP_HSUT = NgayNop,
                               MA_UV = -1,
                               CV_HSUT = CV_HoSoUngTuyen,
                           };
                           //Thêm hồ sơ ứng tuyển hiển thị
                           ListHoSoUngTuyen.Add(HSUTMoi);

                           MessageBox.Show("Thêm hồ sơ ứng tuyển mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                       }

                       // Thêm chi tiết phiếu chi vào phiếu chi đã có
                       else
                       {
                           var HSUTMoi = new HOSOUNGTUYEN()
                           {
                               VITRICONGVIEC_HSUT = ViTriCongViec,
                               TRANGTHAI_HSUT = SelectedTrangThai,
                               NGAYNOP_HSUT = NgayNop,
                               MA_UV = SelectedUngVien.MA_UV,
                               CV_HSUT = CV_HoSoUngTuyen,
                           };
                           //Thêm hồ sơ ứng tuyển hiển thị
                           ListHoSoUngTuyen.Add(HSUTMoi);

                           //Thêm hồ sơ ứng tuyển vào model
                           DataProvider.Ins.model.HOSOUNGTUYEN.Add(HSUTMoi);


                           MessageBox.Show("Thêm hồ sơ ứng tuyển mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                       }
                       p.Close();
                   }
               }
               else if (SelectedHoSoUngTuyen != null)   //Trường hợp chỉnh sửa chi tiết phiếu chi
               {
                   {
                       //Cật nhật hiển thị
                       SelectedHoSoUngTuyen.VITRICONGVIEC_HSUT = ViTriCongViec;
                       SelectedHoSoUngTuyen.TRANGTHAI_HSUT = SelectedTrangThai;
                       SelectedHoSoUngTuyen.NGAYNOP_HSUT = NgayNop;
                       SelectedHoSoUngTuyen.CV_HSUT = CV_HoSoUngTuyen;

                       if (SelectedUngVien!=null)
                       {
                           //Cập nhật model
                           var HSUTSua = DataProvider.Ins.model.HOSOUNGTUYEN.Where(x => x.MA_HSUT == SelectedHoSoUngTuyen.MA_HSUT).SingleOrDefault();
                           HSUTSua.VITRICONGVIEC_HSUT = ViTriCongViec;
                           HSUTSua.TRANGTHAI_HSUT = SelectedTrangThai;
                           HSUTSua.NGAYNOP_HSUT = NgayNop;
                           HSUTSua.CV_HSUT = CV_HoSoUngTuyen;
                       }
                       MessageBox.Show("Sửa chi tiết phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                       p.Close();
                   }
               }

           });

            //Sort hồ sơ ứng tuyển command
            Sort_HSUTCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListHoSoUngTuyen);
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

            //Hủy hồ sơ ứng tuyểncommand
            Huy_HSUTCommand = new RelayCommand<Window>((p) =>
            {
                return true;
            }, (p) =>
            {
                MessageBoxResult result = MessageBox.Show("Mọi chỉnh sửa sẽ không được lưu\nXác nhận hủy??", "Xác nhận hủy", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    p.Close();                   
                }
                else return;
            });

            //Sửa hồ sơ ứng tuyển Command
            Sua_HSUTCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedHoSoUngTuyen == null)
                {
                    MessageBox.Show("Không thể sửa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                IsEditable_HSUT = true;
            }
            );

            //Hiển thị hồ sơ ứng tuyển Command
            HienThi_HSUTCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedHoSoUngTuyen == null ? false : true;
              
            }, (p) =>
            {

                IsEditable = false;
                ViTriCongViec = SelectedHoSoUngTuyen.VITRICONGVIEC_HSUT;
                SelectedTrangThai = SelectedHoSoUngTuyen.TRANGTHAI_HSUT;
                NgayNop = SelectedHoSoUngTuyen.NGAYNOP_HSUT;
                CV_HoSoUngTuyen = SelectedHoSoUngTuyen.CV_HSUT;
                TenFileCV = "CV.pdf";


                IsEditable_HSUT = false;

                HoSoUngTuyenWindow hoSoUngTuyenWindow = new HoSoUngTuyenWindow();
                hoSoUngTuyenWindow.ShowDialog();
            });

            // Chọn file command
            ChonFile_HSUTCommand = new RelayCommand<Object>((p) =>
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
                    CV_HoSoUngTuyen = FileToBinaryString(openFileDialog.FileName);
                }
            });

            // Xem file command
            XemFile_HSUTCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                try
                {
                    Process.Start(BinaryStringToFile(CV_HoSoUngTuyen));
                }
                catch (Exception)
                {
                    MessageBox.Show("Đã xảy ra lỗi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

            });

        }

        void LoadListUngVien()
        {
            ListUngVien = new ObservableCollection<UNGVIEN>(DataProvider.Ins.model.UNGVIEN);
        }


        public void ResetControls()
        {
            HoTen = null;
            SelectedGioiTinh = null;
            NgaySinh = null;
            Email = null;

            SDT = null;
            DiaChi = null;
        }


        #endregion

        

        #region Hồ sơ ứng tuyển
        #region DataContext
        private ObservableCollection<HOSOUNGTUYEN> _ListHoSoUngTuyen;
        public ObservableCollection<HOSOUNGTUYEN> ListHoSoUngTuyen { get => _ListHoSoUngTuyen; set { _ListHoSoUngTuyen = value; OnPropertyChanged(); } }
        #endregion

        #region Combobox item sources
        private ObservableCollection<string> _ListTrangThai_HSUT;
        public ObservableCollection<string> ListTrangThai_HSUT { get => _ListTrangThai_HSUT; set { _ListTrangThai_HSUT = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _ViTriCongViec;
        public string ViTriCongViec { get => _ViTriCongViec; set { _ViTriCongViec = value; OnPropertyChanged(); } }

        private string _SelectedTrangThai;
        public string SelectedTrangThai { get => _SelectedTrangThai; set { _SelectedTrangThai = value; OnPropertyChanged(); } }

        private DateTime? _NgayNop;
        public DateTime? NgayNop { get => _NgayNop; set { _NgayNop = value; OnPropertyChanged(); } }


        private HOSOUNGTUYEN _SelectedHoSoUngTuyen;
        public HOSOUNGTUYEN SelectedHoSoUngTuyen { get => _SelectedHoSoUngTuyen; set { _SelectedHoSoUngTuyen = value; OnPropertyChanged(); } }

        private bool _IsEditable_HSUT;
        public bool IsEditable_HSUT { get => _IsEditable_HSUT; set { _IsEditable_HSUT = value; OnPropertyChanged(); } }

        private byte[] _CV_HoSoUngTuyen;
        public byte[] CV_HoSoUngTuyen { get => _CV_HoSoUngTuyen; set { _CV_HoSoUngTuyen = value; OnPropertyChanged(); } }

        private string _TenFileCV;
        public string TenFileCV { get => _TenFileCV; set { _TenFileCV = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính khác
       

        public bool sort_HSUT;
        #endregion

        #region Command binding
        public ICommand TaoMoi_HSUTCommand { get; set; }
        public ICommand Luu_HSUTCommand { get; set; }
        public ICommand Huy_HSUTCommand { get; set; }
        public ICommand Sua_HSUTCommand { get; set; }
        public ICommand HienThi_HSUTCommand { get; set; }
        public ICommand Sort_HSUTCommand { get; set; }
        public ICommand Search_HSUTCommand { get; set; }
        public ICommand Xoa_HSUTCommand { get; set; }
        public ICommand ChonFile_HSUTCommand { get; set; }
        public ICommand XemFile_HSUTCommand { get; set; }
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

        void LoadListHoSoUngTuyen()
        {
            ListHoSoUngTuyen = new ObservableCollection<HOSOUNGTUYEN>(DataProvider.Ins.model.HOSOUNGTUYEN);

        }
        public void ReloadListHoSoUngTuyen()
        {
            if (SelectedUngVien == null)
            {
                ListHoSoUngTuyen = new ObservableCollection<HOSOUNGTUYEN>();
                return;

            }
            else
                ListHoSoUngTuyen = new ObservableCollection<HOSOUNGTUYEN>(DataProvider.Ins.model.HOSOUNGTUYEN.Where(x => x.MA_UV == SelectedUngVien.MA_UV));
        }

        public void ResetControls_HSUT()
        {
            ViTriCongViec = null;
            NgayNop = null;
            CV_HoSoUngTuyen = null;
            TenFileCV = null;
        }

        private void UnchangedAllActions()
        {
            foreach (HOSOUNGTUYEN x in DataProvider.Ins.model.HOSOUNGTUYEN)
            {              
                if (DataProvider.Ins.model.Entry(x).State != System.Data.Entity.EntityState.Unchanged)
                        DataProvider.Ins.model.Entry(x).Reload();
            }           
        }
        #endregion
    }
}
