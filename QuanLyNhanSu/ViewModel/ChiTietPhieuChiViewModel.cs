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
    class ChiTietPhieuChiViewModel : BaseViewModel
    {
        #region DataContext
        private ObservableCollection<CHITIETPHIEUCHI> _ListChiTietPhieuChi;
        public ObservableCollection<CHITIETPHIEUCHI> ListChiTietPhieuChi { get => _ListChiTietPhieuChi; set { _ListChiTietPhieuChi = value; OnPropertyChanged(); } }
        #endregion 


        #region Thuộc tính binding
        private string _NoiDung;
        public string NoiDung { get => _NoiDung; set { _NoiDung = value; OnPropertyChanged(); } }

        private decimal? _TriGia;
        public decimal? TriGia { get => _TriGia; set { _TriGia = value; OnPropertyChanged(); } }

        private ThongTinPhieuChi _SelectedThongTinPhieuChi;
        public ThongTinPhieuChi SelectedThongTinPhieuChi { get => _SelectedThongTinPhieuChi; set { _SelectedThongTinPhieuChi = value; OnPropertyChanged(); } }

        private CHITIETPHIEUCHI _SelectedChiTietPhieuChi;
        public CHITIETPHIEUCHI SelectedChiTietPhieuChi { get => _SelectedChiTietPhieuChi; set { _SelectedChiTietPhieuChi = value; OnPropertyChanged(); } }


        private bool _IsEditable;
        public bool IsEditable { get => _IsEditable; set { _IsEditable = value; OnPropertyChanged(); } }

        #endregion

        #region Thuộc tính khác
        public bool sort;

        private int TriGiaHienTai;
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

        #region List Thêm, xóa, sửa CTPC tạm


        #endregion

        public ChiTietPhieuChiViewModel()
        {
            LoadListChiTietPhieuChi();
          
            IsEditable = false;

            
            //Xóa Chi tiết phiếu chi command
            XoaCommand = new RelayCommand<Window>((p) =>
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
                    using (var transactions = DataProvider.Ins.model.Database.BeginTransaction())
                    {
                        try
                        {
                            var ctpc = DataProvider.Ins.model.CHITIETPHIEUCHI.Where(x => x.MA_CTPC == SelectedChiTietPhieuChi.MA_CTPC).FirstOrDefault();
                          
                            // update trị giá của phiếu chi đang chọn.
                            var PhieuChiSua = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedThongTinPhieuChi.PhieuChi.MA_PC).SingleOrDefault();
                            PhieuChiSua.TRIGIA_PC = PhieuChiSua.TRIGIA_PC - ctpc.TRIGIA_CTPC;                            

                            //xóa chi tiết phiếu chi đang chọn
                            DataProvider.Ins.model.CHITIETPHIEUCHI.Remove(ctpc);

                           // DataProvider.Ins.model.SaveChanges();
                            transactions.Commit();
                            MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                        
                            p.Close();

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Xóa không thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            transactions.Rollback();
                        }
                        ReloadListChiTietPhieuChi();

                    }
                }

            });

            //Tạo mới command
            TaoMoiCommand = new RelayCommand<Object>((p) =>
            {
                if (SelectedThongTinPhieuChi == null)
                {
                    MessageBox.Show("Vui lòng thêm phiếu chi trước!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
                return true;
            }, (p) =>
            {

                ResetControls();
                IsEditable = true;
                SelectedChiTietPhieuChi = null;

                ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();

                chiTietPhieuChiWindow.ShowDialog();
            });

            //Lưu Command
            LuuCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(NoiDung)
                || TriGia == null)
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin chi tiết phiếu chi!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                if (SelectedChiTietPhieuChi == null)
                {
                    var CTPCMoi = new CHITIETPHIEUCHI()
                    {
                        NOIDUNG_CTPC = NoiDung,
                        TRIGIA_CTPC = TriGia,
                        MA_PC = SelectedThongTinPhieuChi.PhieuChi.MA_PC,
                    };
                    DataProvider.Ins.model.CHITIETPHIEUCHI.Add(CTPCMoi);
                    
                    var PhieuChiSua = DataProvider.Ins.model.PHIEUCHI.Where(x => x.MA_PC == SelectedThongTinPhieuChi.PhieuChi.MA_PC).SingleOrDefault();
                    PhieuChiSua.TRIGIA_PC = PhieuChiSua.TRIGIA_PC + CTPCMoi.TRIGIA_CTPC;

                   // DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Thêm chi tiết phiếu chi mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    
                }
                else
                {
                    var CTPCSua = DataProvider.Ins.model.CHITIETPHIEUCHI.Where(x => x.MA_CTPC == SelectedChiTietPhieuChi.MA_CTPC).SingleOrDefault();
                    CTPCSua.NOIDUNG_CTPC = NoiDung;
                    CTPCSua.TRIGIA_CTPC = TriGia;
                   // DataProvider.Ins.model.SaveChanges();
                    MessageBox.Show("Cập nhật thông tin thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                ReloadListChiTietPhieuChi();
                p.Close();
            });

            //Sort command
            SortCommand = new RelayCommand<GridViewColumnHeader>((p) => { return p == null ? false : true; }, (p) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListChiTietPhieuChi);
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
                if (SelectedChiTietPhieuChi == null)
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
                return SelectedChiTietPhieuChi == null ? false : true;
            }, (p) =>
            {
                IsEditable = false;
                NoiDung = SelectedChiTietPhieuChi.NOIDUNG_CTPC;
                TriGia = SelectedChiTietPhieuChi.TRIGIA_CTPC;

                ChiTietPhieuChiWindow chiTietPhieuChiWindow = new ChiTietPhieuChiWindow();
                chiTietPhieuChiWindow.ShowDialog();

            });

        }
    

        void LoadListChiTietPhieuChi()
        {
            ListChiTietPhieuChi = new ObservableCollection<CHITIETPHIEUCHI>(DataProvider.Ins.model.CHITIETPHIEUCHI);

        }
        public void ReloadListChiTietPhieuChi()
        {
            if (SelectedThongTinPhieuChi == null)
            {
                ListChiTietPhieuChi = null;
                return;

            }
            else
                ListChiTietPhieuChi = new ObservableCollection<CHITIETPHIEUCHI>(DataProvider.Ins.model.CHITIETPHIEUCHI.Where(x => x.MA_PC == SelectedThongTinPhieuChi.PhieuChi.MA_PC));
        }

        public void ResetControls()
        {
            TriGia = null;
            NoiDung = null;
        }

    }
}
