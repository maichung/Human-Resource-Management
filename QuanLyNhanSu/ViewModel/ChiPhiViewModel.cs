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
        #region Phiếu chi ViewModel
        #region DataContext
        private ObservableCollection<PHIEUCHI> _ListPhieuChi;
        public ObservableCollection<PHIEUCHI> ListPhieuChi { get => _ListPhieuChi; set { _ListPhieuChi = value; OnPropertyChanged(); } }

        #endregion 

        #region Combobox item sources 
        private ObservableCollection<NHANVIEN> _ListNhanVien;
        public ObservableCollection<NHANVIEN> ListNhanVien { get => _ListNhanVien; set { _ListNhanVien = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính binding


        private PHIEUCHI _SelectedPhieuChi;
        public PHIEUCHI SelectedPhieuChi { get => _SelectedPhieuChi; set { _SelectedPhieuChi = value; OnPropertyChanged(); } }


        private NHANVIEN _SelectedNhanVien;
        public NHANVIEN SelectedNhanVien { get => _SelectedNhanVien; set { _SelectedNhanVien = value; OnPropertyChanged(); } }


        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        private long _TriGia;
        public long TriGia { get => _TriGia; set { _TriGia = value; OnPropertyChanged(); } }

        private string _ThoiGianLap;
        public string ThoiGianLap { get => _ThoiGianLap; set { _ThoiGianLap = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        private string _SearchPhieuChi;
        public string SearchPhieuChi { get => _SearchPhieuChi; set { _SearchPhieuChi = value; OnPropertyChanged(); } }

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

        public ICommand ClosedCommand { get; set; }
        #endregion

        public ChiPhiViewModel()
        {
            LoadListPhieuChi();
            LoadListNhanVien();
            IsEditable = false;

            //Dong Window command
            ClosedCommand = new RelayCommand<Object>((p) =>
            {
                return true;
            }, (p) =>
            {
                UnchangedAllActions();
            });


            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
             {
                 return true;
             }, (p) =>
             {
                

                 IsEditable = true;
                 ResetControls();
                 SelectedPhieuChi = null;
                 ReloadListChiTietPhieuChi();
                 PhieuChiWindow phieuChiWindow = new PhieuChiWindow();
                 phieuChiWindow.ShowDialog();
             });


            // Xóa phiếu chi
            XoaCommand = new RelayCommand<Window>((p) =>
            {

                if (SelectedPhieuChi == null)
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
                    {
                        try
                        {
                            foreach (CHITIETPHIEUCHI ctpc in ListChiTietPhieuChi)
                            {
                                DataProvider.Ins.model.CHITIETPHIEUCHI.Remove(ctpc);
                            }
                            var pc = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedPhieuChi.MA_PC).FirstOrDefault();
                            DataProvider.Ins.model.PHIEUCHI.Remove(pc);
                            DataProvider.Ins.model.SaveChanges();
                            transactions.Commit();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadListPhieuChi();
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
                  if (SelectedNhanVien == null)
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
              }, (p) =>
               {

                   // Thêm mới phiếu chi và các chi tiết phiếu chi
                    if (SelectedPhieuChi==null)
                   {
                       var PhieuChiMoi = new PHIEUCHI()
                       {
                           NHANVIEN = SelectedNhanVien,
                           TRIGIA_PC = (decimal?)TriGia,
                           THOIGIANLAP_PC = DateTime.Now,
 
                       };
                       DataProvider.Ins.model.PHIEUCHI.Add(PhieuChiMoi);
                       DataProvider.Ins.model.SaveChanges();
                       
            
                       foreach (CHITIETPHIEUCHI x in ListChiTietPhieuChi)
                       {
                           
                               x.MA_PC = PhieuChiMoi.MA_PC;
                           DataProvider.Ins.model.CHITIETPHIEUCHI.Add(x);
                       }
                       DataProvider.Ins.model.SaveChanges();
                       MessageBox.Show("Thêm phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                      
                   }

                    //Chỉnh sửa phiếu chi đã có
                    else
                   {
                       var PhieuChiSua = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedPhieuChi.MA_PC).SingleOrDefault();
                       PhieuChiSua.NHANVIEN = SelectedNhanVien;
                       PhieuChiSua.TRIGIA_PC = (decimal?)TriGia;
                       DataProvider.Ins.model.SaveChanges();
                       MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                      
                   }
                   LoadListPhieuChi();
                   p.Close();
               });


            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListPhieuChi);
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
                    CollectionViewSource.GetDefaultView(ListPhieuChi).Filter = (all) => { return true; };
                }
                else
                {
                    CollectionViewSource.GetDefaultView(ListPhieuChi).Filter = (searchPhieuChi) =>
                    {
                        return (searchPhieuChi as PHIEUCHI).NHANVIEN.HOTEN_NV.ToString().IndexOf(SearchPhieuChi, StringComparison.OrdinalIgnoreCase) >= 0;
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
                  if (SelectedPhieuChi == null)
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
                  return SelectedPhieuChi == null ? false : true;
              }, (p) =>
             {
                 

                 IsEditable = false;
                 SelectedNhanVien = SelectedPhieuChi.NHANVIEN;
                 TriGia = (long)SelectedPhieuChi.TRIGIA_PC;
                 ThoiGianLap = SelectedPhieuChi.THOIGIANLAP_PC.ToString();
                 PhieuChiWindow phieuChiWindow = new PhieuChiWindow();
                 ReloadListChiTietPhieuChi();
                 phieuChiWindow.ShowDialog();
             });



            /* --------------------------------------------------------------------------------------*/


            //Xóa Chi tiết phiếu chi command
            Xoa_CTPCCommand = new RelayCommand<Window>((p) =>
            {
                if (SelectedChiTietPhieuChi == null)
                {
                    MessageBox.Show("Không thể xóa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {

            MessageBoxResult result = MessageBox.Show("Xác nhận xóa?", "Xóa chi tiết phiếu chi", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (SelectedPhieuChi != null)
                    {
                        //Xóa trên model
                        DataProvider.Ins.model.CHITIETPHIEUCHI.Remove(SelectedChiTietPhieuChi);

                        //Xóa trên hiển thị
                        ListChiTietPhieuChi.Remove(SelectedChiTietPhieuChi);                      
                    }
                    else
                    {
                        //Xóa trên hiển thị
                        ListChiTietPhieuChi.Remove(SelectedChiTietPhieuChi);
                    }
                    TinhTongTriGiaChiTietPhieuChi();
                    p.Close();
                }
                else return;
            });

            //Tạo mới chi tiết phiếu chi command
            TaoMoi_CTPCCommand = new RelayCommand<Object>((p) =>
            {
                if (IsEditable==false)
                {
                    MessageBox.Show("Bấm chỉnh sửa trước khi thêm chi tiết phiếu chi", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                ResetControls_CTPC();
                IsEditable_CTPC = true;
                ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                chiTietPhieuChiWindow.ShowDialog();
               
            });

            //Lưu Chi tiết phiếu chi Command
            Luu_CTPCCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(NoiDung_CTPC)
                || TriGia_CTPC == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin chi tiết phiếu chi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                if (IsEditable_CTPC == false)
                {
                    MessageBox.Show("Vui lòng chỉnh sửa thông tin trước khi lưu!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                return true;
            }, (p) =>
            {
              if (SelectedChiTietPhieuChi==null)    //Trường hợp thêm mới
                {
                    
                    {
                        // Thêm chi tiết phiếu chi vào phiếu chi đang tạo
                        if (SelectedPhieuChi == null)
                        {
                            var chiTietPhieuChiMoi = new CHITIETPHIEUCHI()
                            {
                                NOIDUNG_CTPC = NoiDung_CTPC,
                                TRIGIA_CTPC = TriGia_CTPC,
                                MA_PC = -1,
                            };

                            //Thêm chi tiết phiếu chi hiển thị
                            ListChiTietPhieuChi.Add(chiTietPhieuChiMoi);

                            MessageBox.Show("Thêm chi tiết phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        
                        }

                        // Thêm chi tiết phiếu chi vào phiếu chi đã có
                        else
                        {
                            var chiTietPhieuChiMoi = new CHITIETPHIEUCHI()
                            {
                                NOIDUNG_CTPC = NoiDung_CTPC,
                                TRIGIA_CTPC = TriGia_CTPC,
                                MA_PC = SelectedPhieuChi.MA_PC,
                            };
                            //Thêm chi tiết phiếu chi hiển thị
                            ListChiTietPhieuChi.Add(chiTietPhieuChiMoi);

                            //Thêm chi tiết phiếu chi vào model
                            DataProvider.Ins.model.CHITIETPHIEUCHI.Add(chiTietPhieuChiMoi);


                            MessageBox.Show("Thêm chi tiết phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                           
                        }
                        TinhTongTriGiaChiTietPhieuChi();
                        p.Close();
                    }
                }
              else if (SelectedChiTietPhieuChi!=null)   //Trường hợp chỉnh sửa chi tiết phiếu chi
                {
                    {
                        //Cật nhật hiển thị
                        SelectedChiTietPhieuChi.NOIDUNG_CTPC = NoiDung_CTPC;
                        SelectedChiTietPhieuChi.TRIGIA_CTPC = TriGia_CTPC;
                        if (SelectedPhieuChi != null)
                        {
                            //Cập nhật model
                            var ChiTietPhieuChiSua = DataProvider.Ins.model.CHITIETPHIEUCHI.Where(x => x.MA_CTPC == SelectedChiTietPhieuChi.MA_CTPC).SingleOrDefault();
                            ChiTietPhieuChiSua.NOIDUNG_CTPC = NoiDung_CTPC;
                            ChiTietPhieuChiSua.TRIGIA_CTPC = TriGia_CTPC;
                        }

                        MessageBox.Show("Sửa chi tiết phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        TinhTongTriGiaChiTietPhieuChi();
                        p.Close();
                    }
                }
              

            });

            //Sort command
            Sort_CTPCCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListChiTietPhieuChi);
                if (sort_CTPC)
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Ascending));
                }
                else
                {
                    view.SortDescriptions.Clear();
                    view.SortDescriptions.Add(new SortDescription(p.Tag.ToString(), ListSortDirection.Descending));
                }
                sort_CTPC = !sort_CTPC;
            });

            //Hủy command
            Huy_CTPCCommand = new RelayCommand<Window>((p) =>
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

            //Sửa Command
            Sua_CTPCCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedChiTietPhieuChi == null)
                {
                    MessageBox.Show("Không thể sửa khi đang thêm mới.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
                return true;
            }, (p) =>
            {
                IsEditable_CTPC = true;

            }
            );

            //Hiển thị Command
            HienThi_CTPCCommand = new RelayCommand<Object>((p) =>
            {
                return SelectedChiTietPhieuChi == null ? false : true;
            }, (p) =>
            {
                NoiDung_CTPC = SelectedChiTietPhieuChi.NOIDUNG_CTPC;
                TriGia_CTPC = SelectedChiTietPhieuChi.TRIGIA_CTPC;
                IsEditable_CTPC = false;

                ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                chiTietPhieuChiWindow.ShowDialog();

            });

        }

        void LoadListPhieuChi()
        {
            ListPhieuChi = new ObservableCollection<PHIEUCHI>(DataProvider.Ins.model.PHIEUCHI);

        }

        void LoadListNhanVien()
        {
            ListNhanVien = new ObservableCollection<NHANVIEN>(DataProvider.Ins.model.NHANVIEN);

        }

        public void ResetControls()
        {
            TriGia = 0; ;
            ThoiGianLap = null;
            SelectedNhanVien = null;
        }

        private void TinhTongTriGiaChiTietPhieuChi()
        {
            long tongTriGia = 0;
            foreach(CHITIETPHIEUCHI x in ListChiTietPhieuChi)
            {
                tongTriGia += (Int64)x.TRIGIA_CTPC;
            }
            TriGia = tongTriGia;
        }
        #endregion

        

        #region Chi tiết phiếu chi
        #region DataContext
        private ObservableCollection<CHITIETPHIEUCHI> _ListChiTietPhieuChi;
        public ObservableCollection<CHITIETPHIEUCHI> ListChiTietPhieuChi { get => _ListChiTietPhieuChi; set { _ListChiTietPhieuChi = value; OnPropertyChanged(); } }
        #endregion


        #region Thuộc tính binding
        private string _NoiDung_CTPC;
        public string NoiDung_CTPC { get => _NoiDung_CTPC; set { _NoiDung_CTPC = value; OnPropertyChanged(); } }

        private decimal? _TriGia_CTPC;
        public decimal? TriGia_CTPC { get => _TriGia_CTPC; set { _TriGia_CTPC = value; OnPropertyChanged(); } }

        private CHITIETPHIEUCHI _SelectedChiTietPhieuChi;
        public CHITIETPHIEUCHI SelectedChiTietPhieuChi { get => _SelectedChiTietPhieuChi; set { _SelectedChiTietPhieuChi = value; OnPropertyChanged(); } }


        private bool _IsEditable_CTPC;
        public bool IsEditable_CTPC { get => _IsEditable_CTPC; set { _IsEditable_CTPC = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        public bool sort_CTPC;

        #endregion

        #region Command binding
        public ICommand TaoMoi_CTPCCommand { get; set; }
        public ICommand Luu_CTPCCommand { get; set; }
        public ICommand Huy_CTPCCommand { get; set; }
        public ICommand Sua_CTPCCommand { get; set; }
        public ICommand HienThi_CTPCCommand { get; set; }
        public ICommand Sort_CTPCCommand { get; set; }
        public ICommand Search_CTPCCommand { get; set; }
        public ICommand Xoa_CTPCCommand { get; set; }
        #endregion

        void LoadListChiTietPhieuChi()
        {
            ListChiTietPhieuChi = new ObservableCollection<CHITIETPHIEUCHI>(DataProvider.Ins.model.CHITIETPHIEUCHI.Where(p => p.MA_PC != -2));

        }
        public void ReloadListChiTietPhieuChi()
        {
            if (SelectedPhieuChi == null)
            {
                ListChiTietPhieuChi = new ObservableCollection<CHITIETPHIEUCHI>();
                return;

            }
            else
            {
                ListChiTietPhieuChi = new ObservableCollection<CHITIETPHIEUCHI>(DataProvider.Ins.model.CHITIETPHIEUCHI.Where(x => x.MA_PC == SelectedPhieuChi.MA_PC));
            }
        }

        public void ResetControls_CTPC()
        {
            TriGia_CTPC = null;
            NoiDung_CTPC = null;
        }

        private void UnchangedAllActions()
        {
            /*
            foreach (CHITIETPHIEUCHI x in DataProvider.Ins.model.CHITIETPHIEUCHI)
            {              
                if (DataProvider.Ins.model.Entry(x).State != System.Data.Entity.EntityState.Unchanged)
                        DataProvider.Ins.model.Entry(x).Reload();
            }*/
            var changedEntries = DataProvider.Ins.model.ChangeTracker.Entries()
              .Where(x => x.State != System.Data.Entity.EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case System.Data.Entity.EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                    case System.Data.Entity.EntityState.Added:
                        entry.State = System.Data.Entity.EntityState.Detached;
                        break;
                    case System.Data.Entity.EntityState.Deleted:
                        entry.State = System.Data.Entity.EntityState.Unchanged;
                        break;
                }
            }


        }
        #endregion
    }
}
