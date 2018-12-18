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
    class UngVienViewModel: BaseViewModel
    {
        #region DataContext
        private ObservableCollection<UNGVIEN> _ListUngVien;
        public ObservableCollection<UNGVIEN> ListUngVien { get => _ListUngVien; set { _ListUngVien = value; OnPropertyChanged(); } }

        private ObservableCollection<HOSOUNGTUYEN> _ListHSUT;
        public ObservableCollection<HOSOUNGTUYEN> ListHSUT { get => _ListHSUT; set { _ListHSUT = value; OnPropertyChanged(); } }
        #endregion 

        #region Combobox item sources
        private ObservableCollection<string> _ListGioiTinh;
        public ObservableCollection<string> ListGioiTinh { get => _ListGioiTinh; set { _ListGioiTinh = value; OnPropertyChanged(); } }
        #endregion

        #region Thuộc tính binding
        private string _HoTen;
        public string HoTen { get => _HoTen; set { _HoTen = value; OnPropertyChanged();  } }

        private string _SelectedGioiTinh;
        public string SelectedGioiTinh { get => _SelectedGioiTinh; set { _SelectedGioiTinh = value;OnPropertyChanged(); } }

        private DateTime? _NgaySinh;
        public DateTime? NgaySinh { get => _NgaySinh; set { _NgaySinh = value;OnPropertyChanged(); } }

        public string _Email;
        public string Email { get => _Email; set { _Email = value;OnPropertyChanged(); } }

        public string _SDT;
        public string SDT { get => _SDT; set { _SDT = value;OnPropertyChanged(); } }

        public string _DiaChi;
        public string DiaChi { get => _DiaChi; set { _DiaChi = value;OnPropertyChanged(); } }

        private UNGVIEN _SelectedUngVien;
        public UNGVIEN SelectedUngVien { get => _SelectedUngVien; set { _SelectedUngVien = value; OnPropertyChanged(); } }

        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        private string _SearchUngVien;
        public string SearchUngVien { get => _SearchUngVien; set { _SearchUngVien = value;OnPropertyChanged(); } }

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
        public ICommand XoaUngVienCommand { get; set; }
        #endregion

        public UngVienViewModel()
        {
            LoadListUngVien();
            string[] DSGioiTinh = new string[] { "Nam", "Nữ" };
            ListGioiTinh = new ObservableCollection<string>(DSGioiTinh);
            IsEditable = false;

            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
             {
                 return true;
             }, (p) =>
             {
                 IsEditable = true;
                 ResetControls();
                 SelectedUngVien = null;

                 HoSoUngTuyen hoSoUngTuyen = new HoSoUngTuyen();
                 var hoSoUngTuyenVM = hoSoUngTuyen.DataContext as HoSoUngTuyenViewModel;
                 hoSoUngTuyenVM.SelectedUngVien = SelectedUngVien;
                 // MessageBox.Show(hoSoUngTuyenVM.SelectedUngVien.HOTEN_UV);
                 UngVienWindow ungVienWindow = new UngVienWindow();
                 hoSoUngTuyenVM.ReloadListHSUT();
                 hoSoUngTuyen.Close();
                 ungVienWindow.ShowDialog();
             });


            //Ứng viên
            XoaUngVienCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedUngVien == null)
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
                        var uv = DataProvider.Ins.model.UNGVIEN.Where(x => x.MA_UV == SelectedUngVien.MA_UV).FirstOrDefault();
                        DataProvider.Ins.model.UNGVIEN.Remove(uv);
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
                    LoadListUngVien();

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
                  || NgaySinh == null )
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
              },(p)=>
              {
                  if (SelectedUngVien==null)
                  {
                      var UngVienMoi = new UNGVIEN()
                      {
                          HOTEN_UV = HoTen,
                          NGAYSINH_UV = NgaySinh,
                          GIOITINH_UV = SelectedGioiTinh == "Nữ" ? true : false,
                          EMAIL_UV = Email,
                          SODIENTHOAI_UV = SDT,
                          DIACHI_UV = DiaChi
                      };
                      DataProvider.Ins.model.UNGVIEN.Add(UngVienMoi);
                      DataProvider.Ins.model.SaveChanges();
                      MessageBox.Show("Thêm ứng viên mới thành công!","Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                  }
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
                        return (searchUngVien as UNGVIEN).HOTEN_UV.IndexOf(SearchUngVien, StringComparison.OrdinalIgnoreCase) >= 0;
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
                  return SelectedUngVien == null ? false : true;
              }, (p) =>
             {
                 IsEditable = false;
                 HoTen = SelectedUngVien.HOTEN_UV;
                 SelectedGioiTinh =SelectedUngVien.GIOITINH_UV==true?"Nữ":"Nam";
                 NgaySinh=SelectedUngVien.NGAYSINH_UV;
                 Email=SelectedUngVien.EMAIL_UV;
                 SDT=SelectedUngVien.SODIENTHOAI_UV;
                 DiaChi = SelectedUngVien.DIACHI_UV;


                HoSoUngTuyen hoSoUngTuyen = new HoSoUngTuyen();     
                 var hoSoUngTuyenVM = hoSoUngTuyen.DataContext as HoSoUngTuyenViewModel;
                 hoSoUngTuyenVM.SelectedUngVien = SelectedUngVien;
                // MessageBox.Show(hoSoUngTuyenVM.SelectedUngVien.HOTEN_UV);
                 UngVienWindow ungVienWindow = new UngVienWindow();
                 hoSoUngTuyenVM.ReloadListHSUT();
                 hoSoUngTuyen.Close();
                 ungVienWindow.ShowDialog();
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


    }
}
