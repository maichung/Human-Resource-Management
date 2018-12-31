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
    class ChiPhiViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<ThongTinPhieuChi> _ListThongTinPhieuChi;
        public ObservableCollection<ThongTinPhieuChi> ListThongTinPhieuChi { get => _ListThongTinPhieuChi; set { _ListThongTinPhieuChi = value; OnPropertyChanged(); } }

        #endregion 

        #region Combobox item sources 
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }
                     
        #endregion

        #region Thuộc tính binding


        private ThongTinPhieuChi _SelectedThongTinPhieuChi;
        public ThongTinPhieuChi SelectedThongTinPhieuChi { get => _SelectedThongTinPhieuChi; set { _SelectedThongTinPhieuChi = value; OnPropertyChanged(); } }


        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }


        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        private string _TriGia;
        public string TriGia { get => _TriGia; set { _TriGia = value; OnPropertyChanged(); } }

        private string _ThoiGianLap;
        public string ThoiGianLap { get => _ThoiGianLap; set { _ThoiGianLap = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        private string _SearchPhieuChi;
        public string SearchPhieuChi { get => _SearchPhieuChi; set { _SearchPhieuChi = value;OnPropertyChanged(); } }

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

        public ChiPhiViewModel()
        {
            LoadListThongTinPhieuChi();
            LoadListNhanVien();
            IsEditable = false;


            
            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
             {
                 return true;
             }, (p) =>
             {
                 IsEditable = true;
                 ResetControls();
                 SelectedThongTinPhieuChi = null;

                 ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                 var chiTietPhieuChiViewModel = chiTietPhieuChiWindow.DataContext as ChiTietPhieuChiViewModel;
                 chiTietPhieuChiViewModel.SelectedThongTinPhieuChi = null;
                 chiTietPhieuChiViewModel.ReloadListChiTietPhieuChi();
                 chiTietPhieuChiWindow.Close();

                 PhieuChiWindow phieuChiWindow = new PhieuChiWindow();
                 phieuChiWindow.ShowDialog();
             });

    
            // Xóa phiếu chi
            XoaCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedThongTinPhieuChi == null)
                {
                    MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;                        
                }
                return true;
            }, (p) =>
            {
            
                MessageBoxResult result = MessageBox.Show("Xác nhận xóa phiếu chi?", "Xóa phiếu chi", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                {
                    try
                    {
                            ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                            var chiTietPhieuChiVM = chiTietPhieuChiWindow.DataContext as ChiTietPhieuChiViewModel;
                            chiTietPhieuChiWindow.Close();

                            foreach (CHITIETPHIEUCHI ctpc in chiTietPhieuChiVM.ListChiTietPhieuChi)
                            {
                                DataProvider.Ins.model.CHITIETPHIEUCHI.Remove(ctpc);
                            }
                            var pc = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedThongTinPhieuChi.PhieuChi.MA_PC).FirstOrDefault();
                        DataProvider.Ins.model.PHIEUCHI.Remove(pc);
                        DataProvider.Ins.model.SaveChanges();
                        transactions.Commit();
                            LoadListThongTinPhieuChi();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            
                            p.Close();

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                        transactions.Rollback();
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
                 if (SelectedNhanVien==null)
                  {
                      MessageBox.Show("Chưa chọn nhân viên cho phiếu chi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
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
                  if (SelectedThongTinPhieuChi==null)
                  {
                 
                      var PhieuChiMoi = new PHIEUCHI()
                      {
                          MA_NV = SelectedNhanVien.MA_NV,
                          TRIGIA_PC = 0,
                          THOIGIANLAP_PC = DateTime.UtcNow
                      };
                      DataProvider.Ins.model.PHIEUCHI.Add(PhieuChiMoi);
                      DataProvider.Ins.model.SaveChanges();
                      MessageBox.Show("Thêm phiếu chi mới thành công!","Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                      
                      ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();

                      var chiTietPhieuChiVM = chiTietPhieuChiWindow.DataContext as ChiTietPhieuChiViewModel;
                      SelectedThongTinPhieuChi = new ThongTinPhieuChi()
                      {
                          PhieuChi = PhieuChiMoi,
                          NhanVien = SelectedNhanVien
                      };
                      chiTietPhieuChiVM.SelectedThongTinPhieuChi = SelectedThongTinPhieuChi;
                      chiTietPhieuChiVM.ReloadListChiTietPhieuChi();
                      chiTietPhieuChiWindow.Close();
                      IsEditable = false;
                      LoadListThongTinPhieuChi();
                  }
                  else
                  {
                      var PhieuChiSua = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedThongTinPhieuChi.PhieuChi.MA_PC).SingleOrDefault();
                      PhieuChiSua.THOIGIANLAP_PC = DateTime.UtcNow;
                      PhieuChiSua.MA_NV = SelectedNhanVien.MA_NV;
                      DataProvider.Ins.model.SaveChanges();
                      MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                      LoadListThongTinPhieuChi();
                      IsEditable = false;
                      p.Close();
                  }                
                  
              });
              

            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListThongTinPhieuChi);
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
                if (string.IsNullOrEmpty(SearchPhieuChi))
                {
                    CollectionViewSource.GetDefaultView(ListThongTinPhieuChi).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListThongTinPhieuChi).Filter = (searchThongTinPhieuChi) =>
                    {
                        return (searchThongTinPhieuChi as ThongTinPhieuChi).NhanVien.HOTEN_NV.ToString().IndexOf(SearchPhieuChi, StringComparison.OrdinalIgnoreCase) >= 0;
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
                  if (SelectedThongTinPhieuChi == null)
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
                  return SelectedThongTinPhieuChi == null ? false : true;
              }, (p) =>
             {
                 IsEditable = false;
                 PhieuChiWindow phieuChiWindow = new PhieuChiWindow();

                 SelectedNhanVien = SelectedThongTinPhieuChi.NhanVien;
                 TriGia = SelectedThongTinPhieuChi.PhieuChi.TRIGIA_PC.ToString();
                 ThoiGianLap = SelectedThongTinPhieuChi.PhieuChi.THOIGIANLAP_PC.ToString();

                 ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                 var chiTietPhieuChiVM = chiTietPhieuChiWindow.DataContext as ChiTietPhieuChiViewModel;
                 chiTietPhieuChiVM.SelectedThongTinPhieuChi = SelectedThongTinPhieuChi;                              
                 chiTietPhieuChiVM.ReloadListChiTietPhieuChi();
                 chiTietPhieuChiWindow.Close();

                 phieuChiWindow.ShowDialog();
             });
            
        }

        void LoadListThongTinPhieuChi()
        {
            ListThongTinPhieuChi = new ObservableCollection<ThongTinPhieuChi>();
            var query = from nv in DataProvider.Ins.model.NHANVIEN
                        join pc in DataProvider.Ins.model.PHIEUCHI
                        on nv.MA_NV equals pc.MA_NV
                        select new ThongTinPhieuChi()
                        {
                            NhanVien = nv,
                            PhieuChi = pc
                        };
            foreach (ThongTinPhieuChi item in query)
            {
                ListThongTinPhieuChi.Add(item);
            }
        }
        void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN);
           
        }

        

        public void ResetControls()
        {
            TriGia = 0.ToString(); ;
           ThoiGianLap = null;
            SelectedNhanVien = null;
        }



    }
}
